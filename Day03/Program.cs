// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    List<List<int>> _banks = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int totalJoltage = 0;

        foreach (List<int> bank in _banks)
        {
            int first = 0;
            int firstPos = 0;
            int second = 0;
            int i = 0;
            while (i < bank.Count - 1)
            {
                if (bank[i] > first)
                {
                    first = bank[i];
                    firstPos = i;
                    if (first == 9)
                    {
                        break;
                    }
                }
                i++;
            }
            i = firstPos + 1;
            while (i < bank.Count)
            {
                if (bank[i] > second)
                {
                    second = bank[i];
                    if (second == 9)
                    {
                        break;
                    }
                }
                i++;
            }

            totalJoltage += first * 10 + second; 
        }

        Console.WriteLine("Part1: {0}", totalJoltage);
    }

    public void Part2()
    {
        long totalJoltage = 0;
        List<int> batteries = new();
        foreach (List<int> bank in _banks)
        {
            // repeatedly find the largest digit with enough digits remaining to complete the 12
            // 
            batteries.Clear();
            int bankLen = bank.Count;
            int startPos = 0;
            int endPos = bank.Count - 12;
            while (batteries.Count < 12)
            {
                int pos = FindNextDigit(bank, startPos, endPos++);
                batteries.Add(bank[pos]);
                startPos = pos + 1;
            }
            long joltage = batteries[0];
            for (int i = 1; i < 12; i++)
            {
                joltage = joltage * 10 + batteries[i];
            }
            totalJoltage += joltage;
        }

        Console.WriteLine("Part2: {0}", totalJoltage);
    }

    private int FindNextDigit(List<int> bank, int startPos, int endPos)
    {
        int digitPos = startPos;

        for (int i = startPos; i <= endPos; i++)
        {
            if (bank[i] > bank[digitPos])
            {
                digitPos = i;
            }
        }
        return digitPos;
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
                List<int> bank = new();
                foreach (char c in line)
                {
                    bank.Add(c - '0');
                }
                _banks.Add(bank);
            }

            file.Close();
        }
    }

}
