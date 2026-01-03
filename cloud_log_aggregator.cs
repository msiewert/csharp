using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// The Challenge: The Cloud Log Aggregator
// Scenario: Your service is monitoring a fleet of 500 microservices. Each service writes logs to a shared storage. You need to write a utility that identifies "Hot Services"—services that have experienced more than 50 critical errors in the last 5 minutes.

// Requirements:

// Efficiency: Use LINQ to process the logs, but ensure you aren't iterating over the data multiple times.

// Projection: The output should be a list of ServiceReport objects containing the ServiceName and the ErrorCount.

// Resilience (The "Cloud" Twist): The logs are fetched via an external API. Implement a simple Retry Pattern using a loop or a library-like logic to attempt the fetch up to 3 times if it fails.

// Parsing: Some logs might have malformed timestamps. Use DateTime.TryParse to ensure the aggregator doesn't crash on bad data.

public class LogEntry
{
    public string ServiceName { get; set; }
    public string Level { get; set; } // e.g., "INFO", "CRITICAL"
    public string Timestamp { get; set; } // Note: String format from API
}

public class CloudOpsUtility
{
    public async Task<List<ServiceReport>> GetHotServicesAsync(
        Func<CancellationToken, IAsyncEnumerable<LogEntry>> fetchLogs,
        CancellationToken ct = default)
    {
        IAsyncEnumerable<LogEntry>? logStream = null;
        int maxAttempts = 3;
        int delayMilliseconds = 1000; // Starting delay

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                // We check if the caller canceled before starting the fetch
                ct.ThrowIfCancellationRequested();

                logStream = fetchLogs(ct);

                // We must verify the stream is accessible (trigger a Read)
                // In some implementations, you might do a dummy check here
                break;
            }
            catch (Exception ex) when (attempt < maxAttempts && !(ex is OperationCanceledException))
            {
                // Exponential Backoff: 1s, 2s, 4s...
                await Task.Delay(delayMilliseconds, ct);
                delayMilliseconds *= 2;
            }
            catch
            {
                throw; // Final attempt or Cancellation failure
            }
        }

        DateTime fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
        var stats = new Dictionary<string, int>();

        if (logStream != null)
        {
            // Use await foreach for IAsyncEnumerable
            await foreach (var log in logStream.WithCancellation(ct))
            {
                if (log.Level == "CRITICAL" &&
                    DateTime.TryParse(log.Timestamp, out DateTime ts) &&
                    ts >= fiveMinutesAgo)
                {
                    stats[log.ServiceName] = stats.GetValueOrDefault(log.ServiceName) + 1;
                }
            }
        }

        return stats
            .Where(kvp => kvp.Value > 50)
            .Select(kvp => new ServiceReport { ServiceName = kvp.Key, ErrorCount = kvp.Value })
            .ToList();
    }
}

public class ServiceReport
{
    public string ServiceName { get; set; }
    public int ErrorCount { get; set; }
}