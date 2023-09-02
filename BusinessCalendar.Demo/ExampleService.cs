using System.Globalization;
using BusinessCalendar.Client.Providers;
using Microsoft.Extensions.Hosting;

namespace BusinessCalendar.Demo;

public class ExampleService : IHostedService
{
    private readonly IWorkdayProvider _workdayProvider;

    public ExampleService(IWorkdayProvider workdayProvider)
    {
        _workdayProvider = workdayProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var input = DateTime.Today.ToString("yyyy-MM-dd");
        while (true)
        {
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var date))
            {
                var isWorkday = await _workdayProvider.IsWorkday("State_US", date);
                Console.WriteLine($"{date:yyyy-MM-dd} is workday: {isWorkday}");

                Console.Write("Write any date (yyyy-MM-dd) to check for workday:");
                input = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid input, try again..");
                Console.Write("Write any date (yyyy-MM-dd) to check for workday:");
                input = Console.ReadLine();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;;
    }
}