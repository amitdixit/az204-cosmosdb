using Azure.Data.Tables;
using Azure;

namespace CosmosDbApp;

public record Product : ITableEntity
{
    public string RowKey { get; set; } = default!;

    public string PartitionKey { get; set; } = default!;

    public string Name { get; init; } = default!;

    public int Quantity { get; init; }

    public bool Sale { get; init; }

    public ETag ETag { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; } = default!;
}