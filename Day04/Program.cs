// See https://aka.ms/new-console-template for more information
using System.Data;

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
    char[,] _map;

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int totalAdjacent = 0;

        for (int row = 1; row < _map.GetLength(0)-1; row++)
        {
            for (int col = 1; col < _map.GetLength(1)-1; col++)
            {
                if (_map[row, col] == '@')
                {
                    int adjacent = CountAdjacent(row, col);
                    if (adjacent < 4)
                    {
                        _map[row, col] = 'x';
                    }
                    totalAdjacent += adjacent < 4 ? 1 : 0;
                }
            }
        }

        Console.WriteLine("Part1: {0}", totalAdjacent);
    }

    public void Part2()
    {
        int totalAdjacent = 0;

        for (int row = 1; row < _map.GetLength(0) - 1; row++)
        {
            for (int col = 1; col < _map.GetLength(1) - 1; col++)
            {
                if (_map[row, col] == 'x')
                {
                    _map[row, col] = '.';
                    totalAdjacent += 1;
                }
            }
        }

        bool modified = true;
        while (modified)
        {
            modified = false;
            for (int row = 1; row < _map.GetLength(0) - 1; row++)
            {
                for (int col = 1; col < _map.GetLength(1) - 1; col++)
                {
                    if (_map[row, col] == '@')
                    {
                        int adjacent = CountAdjacent(row, col);
                        if (adjacent < 4)
                        {
                            _map[row, col] = '.';
                            totalAdjacent++;
                            modified = true;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Part2: {0}", totalAdjacent);
    }

    private int CountAdjacent(int row, int col)
    {
        (int row, int col)[] neighbors =
            {
                (0,-1), // left
                (0,1),  // right
                (-1,0), // above
                (1,0),  // below
                (-1,-1),// upper-left
                (-1,1), // upper-right
                (1,-1), // lower-left
                (1,1)   // lower-right
            };

        int rollCount = 0;

        for (int i = 0; i < 8; i++)
        {
            (int row, int col) offset = neighbors[i];
            char c = _map[row + offset.row, col + offset.col];
            rollCount += (c != '.') ? 1 : 0;
        }

        return rollCount;
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            int lineLen = line.Length;
            _map = new char[lineLen+2, lineLen+2];
            for (int col = 0; col < lineLen + 2; col++)
            {
                _map[0, col] = '.';
                _map[lineLen+1, col] = '.';
            }
            int row = 1;
            do
            {
                _map[row, 0] = '.';
                _map[row, lineLen + 1] = '.';
                int col = 1;
                foreach (char c in line)
                {
                    _map[row, col++] = c;
                }
                row++;
            } while ((line = file.ReadLine()) != null);

            file.Close();
        }
    }

}
