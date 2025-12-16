using System;

namespace LightSensorSimulation
{
    public delegate void LightEventHandler(LightSensor sender, int lux);
    
    public class LightSensor
    {
        public event LightEventHandler? LightLevelChanged;
        
        private EventHandler<int>? blindingLightHandlers;
        private const int BlindingThreshold = 800;
        private const int MinLux = 50;
        private const int MaxLux = 1000;
        
        public event EventHandler<int> BlindingLightReached
        {
            add 
            { 
                blindingLightHandlers += value; 
                Console.WriteLine("Subscriber added to BlindingLightReached event");
            }
            remove 
            { 
                blindingLightHandlers -= value; 
                Console.WriteLine("Subscriber removed from BlindingLightReached event");
            }
        }
        
        public void Start()
        {
            Random random = new Random();
            int measurementsCount = random.Next(8, 13);
            
            Console.WriteLine($"Starting measurements: {measurementsCount} readings");
            Console.WriteLine(new string('-', 40));
            
            for (int i = 0; i < measurementsCount; i++)
            {
                int lux = random.Next(MinLux, MaxLux + 1);
                
                LightLevelChanged?.Invoke(this, lux);
                
                if (lux >= BlindingThreshold)
                {
                    blindingLightHandlers?.Invoke(this, lux);
                }
                
                System.Threading.Thread.Sleep(300);
            }
            
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Measurements completed");
        }
    }
    
    public class ConsoleDisplay
    {
        public void SubscribeToEvents(LightSensor sensor)
        {
            sensor.LightLevelChanged += OnLightLevelChanged;
            sensor.BlindingLightReached += OnBlindingLightReached;
        }
        
        public void UnsubscribeFromBlindingLight(LightSensor sensor)
        {
            sensor.BlindingLightReached -= OnBlindingLightReached;
        }
        
        private void OnLightLevelChanged(LightSensor sender, int lux)
        {
            Console.WriteLine($"Illuminance: {lux} lux");
        }
        
        private void OnBlindingLightReached(object? sender, int lux)
        {
            Console.WriteLine($"Too bright: {lux} lux — squint or reduce brightness");
        }
    }
    
    public class ComfortAdvisor
    {
        private int lowLightCount = 0;
        private const int LowLightThreshold = 200;
        
        public void SubscribeToEvents(LightSensor sensor)
        {
            sensor.LightLevelChanged += OnLightLevelChanged;
        }
        
        private void OnLightLevelChanged(LightSensor sender, int lux)
        {
            if (lux < LowLightThreshold)
            {
                lowLightCount++;
            }
        }
        
        public void Report()
        {
            Console.WriteLine("\n=== Illuminance Comfort Report ===");
            Console.WriteLine($"Low illuminance (<{LowLightThreshold} lux) occurred {lowLightCount} times");
            
            if (lowLightCount == 0)
            {
                Console.WriteLine("Illuminance level was sufficient");
            }
            else if (lowLightCount <= 3)
            {
                Console.WriteLine("Recommend occasionally increasing illuminance");
            }
            else
            {
                Console.WriteLine("Recommend increasing overall lighting level");
            }
        }
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Light Sensor Simulation ===\n");
            
            LightSensor sensor = new LightSensor();
            ConsoleDisplay display = new ConsoleDisplay();
            ComfortAdvisor advisor = new ComfortAdvisor();
            
            Console.WriteLine("Subscribing event handlers...");
            display.SubscribeToEvents(sensor);
            advisor.SubscribeToEvents(sensor);
            
            Console.WriteLine("\nStarting sensor:");
            
            sensor.Start();
            
            Console.WriteLine("\nUnsubscribing ConsoleDisplay from BlindingLightReached event...");
            display.UnsubscribeFromBlindingLight(sensor);
            
            advisor.Report();
            
            Console.WriteLine("\nProgram completed.");
            Console.ReadKey();
        }
    }
}