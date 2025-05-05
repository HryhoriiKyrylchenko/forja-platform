namespace Forja.Launcher.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Game> Games { get; } = new();
    public ReactiveCommand<Game, Unit> UpdateCommand { get; }
    public ReactiveCommand<Game, Unit> PlayCommand { get; }
    public ReactiveCommand<Game, Unit> InstallCommand { get; }
    public ReactiveCommand<Game, Unit> StopCommand { get; }
    
    public ReactiveCommand<Game, Unit> SelectGameCommand { get; }

    private readonly GameLaunchService _launchService = new();
    private readonly ApiService _apiService;
    private readonly string _gamesRootDir;
    
    private GameAction _currentGameAction;
    public GameAction CurrentGameAction
    {
        get => _currentGameAction;
        set => this.RaiseAndSetIfChanged(ref _currentGameAction, value);
    }
    
    private Game? _selectedGame;
    public Game? SelectedGame
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
            GameAction.Install => InstallCommand,
            GameAction.Update => UpdateCommand,
            GameAction.Start => PlayCommand,
            GameAction.Stop => StopCommand,
            _ => null
        };
    
    public Bitmap DefaultLogo { get; }
    
    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;
        
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _gamesRootDir = Path.Combine(homeDirectory, "Forja", "games");
        
        var uri = new Uri("avares://Forja.Launcher-PROTOTYPE/Assets/logo_2.png");
        DefaultLogo = new Bitmap(AssetLoader.Open(uri));

        UpdateCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await UpdateGameAsync(game));

        PlayCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await PlayGameAsync(game));

        InstallCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await InstallGameAsync(game));
        
        StopCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await StopGame());
        
        SelectGameCommand = ReactiveCommand.Create<Game>(game =>
        {
            foreach (var g in Games)
            {
                g.IsSelected = false;
            }
            game.IsSelected = true;
            SelectedGame = game;
        });
        
        this.WhenAnyValue(x => x.CurrentGameAction)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(CurrentActionText));
                this.RaisePropertyChanged(nameof(CurrentActionCommand));
            });

        this.WhenAnyValue(x => x.SelectedGame)
            .Subscribe(game =>
            {
                UpdateCurrentGameAction();
            });

        InitializeGamesAsync();
    }

    private async void InitializeGamesAsync()
    {
        try
        {
            var localGames = await Task.Run(GameStorage.LoadAsync);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var game in localGames)
                {
                    Games.Add(game);
                }
            });

            await FetchAndUpdateGamesFromApi();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading games: {ex}");
        }
    }
    
    private async Task UpdateGameAsync(Game game)
    {
        try
        {
            
            if (game.LocalVersion == "0.0.0" || game.LocalVersion == game.LatestVersion)
                return; 

            var objectPath = game.DownloadUrl;
            var destinationPath = Path.Combine(Path.GetDirectoryName(game.ExecutablePath)!, $"version_{game.LatestVersion}.zip");

            var progressReporter = new Progress<double>(p =>
            {
                game.Progress = p;
                Debug.WriteLine($"Update progress: {p:P0}");
            });

            await _apiService.DownloadFileInChunksAsync(objectPath, destinationPath, progress: progressReporter);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                game.LocalVersion = game.LatestVersion;
                game.Progress = 0; 
            });

            await GameStorage.SaveAsync(Games);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating game: {ex}");
        }
    }
    
    private async Task FetchAndUpdateGamesFromApi()
    {
        try
        {
            var apiGames = await _apiService.GetAllGamesAsync();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var game in apiGames)
                {
                    var latestVersion = game.Versions
                        .Select(v => v.Version)
                        .OrderByDescending(v => v)
                        .FirstOrDefault();

                    if (latestVersion is null)
                    {
                        Debug.WriteLine($"Game '{game.Title}' has no versions available.");
                        continue;
                    }

                    var versionInfo = game.Versions.FirstOrDefault(v => v.Version == latestVersion);
                    if (versionInfo is null)
                    {
                        Debug.WriteLine($"Latest version '{latestVersion}' for game '{game.Title}' was not found in the version list.");
                        continue;
                    }

                    var existingGame = Games.FirstOrDefault(g => g.Id == game.Id);
                    if (existingGame != null)
                    {
                        existingGame.Title = game.Title;
                        existingGame.LogoUrl = game.LogoUrl;
                        existingGame.LatestVersion = latestVersion;
                        existingGame.DownloadUrl = versionInfo.StorageUrl;
                    }
                    else
                    {
                        var newGame = new Game
                        {
                            Id = game.Id,
                            Title = game.Title,
                            LatestVersion = latestVersion,
                            DownloadUrl = versionInfo.StorageUrl,
                            LogoUrl = game.LogoUrl
                        };
                        Games.Add(newGame);
                    }
                }
            });

            await GameStorage.SaveAsync(Games);
            
            foreach (var game in Games)
            {
                await game.LoadLogoAsync();
            }
        }
        catch (Exception ex)
        {
            // Add proper error handling
            Debug.WriteLine($"Error fetching games: {ex}");
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
    
    private async Task InstallGameAsync(Game game)
    {
        try
        {
            var gameFolder = Path.Combine(_gamesRootDir, game.Title);
            var destinationPath = Path.Combine(gameFolder, $"version_{game.LatestVersion}.zip");
            
            Directory.CreateDirectory(gameFolder);

            var progressReporter = new Progress<double>(p =>
            {
                game.Progress = p;
                Debug.WriteLine($"Download progress: {p:P0}");
            });

            await _apiService.DownloadFileInChunksAsync(game.DownloadUrl, destinationPath, progress: progressReporter);
            
            System.IO.Compression.ZipFile.ExtractToDirectory(destinationPath, gameFolder, overwriteFiles: true);

            File.Delete(destinationPath);

            game.LocalVersion = game.LatestVersion;
            game.ExecutablePath = destinationPath;

            await GameStorage.SaveAsync(Games);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error installing game: {ex}");
        }
    }

    private async Task PlayGameAsync(Game game)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _launchService.LaunchGame(game);
            game.IsRunning = true; 
        });
    }

    private async Task StopGame()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _launchService.StopGame();
            var currentGame = Games.FirstOrDefault(g => g.IsRunning);
            if (currentGame != null)
            {
                currentGame.IsRunning = false;
            }
        });
    }
    
    // private async Task<bool> VerifyInstallationAsync(GameVersionInfo version, string gameInstallDir)
    // {
    //     foreach (var file in version.Files)
    //     {
    //         var fullPath = Path.Combine(gameInstallDir, file.FilePath);
    //         if (!File.Exists(fullPath))
    //         {
    //             Debug.WriteLine($"Missing file: {file.FilePath}");
    //             return false;
    //         }
    //
    //         if (!await HashMatchesAsync(fullPath, file.Hash))
    //         {
    //             Debug.WriteLine($"Corrupted file: {file.FilePath}");
    //             return false;
    //         }
    //     }
    //     return true;
    // }
}