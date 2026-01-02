// Requirements:
// Concurrency: Process multiple images in parallel to save time.
// Thread Safety: Use a shared List<string> to keep track of successfully processed filenames. Ensure no race conditions occur when adding to this list.
// Error Handling: If one download fails, it shouldn't stop the entire process.
// Resource Management: Ensure you use a HttpClient correctly (hint: don't wrap it in a using block inside a loop!).
// Try to fill in the logic for ProcessImagesAsync.

public class ImageService
{
    private static readonly HttpClient _httpClient = new();
    private readonly List<string> _processedLogs = [];
    private readonly Lock _lock = new();

    public async Task ProcessImagesAsync(List<string> urls, int maxConcurrency = 10)
    {
        // Semaphore limits the number of threads entering the block
        using var semaphore = new SemaphoreSlim(maxConcurrency);

        var tasks = urls.Select(async url =>
        {
            await semaphore.WaitAsync(); // Wait for a slot to open
            try
            {
                await DownloadAndFilterAsync(url);
            }
            finally
            {
                semaphore.Release(); // Release the slot for the next task
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task DownloadAndFilterAsync(string url)
    {
        try
        {
            // perform work
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            //do stuff with the response content

            lock (_lock)
            {
                _processedLogs.Add($"Processed: {url}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {url}: {ex.Message}");
        }
    }
}