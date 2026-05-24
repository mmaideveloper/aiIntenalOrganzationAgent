namespace Dovera.AI.MCP.Abstractions;

public interface IMcpServer
{
    string Name { get; }
    ToolResult GetVersion(string correlationId);
    IReadOnlyList<string> GetSupportedTools();
}

public interface ISearchableMcpServer : IMcpServer
{
    ToolResult Search(string correlationId, string query, DateOnly? from = null, DateOnly? to = null);
}

public sealed record ToolResult(string ToolName, string CorrelationId, object Data, string SourceServer);
