using System;

namespace NotificationPrioritySystem
{
    public enum EventType
    {
        Info,
        Warning,
        Alert,
        Critical
    }
    
    public sealed class NotifyContext
    {
        public bool IsQuietHours { get; set; }
        public bool IsVIPUser { get; set; }
        public bool HasCriticalFlag { get; set; }
    }
    
    public static class PriorityCalculatorEnum
    {
        private const int MinPriority = 1;
        private const int QuietMod = -2;
        private const int VipMod = 2;
        private const int CriticalMod = 3;
        
        public static int CalculatePriority(EventType type, NotifyContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            
            int priority = type switch
            {
                EventType.Info => 1,
                EventType.Warning => 3,
                EventType.Alert => 5,
                EventType.Critical => 8,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
            
            if (ctx.IsQuietHours) priority += QuietMod;
            if (ctx.IsVIPUser) priority += VipMod;
            
            priority = Math.Max(priority, MinPriority);
            
            if (ctx.HasCriticalFlag) priority += CriticalMod;
            
            return priority;
        }
    }
    public abstract class Event
    {
        protected const int MinPriority = 1;
        protected const int QuietMod = -2;
        protected const int VipMod = 2;
        protected const int CriticalMod = 3;
        
        protected readonly int BasePriority;
        
        protected Event(int priority)
        {
            if (priority < 1) 
                throw new ArgumentException("Priority must be positive");
            BasePriority = priority;
        }
        
        public virtual int GetPriority(NotifyContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            return ApplyModifiers(BasePriority, ctx);
        }
        
        protected int ApplyModifiers(int priority, NotifyContext ctx)
        {
            if (ctx.IsQuietHours) priority += QuietMod;
            if (ctx.IsVIPUser) priority += VipMod;
            
            priority = Math.Max(priority, MinPriority);
            
            if (ctx.HasCriticalFlag) priority += CriticalMod;
            
            return priority;
        }
    }
    
    public sealed class InfoEvent : Event
    {
        public InfoEvent() : base(1) { }
    }
    
    public sealed class WarningEvent : Event
    {
        public WarningEvent() : base(3) { }
    }
    
    public sealed class AlertEvent : Event
    {
        public AlertEvent() : base(5) { }
    }
    
    public sealed class CriticalEvent : Event
    {
        public CriticalEvent() : base(8) { }
    }
    
    public static class PriorityCalculatorOop
    {
        public static int CalculatePriority(Event ev, NotifyContext ctx)
        {
            if (ev == null) throw new ArgumentNullException(nameof(ev));
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            return ev.GetPriority(ctx);
        }
    }
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== Enum Approach ===");
            TestEnum();
            
            Console.WriteLine("\n=== OOP Approach ===");
            TestOop();
            
            Console.WriteLine("\n=== Validation ===");
            Validate();
        }
        
        private static void TestEnum()
        {
            var ctx = new NotifyContext 
            { 
                IsQuietHours = true, 
                IsVIPUser = true, 
                HasCriticalFlag = false 
            };
            
            Console.WriteLine($"Info: {PriorityCalculatorEnum.CalculatePriority(EventType.Info, ctx)}");
            Console.WriteLine($"Critical: {PriorityCalculatorEnum.CalculatePriority(EventType.Critical, ctx)}");
        }
        
        private static void TestOop()
        {
            var ctx = new NotifyContext
            {
                IsQuietHours = true,
                IsVIPUser = true,
                HasCriticalFlag = true
            };
            
            Event[] events = 
            {
                new InfoEvent(),
                new WarningEvent(),
                new AlertEvent(),
                new CriticalEvent()
            };
            
            foreach (var ev in events)
            {
                Console.WriteLine($"{ev.GetType().Name}: {ev.GetPriority(ctx)}");
            }
        }
        
        private static void Validate()
        {
            var ctx = new NotifyContext
            {
                IsQuietHours = false,
                IsVIPUser = true,
                HasCriticalFlag = true
            };
            
            int enumResult = PriorityCalculatorEnum.CalculatePriority(EventType.Alert, ctx);
            int oopResult = PriorityCalculatorOop.CalculatePriority(new AlertEvent(), ctx);
            
            Console.WriteLine($"Enum: {enumResult}, OOP: {oopResult}");
            Console.WriteLine($"Match: {enumResult == oopResult}");
        }
    }
}