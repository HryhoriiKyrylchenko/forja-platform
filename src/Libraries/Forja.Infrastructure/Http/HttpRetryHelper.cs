namespace Forja.Infrastructure.Http;

/// <summary>
/// Provides helper methods for executing HTTP requests with retry policies.
/// </summary>
public static class HttpRetryHelper
{
    /// <summary>
    /// Executes an HTTP request with retry logic.
    /// Handles transient network errors and unsuccessful HTTP response statuses that are not Unauthorized or Forbidden.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> instance used to send the HTTP request.</param>
    /// <param name="request">The <see cref="HttpRequestMessage"/> to be sent.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains the <see cref="HttpResponseMessage"/> received from the server.</returns>
    /// <exception cref="Exception">Thrown when the HTTP request fails due to network or server issues.</exception>
    public static async Task<HttpResponseMessage> ExecuteWithRetryAsync(
        HttpClient httpClient,
        HttpRequestMessage request)
    {
        try
        {
            return await Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r =>
                    r.StatusCode != HttpStatusCode.Unauthorized &&
                    r.StatusCode != HttpStatusCode.Forbidden &&
                    !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await httpClient.SendAsync(await CloneHttpRequestMessage(request)
                ));
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to process the HTTP request due to network or server issues.", ex);
        }
    }

    /// <summary>
    /// Creates a deep clone of an <see cref="HttpRequestMessage"/> instance, including its headers, options, and content.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/> instance to be cloned.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains the cloned <see cref="HttpRequestMessage"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided request is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when an error occurs while attempting to clone the request content or headers.</exception>
    private static async Task<HttpRequestMessage> CloneHttpRequestMessage(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version,
        };

        if (request.Content != null)
        {
            var contentBytes = await request.Content.ReadAsByteArrayAsync();
            var memoryContent = new ByteArrayContent(contentBytes);

            foreach (var header in request.Content.Headers)
            {
                memoryContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            clone.Content = memoryContent;
        }

        foreach (var header in request.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        foreach (var property in request.Options)
            clone.Options.TryAdd(property.Key, property.Value);

        return clone;
    }
}