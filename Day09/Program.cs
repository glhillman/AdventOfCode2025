// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Drawing;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();

public class MinMax
{
    public MinMax(long min, long max)
    {
        Min = min;
        Max = max;
    }
    public long Min { get; set; }
    public long Max { get; set; }
    public bool Contains(long value)
    {
        return value >= Min && value <= Max;
    }

    public override string ToString()
    {
        return $"Min: {Min}, Max: {Max}";
    }
}
internal class DayClass
{
    List<(long x, long y)> _redTiles = new();
    Dictionary<long, MinMax> _rowMinMax = new(); // indexed by row (y), contains min/max for each column in the row (x's)
    Dictionary<long, MinMax> _colMinMax = new(); // indexed by col (x), contains min/max for each row in the column (y's)

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long maxArea = 0;

        for (int i = 0; i < _redTiles.Count-1; i++)
        {
            for (int j = i + 1; j < _redTiles.Count; j++)
            {
                long area = Math.Abs(_redTiles[i].x - _redTiles[j].x + 1) * Math.Abs(_redTiles[i].y - _redTiles[j].y + 1);
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }
        }

        Console.WriteLine("Part1: {0}", maxArea);
    }

    public void Part2()
    {
        long maxArea = 0;

        (long x, long y) p1 = _redTiles[0];
        (long x, long y) p2;
        for (int i = 1; i < _redTiles.Count; i++)
        {
            p2 = _redTiles[i];
            GreenOutline(p1, p2);
            p1 = p2;
        }
        // final wrap
        p2 = _redTiles[0];
        GreenOutline(p1, p2); // load greenTiles with the green outline

        int redCount = _redTiles.Count;
        for (int i = 0; i < redCount-1; i++)
        {
            for (int j = i + 1; j < redCount; j++)
            {
                (long x, long y) redTile1 = _redTiles[i];
                (long x, long y) redTile2 = _redTiles[j];

                if (redTile1 == redTile2)
                {
                    continue;
                }

                // we know the red tiles are valid corners, so do a quick test on the opposite corners
                if ((_rowMinMax[redTile1.y].Contains(redTile2.x) && _rowMinMax[redTile2.y].Contains(redTile1.x)) == false)
                {
                    continue;
                }

                long x1 = Math.Min(redTile1.x, redTile2.x);
                long x2 = Math.Max(redTile1.x, redTile2.x);
                long y1 = Math.Min(redTile1.y, redTile2.y);
                long y2 = Math.Max(redTile1.y, redTile2.y);
                long currArea = (x2 - x1 + 1) * (y2 - y1 + 1);

                // if we've already found a larger area, skip testing this one
                if (maxArea > currArea)
                {
                    continue;
                }

                // test rectagle defined by redTile1 and redTile2 for IsValid & keep if larger than maxArea
                bool isValid = true;
                long x = x1 + 1;
                long y = y1 + 1;

                // test each row of the rectangle for validity
                while (isValid && y < y2)
                {
                    isValid = _rowMinMax[y].Contains(x1) && _rowMinMax[y].Contains(x2);
                    y++;
                }
                while (isValid && x < x2)
                {
                    isValid = _colMinMax[x].Contains(y1) && _colMinMax[x].Contains(y2);
                    x++;
                }
                if (isValid)
                {
                    maxArea = currArea;
                }
            }
        }


        Console.WriteLine("Part2: {0}", maxArea);
    }

    public long XMax { get; set; } = long.MinValue;
    public long XMin { get; set; } = long.MaxValue;
    public long YMax { get; set; } = long.MinValue;
    public long YMin { get; set; } = long.MaxValue;

    private void GreenOutline((long x, long y) p1, (long x, long y) p2)
    {
        int xDelta = 0;
        int yDelta = 0;

        if (p1.x == p2.x)
        {
            xDelta = 0;
            yDelta = p1.y < p2.y ? 1 : -1;
        }
        else
        {
            yDelta = 0;
            xDelta = p1.x < p2.x ? 1 : -1;
        }
        (long x, long y) step = (p1.x + xDelta, p1.y + yDelta);

        while (step != p2)
        {
            if (_rowMinMax.TryGetValue(step.y, out MinMax? minMax)) // save min/max for each row
            {
                minMax.Min = Math.Min(minMax.Min, step.x);
                minMax.Max = Math.Max(minMax.Max, step.x);
            }
            else
            {
                _rowMinMax[step.y] = new MinMax(step.x, step.x);
            }

            if (_colMinMax.TryGetValue(step.x, out minMax)) // save min/max for each column
            {
                minMax.Min = Math.Min(minMax.Min, step.y);
                minMax.Max = Math.Max(minMax.Max, step.y);
            }
            else
            {
                _colMinMax[step.x] = new MinMax(step.y, step.y);
            }
            step = (step.x + xDelta, step.y + yDelta);
        }
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
                string[] parts = line.Split(',');
                long col = long.Parse(parts[0]);
                long row = long.Parse(parts[1]);
                MinMax? minMax;

                XMax = Math.Max(XMax, col);
                XMin = Math.Min(XMin, col);
                YMax = Math.Max(YMax, row);
                YMin = Math.Min(YMin, row);

                if (_rowMinMax.TryGetValue(row, out minMax)) // save min/max for each row
                {
                    minMax.Min = Math.Min(minMax.Min, col);
                    minMax.Max = Math.Max(minMax.Max, col);
                }
                else
                {
                    _rowMinMax[row] = new MinMax(col, col);
                }

                if (_colMinMax.TryGetValue(col, out minMax)) // save min/max for each column
                {
                    minMax.Min = Math.Min(minMax.Min, row);
                    minMax.Max = Math.Max(minMax.Max, row);
                }
                else
                {
                    _colMinMax[col] = new MinMax(row, row);
                }
                _redTiles.Add((long.Parse(parts[0]), long.Parse(parts[1])));
            }

            file.Close();
        }
    }

}
