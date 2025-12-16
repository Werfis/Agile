using System;

public class Book
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int TotalPages { get; private set; }
    public int PagesRead { get; private set; }
    public Book(string title, string author, int totalPages, int pagesRead = 0)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название книги не может быть пустым");
            
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Автор не может быть пустым");
            
        if (totalPages <= 0)
            throw new ArgumentException("Общее количество страниц должно быть положительным числом");
            
        if (pagesRead < 0 || pagesRead > totalPages)
            throw new ArgumentException("Количество прочитанных страниц должно быть от 0 до общего числа страниц");
        
        Title = title;
        Author = author;
        TotalPages = totalPages;
        PagesRead = pagesRead;
    }
    public void Read(int pages)
    {
        if (pages <= 0)
            throw new ArgumentException("Количество страниц для чтения должно быть положительным числом");
            
        if (PagesRead + pages > TotalPages)
            throw new ArgumentException($"Нельзя прочитать больше {TotalPages - PagesRead} страниц");
        
        PagesRead += pages;
    }
    public override string ToString()
    {
        double percentage = TotalPages > 0 ? (double)PagesRead / TotalPages * 100 : 0;
        return $"Книга: {Title}\n" +
               $"Автор: {Author}\n" +
               $"Страниц: {PagesRead}/{TotalPages} ({percentage:F1}% прочитано)";
    }
    public bool IsFinished => PagesRead >= TotalPages;
    public int PagesLeft => TotalPages - PagesRead;
}
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Демонстрация работы класса Книга ===\n");
        try
        {
            Book book = new Book("У страха глаза велики", "Екатерина Вильмонт", 256, 5);
            Console.WriteLine("Создана новая книга:");
            Console.WriteLine(book);
            Console.WriteLine();
            Console.WriteLine("Читаем 50 страниц...");
            book.Read(50);
            Console.WriteLine($"После чтения: {book.PagesRead}/{book.TotalPages} страниц");
            Console.WriteLine($"Осталось прочитать: {book.PagesLeft} страниц");
            Console.WriteLine($"Книга завершена: {book.IsFinished}");
            Console.WriteLine();
            Console.WriteLine("Читаем еще 100 страниц...");
            book.Read(100);
            Console.WriteLine($"После чтения: {book.PagesRead}/{book.TotalPages} страниц");
            Console.WriteLine();
            Console.WriteLine("Текущее состояние книги:");
            Console.WriteLine(book);
            Console.WriteLine();
            Console.WriteLine("=== Другая книга ===");
            Book book2 = new Book("Трудно быть храбрым", "Екатерина Вильмонт", 288);
            Console.WriteLine("Новая книга (еще не начата):");
            Console.WriteLine(book2);
            Console.WriteLine();
            Console.WriteLine("=== Демонстрация обработки ошибок ===");
            try
            {
                book.Read(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
            try
            {
                Book invalidBook = new Book("", "Автор", 100);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании книги: {ex.Message}");
            }
            Console.WriteLine("\n=== Завершение чтения первой книги ===");
            int remainingPages = book.PagesLeft;
            if (remainingPages > 0)
            {
                Console.WriteLine($"Дочитываем оставшиеся {remainingPages} страниц...");
                book.Read(remainingPages);
                Console.WriteLine(book);
                Console.WriteLine($"Книга завершена: {book.IsFinished}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}