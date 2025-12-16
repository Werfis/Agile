using System;

public class ServerContext
{
    public int CpuLoad { get; set; }
    public bool AlertSent { get; set; }
    public bool Restarted { get; set; }
}

public delegate void MonitorHandler(ServerContext context);

public class MonitorPipeline
{
    private MonitorHandler? _handlers;
    
    public void AddHandler(MonitorHandler handler) => _handlers += handler;
    public void RemoveHandler(MonitorHandler handler) => _handlers -= handler;
    public void Run(ServerContext context) => _handlers?.Invoke(context);
}

public static class Handlers
{
    private const int AlertLimit = 80;
    private const int RestartLimit = 95;
    private const int LoadDecrease = 10;
    
    public static void SendAlert(ServerContext ctx)
    {
        if (ctx?.CpuLoad > AlertLimit) ctx.AlertSent = true;
    }
    
    public static void RestartService(ServerContext ctx)
    {
        if (ctx?.CpuLoad > RestartLimit) ctx.Restarted = true;
    }
    
    public static readonly MonitorHandler ReduceLoad = ctx =>
    {
        if (ctx != null) ctx.CpuLoad = Math.Max(0, ctx.CpuLoad - LoadDecrease);
    };
}

class Program
{
    static void Main()
    {
        var pipeline = new MonitorPipeline();
        var server = new ServerContext { CpuLoad = 100 };
        pipeline.AddHandler(Handlers.SendAlert);
        pipeline.AddHandler(Handlers.RestartService);
        pipeline.AddHandler(Handlers.ReduceLoad);
        
        Console.WriteLine("First launch:");
        pipeline.Run(server);
        Show(server);
        server.CpuLoad = 100;
        server.AlertSent = server.Restarted = false;
        pipeline.RemoveHandler(Handlers.RestartService);
        
        Console.WriteLine("\nSecond launch (without RestartService):");
        pipeline.Run(server);
        Show(server);
    }
    
    static void Show(ServerContext s) =>
        Console.WriteLine($"CPU: {s.CpuLoad}, Alert: {s.AlertSent}, Restart: {s.Restarted}");
}