using System;
using System.Collections.Generic;
using System.Linq;

public class CPU
{
    public double Frequency { get; set; }
    public int Cores { get; set; }
    public CPU(double frequency, int cores)
    {
        Frequency = frequency;
        Cores = cores;
    }
    public override string ToString()
    {
        return $"CPU: {Cores} ядер, {Frequency} ГГц";
    }
}
public class Memory
{
    public int Capacity { get; set; }
    public string MemoryType { get; set; }
    public Memory(int capacity, string memoryType)
    {
        Capacity = capacity;
        MemoryType = memoryType;
    }
    public override string ToString()
    {
        return $"Memory: {Capacity} ГБ {MemoryType}";
    }
}
public class Computer
{
    public string SerialNumber { get; set; }
    public string OS { get; set; }
    public string Motherboard { get; set; }
    public CPU Cpu { get; set; }
    public List<Memory> MemoryModules { get; set; }
    public Computer(string serialNumber, string os, string motherboard, 
                    CPU cpu, List<Memory> memoryModules)
    {
        SerialNumber = serialNumber;
        OS = os;
        Motherboard = motherboard;
        Cpu = cpu;
        MemoryModules = memoryModules;
    }
    public int GetTotalMemory()
    {
        return MemoryModules.Sum(memory => memory.Capacity);
    }
    public override string ToString()
    {
        string memoryInfo = string.Join(", ", MemoryModules.Select(m => m.ToString()));
        return $"Компьютер #{SerialNumber}\n" +
               $"ОС: {OS}\n" +
               $"Материнская плата: {Motherboard}\n" +
               $"{Cpu}\n" +
               $"Память: {memoryInfo}\n" +
               $"Всего памяти: {GetTotalMemory()} ГБ";
    }
}
class Program
{
    static void Main(string[] args)
    {
        CPU cpu1 = new CPU(3.6, 8);
        List<Memory> memories1 = new List<Memory>
        {
            new Memory(16, "DDR4"),
            new Memory(16, "DDR4")
        };
        Computer computer1 = new Computer(
            serialNumber: "PC2024-001",
            os: "Windows 11 Pro",
            motherboard: "ASUS ROG Strix Z790-E",
            cpu: cpu1,
            memoryModules: memories1
        );
        Console.WriteLine(computer1);
        Console.WriteLine();
        CPU cpu2 = new CPU(2.8, 4);
        List<Memory> memories2 = new List<Memory>
        {
            new Memory(8, "DDR3")
        };
        
        Computer computer2 = new Computer(
            serialNumber: "PC2025-002",
            os: "Ubuntu 22.04",
            motherboard: "Gigabyte B450M",
            cpu: cpu2,
            memoryModules: memories2
        );
        
        Console.WriteLine(computer2);
        Console.WriteLine("\nДополнительная информация:");
        Console.WriteLine($"Общий объем памяти компьютера 1: {computer1.GetTotalMemory()} ГБ");
        Console.WriteLine($"Процессор компьютера 2: {computer2.Cpu}");
    }
}