namespace Forja.Launcher.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Game> Games { get; } = new();
    public ReactiveCommand<Game, Unit> UpdateCommand { get; }
    public ReactiveCommand<Game, Unit> PlayCommand { get; }
    public ReactiveCommand<Game, Unit> InstallCommand { get; }

    private readonly GameLaunchService _launchService = new();
    private readonly ApiService _apiService;
    
    private Game? _selectedGame;
    public Game? SelectedGame
    {
        get => _selectedGame;
        set => this.RaiseAndSetIfChanged(ref _selectedGame, value);
    }

    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;

        UpdateCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await UpdateGameAsync(game));

        PlayCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await PlayGameAsync(game));

        InstallCommand = ReactiveCommand.CreateFromTask<Game>(
            async (game) => await InstallGameAsync(game));

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
            if (game.LocalVersion == game.LatestVersion)
                return; 

            var objectPath = game.DownloadUrl;
            var destinationPath = Path.Combine("/games", game.Id.ToString(), $"version_{game.LatestVersion}.zip");

            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

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
                    var existingGame = Games.FirstOrDefault(g => g.Id == game.Id);
                    if (existingGame != null)
                    {
                        existingGame.Name = game.Name;
                        existingGame.LogoUrl = game.LogoUrl;
                        existingGame.LatestVersion = game.Versions.Select(v => v.Version).OrderByDescending(v => v).FirstOrDefault() ?? throw new InvalidOperationException();
                        existingGame.DownloadUrl = game.Versions.First(v => v.Version == existingGame.LatestVersion).StorageUrl;
                    }
                    else
                    {
                        var newGame = new Game
                        {
                            Id = game.Id,
                            Name = game.Name,
                            DownloadUrl = game.Versions.First(v => v.Version == game.Versions.Select(version => version.Version).OrderByDescending(currentVersion => currentVersion).FirstOrDefault()).StorageUrl,
                            LogoUrl = game.LogoUrl
                        };
                        Games.Add(newGame);
                    }
                }
            });

            await GameStorage.SaveAsync(Games);
        }
        catch (Exception ex)
        {
            // Add proper error handling
            Debug.WriteLine($"Error fetching games: {ex}");
        }
    }
    
    private async Task InstallGameAsync(Game game)
    {
        try
        {
            var objectPath = game.DownloadUrl;
            var destinationPath = Path.Combine("/games", game.Id.ToString(), $"version_{game.LatestVersion}.zip");

            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

            var progressReporter = new Progress<double>(p =>
            {
                game.Progress = p;
                Debug.WriteLine($"Download progress: {p:P0}");
            });

            await _apiService.DownloadFileInChunksAsync(objectPath, destinationPath, progress: progressReporter);

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
}