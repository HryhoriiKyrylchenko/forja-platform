namespace Forja.Launcher.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Game> Games { get; } = new();
    public ReactiveCommand<Game, Unit> UpdateCommand { get; }
    public ReactiveCommand<Game, Unit> PlayCommand { get; }

    private readonly DownloadService _downloadService = new();
    private readonly GameLaunchService _launchService = new();
    private readonly ApiService _apiService;

    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;
        
        Task.Run(async () =>
        {
            var localGames = await GameStorage.LoadAsync();
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                foreach (var game in localGames)
                    Games.Add(game);
            });

            await FetchAndUpdateGamesFromApi();
        });

        UpdateCommand = ReactiveCommand.CreateFromTask<Game>(UpdateGameAsync);

        PlayCommand = ReactiveCommand.Create<Game>(PlayGameAsync);
    }
    
    private async Task UpdateGameAsync(Game game)
    {
        game.LatestVersion = await _apiService.GetLatestVersionAsync(game.Id);
        if (game.LocalVersion != game.LatestVersion)
        {
            var url = await _apiService.GetDownloadUrlAsync(game.Id, game.LatestVersion);
            var filePath = $"/games/{game.Id}/version_{game.LatestVersion}.zip";

            await _downloadService.DownloadFileAsync(url, filePath);
            game.LocalVersion = game.LatestVersion;

            await GameStorage.SaveAsync(Games);
        }
    }

    private void PlayGameAsync(Game game)
    {
        _launchService.LaunchGame(game);
    }

    private async Task StopGame()
    {
        _launchService.StopGame();
        if (_launchService is { CurrentGame: not null, Stopwatch: not null })
                await _apiService.ReportPlayedTimeAsync(_launchService.CurrentGame.Id,
                    _launchService.Stopwatch.Elapsed);
    }
    
    private async Task FetchAndUpdateGamesFromApi()
    {
        var apiGames = await _apiService.GetAllGamesAsync();

        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            Games.Clear();
            foreach (var game in apiGames)
                Games.Add(game);
        });

        await GameStorage.SaveAsync(Games);
    }
}