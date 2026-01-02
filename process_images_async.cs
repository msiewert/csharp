using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


// Requirements:
// Concurrency: Process multiple images in parallel to save time.
// Thread Safety: Use a shared List<string> to keep track of successfully processed filenames. Ensure no race conditions occur when adding to this list.
// Error Handling: If one download fails, it shouldn't stop the entire process.
// Resource Management: Ensure you use a HttpClient correctly (hint: don't wrap it in a using block inside a loop!).
// Try to fill in the logic for ProcessImagesAsync.

public class ImageService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private readonly List<string> _processedLogs = new List<string>();
    private readonly object _lock = new object();

    public async Task ProcessImagesAsync(List<string> urls)
    {
        // TODO: Implement parallel processing here
    }

    private async Task DownloadAndFilterAsync(string url)
    {
        // Simulate work
        await Task.Delay(100); 
        
        lock(_lock)
        {
            _processedLogs.Add($"Processed: {url}");
        }
    }
}