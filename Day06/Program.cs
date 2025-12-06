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
    List<List<string>> _nums = new();
    List<char> _ops = new();
    List<(int offset, int len)> _offsets = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long total = 0;
        for (int col = 0; col < _nums[0].Count; col++)
        {
            long colTotal = long.Parse(_nums[0][col]);
            for (int row = 1; row < _nums.Count; row++)
            {
                if (_ops[col] == '+')
                {
                    colTotal += long.Parse(_nums[row][col]);
                }
                else
                {
                    colTotal *= long.Parse(_nums[row][col]);
                }
            }
            total += colTotal;
        }

        Console.WriteLine("Part1: {0}", total);
    }

    public void Part2()
    {
        long total = 0;

        for (int col = 0; col < _offsets.Count; col++)
        {
            long[] colNums = new long[_offsets[col].len];

            for (int row = 0; row < _nums.Count; row++)
            {
                string sub = _nums[row][col];
                for (int digitCol = 0; digitCol < _offsets[col].len; digitCol++)
                {
                    if (sub[digitCol] != ' ')
                    {
                        colNums[digitCol] *= 10;
                        colNums[digitCol] += sub[digitCol] - '0';
                    }
                }
            }
            long colTotal = colNums[0];
            for (int i = 1; i < colNums.Length; i++)
            {
                if (_ops[col] == '*')
                {
                    colTotal *= colNums[i];
                }
                else
                {
                    colTotal += colNums[i];
                }
            }
            total += colTotal;
        }
        
        Console.WriteLine("Part2: {0}", total);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            List<string> allLines = new();

            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                allLines.Add(line);
            }
            string opLine = allLines[allLines.Count - 1];
            allLines.RemoveAt(allLines.Count - 1);
            _ops.Add(opLine[0]);
            int offset = 1;
            int offsetAnchor = 0;
            int len = 1;
            while (offset < opLine.Length)
            {
                if (opLine[offset] != ' ')
                {
                    _ops.Add(opLine[offset]);
                    _offsets.Add((offsetAnchor, len-1));
                    len = 0;
                    offsetAnchor = offset;
                }
                offset++;
                len++;
            }
            _offsets.Add((offsetAnchor, len));

            // _offsets now has the start and length of each column
            // extract the numeric strings according to their start and length
            for (int row = 0; row < allLines.Count; row++)
            {
                List<string> subs = new();
                for (int col = 0; col < _offsets.Count; col++)
                {
                    string sub = allLines[row].Substring(_offsets[col].offset, _offsets[col].len);
                    subs.Add(sub);
                }
                _nums.Add(subs);
            }

            file.Close();
        }
    }
}
