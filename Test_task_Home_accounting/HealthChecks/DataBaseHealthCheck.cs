using DataBase.Service;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Test_task_Home_accounting.HealthChecks
{
	public class DataBaseHealthCheck : IHealthCheck
	{
		protected virtual HomeAccountContext DBcontext { get; }

		public DataBaseHealthCheck(HomeAccountContext db)
		{
			DBcontext = db;
		}

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) =>
			DBcontext.Database.CanConnect()
				? Task.FromResult(HealthCheckResult.Healthy("DataBase connect is have"))
				: Task.FromResult(HealthCheckResult.Unhealthy("No dataBase connect"));
	}
}
