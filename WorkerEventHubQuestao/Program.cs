using WorkerEventHubQuestao;
using WorkerEventHubQuestao.Data;
using Microsoft.ApplicationInsights.DependencyCollector;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<VotacaoRepository>();
        services.AddHostedService<Worker>();

        if (!String.IsNullOrWhiteSpace(
            hostContext.Configuration["ApplicationInsights:InstrumentationKey"]))
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
                (module, o) =>
                {
                    module.EnableSqlCommandTextInstrumentation = true;
                });
        }
    })
    .Build();

await host.RunAsync();