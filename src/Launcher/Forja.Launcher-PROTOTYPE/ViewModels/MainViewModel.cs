namespace Forja.Launcher.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<GameViewModel> Games { get; } = [];
    
    public ReactiveCommand<GameViewModel, Unit> SelectGameCommand { get; }

    private readonly ApiService _apiService;
    private readonly GameInstallationService _gameInstallationService;
    private readonly GameLaunchService _gameLaunchService;
    
    private GameAction _currentGameAction;
    public GameAction CurrentGameAction
    {
        get => _currentGameAction;
        set => this.RaiseAndSetIfChanged(ref _currentGameAction, value);
    }
    
    private IDisposable? _selectedGameSubscription;
    
    private GameViewModel? _selectedGame;
    public GameViewModel? SelectedGame
    {
        get => _selectedGame;
        set
        {
            if (value == _selectedGame)
                return;

            _selectedGameSubscription?.Dispose();
            
            this.RaiseAndSetIfChanged(ref _selectedGame, value);
            
            if (_selectedGame != null)
            {
                _selectedGameSubscription = _selectedGame
                    .WhenAnyPropertyChanged(
                        nameof(GameViewModel.IsInstalled),
                                                nameof(GameViewModel.IsUpdated),
                                                nameof(GameViewModel.IsRunning))
                    .Subscribe(_ => UpdateCurrentGameAction());
            }
            
            this.RaisePropertyChanged(nameof(RepairCommand));
            this.RaisePropertyChanged(nameof(DeleteCommand));
            
            UpdateCurrentGameAction();
        }
    }

    public string CurrentActionText =>
        CurrentGameAction switch
        {
            GameAction.Install => "INSTALL",
            GameAction.Update => "UPDATE",
            GameAction.Start => "START",
            GameAction.Stop => "STOP",
            _ => string.Empty
        };
    
    // public ICommand? CurrentActionCommand =>
    //     CurrentGameAction switch
    //     {
    //         GameAction.Install => SelectedGame?.InstallOrUpdateCommand,
    //         GameAction.Update => SelectedGame?.InstallOrUpdateCommand,
    //         GameAction.Start => SelectedGame?.PlayCommand,
    //         GameAction.Stop => SelectedGame?.StopCommand,
    //         _ => null
    //     };
    
    public ReactiveCommand<Unit, Unit> CurrentActionCommand { get; }
    
    public ICommand? RepairCommand => SelectedGame?.RepairGameCommand;
    public ICommand? DeleteCommand => SelectedGame?.DeleteGameCommand;
    
    public bool ShowRepairButton => SelectedGame is { IsInstalled: true, IsRunning: false, IsUpdated: true };
    public bool ShowDeleteButton => SelectedGame is { IsInstalled: true, IsRunning: false };
    
    public Bitmap DefaultLogo { get; }
    
    public MainViewModel(ApiService apiService,
        GameInstallationService gameInstallationService,
        GameLaunchService gameLaunchService)
    {
        _apiService = apiService;
        _gameInstallationService = gameInstallationService;
        _gameLaunchService = gameLaunchService;
        
        var uri = new Uri("avares://Forja.Launcher-PROTOTYPE/Assets/logo_2.png");
        DefaultLogo = new Bitmap(AssetLoader.Open(uri));
        
        SelectGameCommand = ReactiveCommand.Create<GameViewModel>(gameVm =>
        {
            foreach (var g in Games)
            {
                g.IsSelected = false;
            }
            gameVm.IsSelected = true;
            SelectedGame = gameVm;
        });
        
        CurrentActionCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedGame == null)
                return;

            switch(CurrentGameAction)
            {
                case GameAction.Install:
                case GameAction.Update:
                    SelectedGame.InstallOrUpdateCommand.Execute();
                    break;
                case GameAction.Start:
                    SelectedGame.PlayCommand.Execute();
                    break;
                case GameAction.Stop:
                    SelectedGame.StopCommand.Execute();
                    break;
                case GameAction.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
        
        this.WhenAnyValue(x => x.CurrentGameAction)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(CurrentActionText));
                this.RaisePropertyChanged(nameof(CurrentActionCommand));
            });
        
        this.WhenAnyValue(x => x.SelectedGame)
            .Subscribe(_ =>
            {
                UpdateCurrentGameAction();
            });
        
        this.WhenAnyValue(x => x.SelectedGame)
            .Where(x => x != null)
            .SelectMany(game => game!.WhenAnyValue(
                g => g.IsInstalled,
                g => g.IsRunning,
                g => g.IsUpdated,
                (_, _, _) => Unit.Default))
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(ShowRepairButton));
                this.RaisePropertyChanged(nameof(ShowDeleteButton));
            });

        
        _saveDelayCts = new CancellationTokenSource();

        InitializeGamesAsync();
        
        foreach (var game in Games)
        {
            game.LocalDataChanged += (_, _) => SaveInstalledGamesSafe();
            game.MainStatsChanged += (_, _) => UpdateRepairAndDeleteButtonsState();
        }
        
        Games.CollectionChanged += (_, args) =>
        {
            if (args.NewItems != null)
            {
                foreach (GameViewModel newGame in args.NewItems)
                {
                    newGame.LocalDataChanged += (_, _) => SaveInstalledGamesSafe();
                    newGame.MainStatsChanged += (_, _) => UpdateRepairAndDeleteButtonsState();
                }
            }
        };
    }

    private async void InitializeGamesAsync()
    {
        try
        {
            var currentPlatform = GetCurrentPlatformType();
            
            var localGames = await Task.Run(ProductStorage.LoadInstalledGamesAsync);
            List<LibraryGameModel>? apiGames = null;

            try
            {
                apiGames = await _apiService.GetAllGamesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API fetch failed: {ex}");
            }

            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                Games.Clear();
                
                if (apiGames == null || apiGames.Count == 0)
                {
                    Debug.WriteLine("API returned no games or failed. Using local data only.");

                    foreach (var localGame in localGames)
                    {
                        var gameVm = new GameViewModel(null, localGame, _gameLaunchService, _gameInstallationService)
                        {
                            IsUnavailable = true
                        };
                        Games.Add(gameVm);
                    }
                }
                else
                {
                    var apiGameDict = apiGames.ToDictionary(g => g.Id);

                    foreach (var apiGame in apiGames)
                    {
                        var localData = localGames.FirstOrDefault(lg => lg.Id == apiGame.Id);
                        localData ??= ModelMapper.MapToInstalledGame(apiGame, string.Empty);
                        localData.LogoUrl = apiGame.LogoUrl;
                        var gameVm = new GameViewModel(apiGame, localData, _gameLaunchService, _gameInstallationService)
                        {
                            IsUnavailable = !apiGame.Platforms.Contains(currentPlatform)
                        };

                        Games.Add(gameVm);
                    }

                    var missingGames = localGames.Where(local => !apiGameDict.ContainsKey(local.Id));
                    foreach (var missingLocal in missingGames)
                    {
                        var unavailableVm = new GameViewModel(null, missingLocal, _gameLaunchService, _gameInstallationService)
                        {
                            IsUnavailable = true
                        };
                        Games.Add(unavailableVm);
                    }
                }
                
                var gamesToSave = Games.Select(g => g.LocalData!).ToList();
                await ProductStorage.SaveInstalledGamesAsync(gamesToSave);
            });

            foreach (var gameVm in Games)
            {
                await gameVm.LoadLogoAsync();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error initializing games: {ex}");
        }
    }
    
    private void UpdateCurrentGameAction()
    {
        if (SelectedGame == null)
        {
            CurrentGameAction = GameAction.None;
        }
        else if (!SelectedGame.IsInstalled)
        {
            CurrentGameAction = GameAction.Install;
        }
        else if (!SelectedGame.IsUpdated)
        {
            CurrentGameAction = GameAction.Update;
        }
        else if (!SelectedGame.IsRunning)
        {
            CurrentGameAction = GameAction.Start;
        }
        else
        {
            CurrentGameAction = GameAction.Stop;
        }
    }

    private PlatformType GetCurrentPlatformType()
    {
        return Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => PlatformType.Windows,
            PlatformID.Unix => PlatformType.Linux,
            PlatformID.MacOSX => PlatformType.Mac,
            _ => PlatformType.Windows
        };
    }
    
    private CancellationTokenSource _saveDelayCts;

    private void SaveInstalledGamesSafe()
    {
        _saveDelayCts.Cancel();
        _saveDelayCts = new CancellationTokenSource();
        var token = _saveDelayCts.Token;

        Task.Delay(1000, token).ContinueWith(async t =>
        {
            if (!t.IsCanceled)
            {
                var gamesToSave = Games.Select(g => g.LocalData!).ToList();
                await ProductStorage.SaveInstalledGamesAsync(gamesToSave);
            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void UpdateRepairAndDeleteButtonsState()
    {
        this.RaisePropertyChanged(nameof(RepairCommand));
        this.RaisePropertyChanged(nameof(DeleteCommand));
    }
    
    public void RaiseSelectedGameChanged()
    {
        this.RaisePropertyChanged(nameof(SelectedGame));
    }
}