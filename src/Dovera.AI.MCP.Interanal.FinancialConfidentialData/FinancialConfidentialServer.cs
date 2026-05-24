using Dovera.AI.MCP.Abstractions;

namespace Dovera.AI.MCP.Interanal.FinancialConfidentialData;

public sealed class FinancialConfidentialServer : ISearchableMcpServer
{
    public const string ServerName = "Dovera.AI.MCP.Interanal.FinancialConfidentialData";
    private readonly string _requiredCode;

    public FinancialConfidentialServer(string requiredCode)
    {
        _requiredCode = requiredCode;
    }

    public string Name => ServerName;

    public ToolResult GetVersion(string correlationId)
    {
        return new ToolResult("get_version", correlationId, new { version = "1.0.0" }, Name);
    }

    public ToolResult GetData(string code, string correlationId, DateOnly from, DateOnly to)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return new ToolResult("get_data", correlationId, new
            {
                requiresCode = true,
                message = "Security code is required before confidential data can be shown."
            }, Name);
        }

        if (!string.Equals(code, _requiredCode, StringComparison.Ordinal))
        {
            return new ToolResult("get_data", correlationId, new
            {
                authorized = false,
                message = "Invalid security code."
            }, Name);
        }

        return new ToolResult("get_data", correlationId, new
        {
            authorized = true,
            period = new { from, to },
            status = new
            {
                income = 3_240_000m,
                paymentsForServices = 2_760_000m,
                operatingCosts = 310_000m,
                reserves = 780_000m,
                recommendation = "Increase preventive care funding by 4% to reduce long-term high-cost claims."
            }
        }, Name);
    }

    public IReadOnlyList<string> GetSupportedTools() => ["get_version", "get_data", "search"];

    public ToolResult Search(string correlationId, string query, DateOnly? from = null, DateOnly? to = null)
    {
        return new ToolResult("search", correlationId, new
        {
            info = "Confidential dataset matched query.",
            query,
            from,
            to
        }, Name);
    }
}
