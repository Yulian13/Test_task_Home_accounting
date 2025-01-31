using BusinessLogic.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Test_task_Home_accounting.HealthChecks
{
	public class CurrentServiceHealthCheck : IHealthCheck
	{
		private FixerApiHttpService currencyService;

		public CurrentServiceHealthCheck(FixerApiHttpService currencyService)
		{
			this.currencyService = currencyService;
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) =>
			(await currencyService.CheckServiceAndApiKey())
				? HealthCheckResult.Healthy("Currency service is work")
				: HealthCheckResult.Unhealthy("Error in currency service or api key");
	}
}
