namespace Forja.Launcher.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<GameViewModel> Games { get; } = [];
    
    public ReactiveCommand<GameViewModel, Unit> SelectGameCommand { get; }

    private readonly ApiService _apiService;
    
    private GameAction _currentGameAction;
    public GameAction CurrentGameAction
    {
        get => _currentGameAction;
        set => this.RaiseAndSetIfChanged(ref _currentGameAction, value);
    }
    
    private GameViewModel? _selectedGame;
    public GameViewModel? SelectedGame
    {
        get => _selectedGame;
        set => this.RaiseAndSetIfChanged(ref _selectedGame, value);
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
    
    public ICommand? CurrentActionCommand =>
        CurrentGameAction switch
        {
            GameAction.Install => SelectedGame?.InstallOrUpdateCommand,
            GameAction.Update => SelectedGame?.InstallOrUpdateCommand,
            GameAction.Start => SelectedGame?.PlayCommand,
            GameAction.Stop => SelectedGame?.StopCommand,
            _ => null
        };
    
    public ICommand? RepairCommand => SelectedGame?.RepairGameCommand;
    public ICommand? DeleteCommand => SelectedGame?.DeleteGameCommand;
    
    public Bitmap DefaultLogo { get; }
    
    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;
        
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

        InitializeGamesAsync();
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
                        var gameVm = new GameViewModel(null, _apiService, localGame)
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
                        var gameVm = new GameViewModel(apiGame, _apiService, localData)
                        {
                            IsUnavailable = !apiGame.Platforms.Contains(currentPlatform)
                        };

                        Games.Add(gameVm);
                    }

                    var missingGames = localGames.Where(local => !apiGameDict.ContainsKey(local.Id));
                    foreach (var missingLocal in missingGames)
                    {
                        var unavailableVm = new GameViewModel(null, _apiService, missingLocal)
                        {
                            IsUnavailable = true
                        };
                        Games.Add(unavailableVm);
                    }

                    var installedGamesToSave = Games
                        .Where(g => g is { IsInstalled: true, LocalData: not null })
                        .Select(g => g.LocalData!)
                        .ToList();

                    await ProductStorage.SaveInstalledGamesAsync(installedGamesToSave);
                }
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
}