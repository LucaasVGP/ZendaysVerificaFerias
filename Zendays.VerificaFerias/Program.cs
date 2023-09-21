using Quartz;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);

var schedulerFactory = new StdSchedulerFactory();
var scheduler = await schedulerFactory.GetScheduler();
await scheduler.Start();

var job = JobBuilder.Create<MeuJob>().Build();


//uma vez ao dia,meia noite
ITrigger trigger = TriggerBuilder.Create()
        .StartNow()
        .WithDailyTimeIntervalSchedule(s =>
            s.WithIntervalInHours(24)
            .OnEveryDay()
            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
        )
        .Build();

await scheduler.ScheduleJob(job, trigger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.Run();

public class MeuJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Tarefa agendada executada em: " + DateTime.Now);

        HttpClient client = new()
        {
            Timeout = TimeSpan.FromMilliseconds(5000)
        };
        var result = await client.PostAsync("https://zendays.azurewebsites.net/api/v1/Usuario", null);


    }
}