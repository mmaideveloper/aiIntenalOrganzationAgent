using Dovera.AI.MCP.Abstractions;

namespace Dovera.AI.MCP.External.PublicReporting;

public sealed class PublicReportingServer : ISearchableMcpServer
{
    public const string ServerName = "Dovera.AI.MCP.External.PublicReporting";

    public string Name => ServerName;

    public ToolResult GetVersion(string correlationId)
    {
        return new ToolResult("get_version", correlationId, new { version = "1.0.0" }, Name);
    }

    public ToolResult GetReports(string correlationId, DateOnly timeFrom, DateOnly to, string reportType)
    {
        var reports = new[]
        {
            new { date = "2026-01-01", value = 12343, reportType }
        };

        return new ToolResult(
            "get_reports",
            correlationId,
            new
            {
                timeFrom,
                to,
                reportType,
                data = reports
            },
            Name);
    }

    public IReadOnlyList<string> GetSupportedTools() => ["get_version", "get_reports", "search"];

    public ToolResult Search(string correlationId, string query, DateOnly? from = null, DateOnly? to = null)
    {
        return GetReports(correlationId, from ?? new DateOnly(2026, 1, 1), to ?? new DateOnly(2026, 1, 1), query);
    }
}
