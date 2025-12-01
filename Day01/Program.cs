// See https://aka.ms/new-console-template for more information
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
    List<(char, int)> _moves = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int currPos = 50;
        int zeroCount = 0;

        foreach ((char dir, int value) move in _moves)
        {
            int value = move.value % 100;
            if (move.dir == 'L')
            {
                value = -value;
            }
            currPos += value;
            if (currPos < 0)
            {
                currPos += 100;
            }
            else
            {
                currPos %= 100;
            }
            zeroCount += currPos == 0 ? 1 : 0;
        }
        Console.WriteLine("Part1: {0}", zeroCount);
    }

    public void Part2()
    {
        int zeroCount = 0;
        int prevPos = 50;
        int currPos;

        foreach ((char dir, int value) move in _moves)
        {
            zeroCount += move.value / 100;
            int value = move.value % 100;

            if (move.dir == 'L')
            {
                value = -value;
                currPos = prevPos + value;
                if (currPos < 0)
                {
                    currPos += 100;
                    if (prevPos != 0)
                    {
                        zeroCount++;
                    }
                }
            }
            else
            {
                currPos = prevPos + value;
                if (currPos > 100)
                {
                    zeroCount++;
                }
                currPos %= 100;
            }

            zeroCount += currPos == 0 ? 1 : 0;
            prevPos = currPos;
        }

        Console.WriteLine("Part2: {0}", zeroCount);
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
                int value = Convert.ToInt32(line.Substring(1));
                _moves.Add((line[0],value));
            }

            file.Close();
        }
    }

}
