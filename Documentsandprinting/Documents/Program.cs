using System;
public class Document
{
    private string _title;
    private string _content;
    
    public Document(string title, string content = "")
    {
        Title = title;
        Content = content;
    }
    
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Title cannot be empty");
            _title = value;
        }
    }
    
    public string Content
    {
        get => _content;
        set => _content = value ?? "";
    }
    
    public int WordCount()
    {
        if (string.IsNullOrWhiteSpace(Content))
            return 0;
        
        return Content.Split(new[] { ' ', '\t', '\n', '\r' }, 
                           StringSplitOptions.RemoveEmptyEntries).Length;
    }
    
    public string Preview(int chars)
    {
        if (chars <= 0) return "";
        if (Content.Length == 0) return "";
        
        return Content.Length <= chars ? Content : Content.Substring(0, chars) + "...";
    }
    
    public virtual string Print()
    {
        return $"{Title}\n{Content}";
    }
}
public class Report : Document
{
    private string _author;
    
    public Report(string title, string author, string content = "") 
        : base(title, content)
    {
        Author = author;
    }
    
    public string Author
    {
        get => _author;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Author cannot be empty");
            _author = value;
        }
    }
    
    public string Header()
    {
        return $"Report by: {Author}";
    }
    
    public override string Print()
    {
        return $"{Header()}\n{base.Print()}";
    }
}
public class Note : Document
{
    private bool _pinned;
    
    public Note(string title, string content = "") : base(title, content)
    {
        _pinned = false;
    }
    
    public bool Pinned => _pinned;
    
    public void Pin()
    {
        _pinned = true;
    }
}
public class StickyNote : Note
{
    private string _color;
    
    public StickyNote(string title, string color, string content = "") 
        : base(title, content)
    {
        Color = color;
    }
    
    public string Color
    {
        get => _color;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Color cannot be empty");
            _color = value;
        }
    }
    
    public void Recolor(string newColor)
    {
        Color = newColor;
    }
    
    public override string Print()
    {
        return $"[{Color}] {base.Print()}";
    }
}
public class Program
{
    public static void Main()
    {
        var doc = new Document("Документ", "Текст документа");
        var report = new Report("Отчёт", "Соджиро", "Результаты работы");
        var note = new Note("Заметка", "Список дел");
        var sticky = new StickyNote("Напоминание", "TAKE YOUR HEART", "Позвонить");
        Console.WriteLine("=== Print() демонстрация ===");
        Console.WriteLine("Document:");
        Console.WriteLine(doc.Print());
        
        Console.WriteLine("\nReport:");
        Console.WriteLine(report.Print());
        
        Console.WriteLine("\nNote:");
        Console.WriteLine(note.Print());
        
        Console.WriteLine("\nStickyNote:");
        Console.WriteLine(sticky.Print());
        Console.WriteLine("\n=== Другие методы ===");
        Console.WriteLine($"Слов в документе: {doc.WordCount()}");
        Console.WriteLine($"Превью (5 символов): {doc.Preview(5)}");
        
        note.Pin();
        Console.WriteLine($"Заметка закреплена: {note.Pinned}");
        
        sticky.Recolor("GIT GUD");
        Console.WriteLine($"Новый цвет стикера: {sticky.Color}");
        Console.WriteLine("\n=== Полиморфизм ===");
        Document[] docs = { doc, report, note, sticky };
        foreach (var d in docs)
        {
            Console.WriteLine($"\n{d.GetType().Name}:");
            Console.WriteLine(d.Print());
        }
        Console.WriteLine("\n=== Валидация ===");
        try
        {
            var badDoc = new Document("", "текст");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }
    }
}