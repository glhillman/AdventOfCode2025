// See https://aka.ms/new-console-template for more information
using System.Reflection.PortableExecutable;
using Day10;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class DayClass
{
    public List<Day10.Machine> _machines = new();
    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int sum = 0;

        foreach (Day10.Machine machine in _machines)
        {
            sum += machine.LowestPresses();
        }

        Console.WriteLine("Part1: {0}", sum);
    }

    public void Part2()
    {

        long rslt = 0;

        Console.WriteLine("Part2: {0}", rslt);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                int target = ParseTarget(parts[0]);
                List<int> buttons = ParseButtons(parts);
                _machines.Add(new Day10.Machine((target, (1 << buttons.Count) - 1), buttons));
            }

            file.Close();
        }
    }
    public int ParseTarget(string s)
    {
        // comes in as [###..#.#..], need to convert to bitmask
        s = s.Trim('[', ']');
        int mask = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '#')
            {
                mask |= 1 << i;
            }
        }

        return mask;
    }

    public List<int> ParseButtons(string[] parts)
    {
        List<int> buttons = new();
        for (int i = 1; i < parts.Length - 1; i++)
        {
            string s = parts[i].Trim('(', ')');
            string[] offsets = s.Split(',');
            int button = 0;
            foreach (string offset in offsets)
            {
                int iOffset = int.Parse(offset);
                button |= (1 << iOffset);
            }
            buttons.Add(button);
        }

        return buttons;
    }
}
