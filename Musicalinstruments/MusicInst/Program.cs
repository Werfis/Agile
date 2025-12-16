using System;
public abstract class Instrument
{
    protected string instrumentName;
    protected string material;

    public Instrument(string name, string material)
    {
        this.instrumentName = name;
        this.material = material;
    }
    public abstract string PlayMusic();
}
public class Piano : Instrument
{
    private int keyCount;

    public Piano(string name, string material, int keyCount) 
        : base(name, material)
    {
        this.keyCount = keyCount;
    }

    public override string PlayMusic()
    {
        return $"Играет {instrumentName}: звучат мелодии на {keyCount} клавишах.";
    }
    public string GetPianoInfo()
    {
        return $"{instrumentName}, материал: {material}, клавиш: {keyCount}";
    }
}
public class Guitar : Instrument
{
    private int stringCount;

    public Guitar(string name, string material, int stringCount) 
        : base(name, material)
    {
        this.stringCount = stringCount;
    }

    public override string PlayMusic()
    {
        return $"Играет {instrumentName}: звенит {stringCount}-струнная гитара.";
    }
    public string GetGuitarInfo()
    {
        return $"{instrumentName}, материал: {material}, струн: {stringCount}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Instrument[] instruments = new Instrument[]
        {
            new Piano("Рояль", "Дерево, металл", 88),
            new Piano("Пианино", "Дерево", 72),
            new Guitar("Акустическая гитара", "Дерево", 6),
            new Guitar("Бас-гитара", "Дерево", 4),
            new Guitar("Электрогитара", "Дерево", 7)
        };

        Console.WriteLine("=== Музыкальные инструменты играют ===\n");
        foreach (var instrument in instruments)
        {
            Console.WriteLine(instrument.PlayMusic());
        }

        Console.WriteLine("\n=== Детальная информация об инструментах ===\n");
        Piano grandPiano = new Piano("Концертный рояль", "Чёрное дерево", 92);
        Guitar classicGuitar = new Guitar("Классическая гитара", "Дерево", 6);

        Console.WriteLine(grandPiano.GetPianoInfo());
        Console.WriteLine(grandPiano.PlayMusic());

        Console.WriteLine("\n" + classicGuitar.GetGuitarInfo());
        Console.WriteLine(classicGuitar.PlayMusic());
        Console.WriteLine("\n=== Инструменты из разных материалов ===\n");

        Piano electronicPiano = new Piano("Электронное пианино", "Пластик", 61);
        Guitar electroAcousticGuitar = new Guitar("Электроакустическая гитара", "Красное дерево", 12);

        Console.WriteLine(electronicPiano.PlayMusic());
        Console.WriteLine(electroAcousticGuitar.PlayMusic());
    }
}