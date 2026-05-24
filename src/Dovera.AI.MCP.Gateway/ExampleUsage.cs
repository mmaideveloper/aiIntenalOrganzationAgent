using Dovera.AI.MCP.External.PublicReporting;
using Dovera.AI.MCP.Gateway;
using Dovera.AI.MCP.Interanal.FinancialConfidentialData;

var publicServer = new PublicReportingServer();
var financialServer = new FinancialConfidentialServer(requiredCode: "FIN-2026");
var gateway = new GatewayServer(apiKey: "GW-API-KEY", publicServer, financialServer);

var correlationId = Guid.NewGuid().ToString("N");
var searchResult = gateway.Search(correlationId, "yearly-summary", "GW-API-KEY", new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));

Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(searchResult));
