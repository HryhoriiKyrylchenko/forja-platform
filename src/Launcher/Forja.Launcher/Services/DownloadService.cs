namespace Forja.Launcher.Services;

public class DownloadService
{
    private readonly HttpClient _http = new();

    public async Task DownloadFileAsync(string url, string destinationPath, IProgress<double>? progress = null)
    {
        using var response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        var canReportProgress = totalBytes != -1 && progress != null;

        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = File.Create(destinationPath);
        var buffer = new byte[81920];
        long totalRead = 0;

        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            totalRead += bytesRead;
            if (canReportProgress)
                progress?.Report((double)totalRead / totalBytes);
        }
    }
}