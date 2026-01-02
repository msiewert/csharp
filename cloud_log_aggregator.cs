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
    public async Task<List<ServiceReport>> GetHotServicesAsync(Func<Task<List<LogEntry>>> fetchLogs)
    {
        List<LogEntry> logs = null;
        
        // TODO: Implement a Retry Pattern (3 attempts) to call fetchLogs()
        
        // TODO: Use LINQ to find services with > 50 "CRITICAL" logs in last 5 mins
        // Hint: Remember to parse the Timestamp safely
        
        return null;
    }
}

public class ServiceReport
{
    public string ServiceName { get; set; }
    public int ErrorCount { get; set; }
}