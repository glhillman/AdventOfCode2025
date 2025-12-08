// See https://aka.ms/new-console-template for more information
DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1and2();
//day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

internal class DayClass
{
    List<char[]> _manifold = new();
    List<long[]> _sums = new();

    public DayClass()
    {
        LoadData();
    }

     public void Part1and2()
    {
        int splitCount = 0;
        int sPos = _manifold[0].IndexOf('S');
        _manifold[1][sPos] = '|';
        _sums[1][sPos] = 1;

        for (int row = 1; row < _manifold.Count - 1; row++)
        {
            for (int col = 0; col < _manifold[row].Length; col++)
            {
                if (_manifold[row][col] == '|')
                {
                    if (_manifold[row + 1][col] == '.' || _manifold[row + 1][col] == '|')
                    {
                        _manifold[row + 1][col] = '|';
                        _sums[row + 1][col] += _sums[row][col];
                    }
                    else if (_manifold[row + 1][col] == '^')
                    {
                        _manifold[row + 1][col - 1] = '|';
                        _sums[row + 1][col - 1] += _sums[row][col];
                        _manifold[row + 1][col + 1] = '|';
                        _sums[row + 1][col + 1] += _sums[row][col];
                        splitCount++;
                    }
                }
            }
        }

        long sum = 0;
        for (int i = 0; i < _sums[0].Length; i++)
        {
            sum += _sums[_sums.Count - 1][i];
        }

        Console.WriteLine("Part1: {0}", splitCount);
        Console.WriteLine("Part2: {0}", sum);
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
                _manifold.Add(line.ToCharArray());
                _sums.Add(new long[line.Length]);
            }

            file.Close();
        }
    }

}
