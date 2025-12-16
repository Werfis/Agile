using System;

public interface IOrder
{
    string Customer { get; set; }
    int TotalCents { get; set; }
    void ApplyDiscount(int percent);
}

public interface ILoyalty
{
    int LoyaltyPoints { get; set; }
    void AddPoints(int amount);
}

public class SimpleOrder : IOrder
{
    private string _customer;
    private int _totalCents;
    private const int MaxPercent = 100;
    private const int MinPercent = 0;

    public string Customer
    {
        get => _customer;
        set => _customer = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int TotalCents
    {
        get => _totalCents;
        set => _totalCents = value >= 0 ? value : 
            throw new ArgumentException("Total cannot be negative");
    }

    public void ApplyDiscount(int percent)
    {
        int validPercent = Math.Clamp(percent, MinPercent, MaxPercent);
        int discount = _totalCents * validPercent / MaxPercent;
        TotalCents = Math.Max(_totalCents - discount, 0);
    }

    public SimpleOrder(string customer, int totalCents)
    {
        Customer = customer;
        TotalCents = totalCents;
    }
}

public class MemberOrder : IOrder, ILoyalty
{
    private const int BonusDivisor = 10;
    private const int MaxPercent = 100;
    private const int MinPercent = 0;
    
    private string _customer;
    private int _totalCents;
    private int _loyaltyPoints;
    private readonly int _minBillCents;

    public string Customer
    {
        get => _customer;
        set => _customer = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int TotalCents
    {
        get => _totalCents;
        set => _totalCents = value >= 0 ? value : 
            throw new ArgumentException("Total cannot be negative");
    }

    public int LoyaltyPoints
    {
        get => _loyaltyPoints;
        set => _loyaltyPoints = value >= 0 ? value : 
            throw new ArgumentException("Points cannot be negative");
    }

    public int MinBillCents => _minBillCents;

    public void ApplyDiscount(int percent)
    {
        int validPercent = Math.Clamp(percent, MinPercent, MaxPercent);
        int discount = _totalCents * validPercent / MaxPercent;
        int newTotal = Math.Max(_totalCents - discount, _minBillCents);
        
        if (newTotal < _totalCents)
        {
            AddPoints(discount / BonusDivisor);
        }
        
        TotalCents = newTotal;
    }

    public void AddPoints(int amount)
    {
        int validAmount = Math.Max(amount, 0);
        LoyaltyPoints += validAmount;
    }

    public MemberOrder(string customer, int totalCents, int minBillCents)
    {
        if (minBillCents < 0)
        {
            throw new ArgumentException("Min bill cannot be negative");
        }
        
        Customer = customer;
        TotalCents = totalCents;
        _minBillCents = minBillCents;
        _loyaltyPoints = 0;
    }
}

public class Program
{
    public static void Main()
    {
        TestSimpleOrder();
        TestMemberOrder();
    }

    private static void TestSimpleOrder()
    {
        Console.WriteLine("=== SimpleOrder ===");
        var simple = new SimpleOrder("Рен Амамия", 100000);
        Console.WriteLine($"Начало: {simple.TotalCents}");
        simple.ApplyDiscount(20);
        Console.WriteLine($"После 20%: {simple.TotalCents}");
        simple.ApplyDiscount(100);
        Console.WriteLine($"После 100%: {simple.TotalCents}");
    }

    private static void TestMemberOrder()
    {
        Console.WriteLine("\n=== MemberOrder ===");
        var member = new MemberOrder("Футаба Сакура", 100000, 50000);
        Console.WriteLine($"Начало: {member.TotalCents}, минимум: {member.MinBillCents}");
        
        member.ApplyDiscount(30);
        Console.WriteLine($"После 30%: {member.TotalCents}, баллы: {member.LoyaltyPoints}");
        
        member.ApplyDiscount(60);
        Console.WriteLine($"После 60%: {member.TotalCents}, баллы: {member.LoyaltyPoints}");
    }
}