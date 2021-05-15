using System;
using System.Text.Json.Serialization;

namespace ExtractApifyResults.Contracts.LastTaskRunContract
{
    public class LastTaskRunContract
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Meta
    {
        [JsonPropertyName("origin")]
        public string Origin { get; set; }

        [JsonPropertyName("clientIp")]
        public string ClientIp { get; set; }

        [JsonPropertyName("userAgent")]
        public string UserAgent { get; set; }
    }

    public class Stats
    {
        [JsonPropertyName("inputBodyLen")]
        public int InputBodyLen { get; set; }

        [JsonPropertyName("restartCount")]
        public int RestartCount { get; set; }

        [JsonPropertyName("resurrectCount")]
        public int ResurrectCount { get; set; }

        [JsonPropertyName("memAvgBytes")]
        public double MemAvgBytes { get; set; }

        [JsonPropertyName("memMaxBytes")]
        public int MemMaxBytes { get; set; }

        [JsonPropertyName("memCurrentBytes")]
        public int MemCurrentBytes { get; set; }

        [JsonPropertyName("cpuAvgUsage")]
        public double CpuAvgUsage { get; set; }

        [JsonPropertyName("cpuMaxUsage")]
        public double CpuMaxUsage { get; set; }

        [JsonPropertyName("cpuCurrentUsage")]
        public int CpuCurrentUsage { get; set; }

        [JsonPropertyName("netRxBytes")]
        public int NetRxBytes { get; set; }

        [JsonPropertyName("netTxBytes")]
        public int NetTxBytes { get; set; }

        [JsonPropertyName("durationMillis")]
        public int DurationMillis { get; set; }

        [JsonPropertyName("runTimeSecs")]
        public double RunTimeSecs { get; set; }

        [JsonPropertyName("metamorph")]
        public int Metamorph { get; set; }

        [JsonPropertyName("computeUnits")]
        public double ComputeUnits { get; set; }
    }

    public class Options
    {
        [JsonPropertyName("build")]
        public string Build { get; set; }

        [JsonPropertyName("timeoutSecs")]
        public int TimeoutSecs { get; set; }

        [JsonPropertyName("memoryMbytes")]
        public int MemoryMbytes { get; set; }

        [JsonPropertyName("diskMbytes")]
        public int DiskMbytes { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("actId")]
        public string ActId { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("actorTaskId")]
        public string ActorTaskId { get; set; }

        [JsonPropertyName("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("finishedAt")]
        public DateTime FinishedAt { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("options")]
        public Options Options { get; set; }

        [JsonPropertyName("buildId")]
        public string BuildId { get; set; }

        [JsonPropertyName("exitCode")]
        public int ExitCode { get; set; }

        [JsonPropertyName("defaultKeyValueStoreId")]
        public string DefaultKeyValueStoreId { get; set; }

        [JsonPropertyName("defaultDatasetId")]
        public string DefaultDatasetId { get; set; }

        [JsonPropertyName("defaultRequestQueueId")]
        public string DefaultRequestQueueId { get; set; }

        [JsonPropertyName("buildNumber")]
        public string BuildNumber { get; set; }

        [JsonPropertyName("containerUrl")]
        public string ContainerUrl { get; set; }
    }
}