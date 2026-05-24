using Dovera.AI.MCP.Abstractions;
using Dovera.AI.MCP.External.PublicReporting;
using Dovera.AI.MCP.Interanal.FinancialConfidentialData;

namespace Dovera.AI.MCP.Gateway;

public sealed class GatewayServer : IMcpServer
{
    public const string ServerName = "Dovera.AI.MCP.Gateway";

    private readonly string _apiKey;
    private readonly PublicReportingServer _publicReporting;
    private readonly FinancialConfidentialServer _financialConfidential;

    public GatewayServer(
        string apiKey,
        PublicReportingServer publicReporting,
        FinancialConfidentialServer financialConfidential)
    {
        _apiKey = apiKey;
        _publicReporting = publicReporting;
        _financialConfidential = financialConfidential;
    }

    public string Name => ServerName;

    public IReadOnlyList<string> GetSupportedTools() =>
    [
        "get_version",
        "search",
        "get_reports",
        "get_financial_data",
        "list_servers"
    ];

    public ToolResult GetVersion(string correlationId)
    {
        return new ToolResult("get_version", correlationId, new { version = "1.0.0" }, Name);
    }

    public ToolResult ListServers(string correlationId, string apiKey)
    {
        if (!ValidateApiKey(apiKey, correlationId, out var error))
        {
            return error!;
        }

        return new ToolResult("list_servers", correlationId, new
        {
            gateway = Name,
            subServers = new[] { _publicReporting.Name, _financialConfidential.Name }
        }, Name);
    }

    public ToolResult Search(string correlationId, string query, string apiKey, DateOnly? from = null, DateOnly? to = null)
    {
        if (!ValidateApiKey(apiKey, correlationId, out var error))
        {
            return error!;
        }

        var results = new[]
        {
            _publicReporting.Search(correlationId, query, from, to),
            _financialConfidential.Search(correlationId, query, from, to)
        };

        return new ToolResult("search", correlationId, new { query, from, to, results }, Name);
    }

    public ToolResult GetReports(string correlationId, string apiKey, DateOnly from, DateOnly to, string reportType)
    {
        if (!ValidateApiKey(apiKey, correlationId, out var error))
        {
            return error!;
        }

        return _publicReporting.GetReports(correlationId, from, to, reportType);
    }

    public ToolResult GetFinancialData(string correlationId, string apiKey, string code, DateOnly from, DateOnly to)
    {
        if (!ValidateApiKey(apiKey, correlationId, out var error))
        {
            return error!;
        }

        return _financialConfidential.GetData(code, correlationId, from, to);
    }

    private bool ValidateApiKey(string apiKey, string correlationId, out ToolResult? error)
    {
        if (string.Equals(apiKey, _apiKey, StringComparison.Ordinal))
        {
            error = null;
            return true;
        }

        error = new ToolResult("auth", correlationId, new
        {
            authorized = false,
            message = "Invalid API key for gateway server."
        }, Name);
        return false;
    }
}
