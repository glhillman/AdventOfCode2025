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
    List<(long first, long last)> _values = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long invalidSum = 0;

        foreach ((long first, long last) pair in _values)
        {
            for (long i = pair.first; i <= pair.last; i++)
            {
                string iStr = i.ToString();
                if (iStr.Length % 2 == 0)
                {
                    int mid = iStr.Length / 2;
                    if (iStr.Substring(0, mid) == iStr.Substring(mid))
                    {
                        invalidSum += i;
                    }
                }
            }
        }

        Console.WriteLine("Part1: {0}", invalidSum);
    }

    public void Part2()
    {
        long invalidSum = 0;

        foreach ((long first, long last) pair in _values)
        {
            for (long i = pair.first; i <= pair.last; i++)
            {
                string iStr = i.ToString();
                int iStrLen = iStr.Length;
                int subMax = iStr.Length / 2;
                for (int iSubLen = 1; iSubLen <= subMax; iSubLen++)
                {
                    if (iStrLen % iSubLen == 0)
                    {
                        string toMatch = iStr.Substring(0, iSubLen);
                        bool isInValid = true;
                        int iSubIndex = iSubLen;
                        while (isInValid && iSubIndex < iStrLen)
                        {
                            isInValid = toMatch == iStr.Substring(iSubIndex, iSubLen);
                            if (isInValid)
                            {
                                iSubIndex += iSubLen;
                            }
                        }
                        if (isInValid)
                        {
                            invalidSum += i;
                            break;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Part2: {0}", invalidSum);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            string[] parts = line.Split(',');
            foreach (string part in parts)
            {
                string[] subParts = part.Split("-");
                _values.Add((long.Parse(subParts[0]), long.Parse(subParts[1])));
            }

            file.Close();
        }
    }

}
