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

public struct NumRange
{
    public NumRange(long first, long last)
    {
        First = first;
        Last = last;
    }
    public long First { get; private set; }
    public long Last { get; private set; }
}

internal class DayClass
{
    List<NumRange> _values = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        long invalidSum = 0;

        for (int i = 0; i < _values.Count; i++) 
        {
            for (long value = _values[i].First; value <= _values[i].Last; value++)
            {
                int nDigits = NDigits(value);
                if (nDigits % 2 == 0)
                {
                    long divisor = 10;
                    int j = nDigits / 2 - 1;
                    while (j-- > 0)
                    { 
                        divisor *= 10;
                    }
                    if (value % divisor == value / divisor)
                    {
                        invalidSum += value;
                    }
                }
            }
        }

        Console.WriteLine("Part1: {0}", invalidSum);
    }

    static public int NDigits(long l)
    {
        return (int)(Math.Log10(l) + 1);
    }

    public void Part2()
    {
        long invalidSum = 0;

        for (int i = 0; i < _values.Count; i++)
        {
            for (long value = _values[i].First; value <= _values[i].Last; value++)
            {
                int nDigits = NDigits(value);
                int subMax = nDigits / 2;
                for (int iSubLen = 1; iSubLen <= subMax; iSubLen++)
                {
                    if (nDigits % iSubLen == 0)
                    {
                        long tempValue = value;
                        long saveValue = value;
                        long divisor = 10;
                        int j = iSubLen - 1;
                        while (j-- > 0)
                        {
                            divisor *= 10;
                        }

                        long toMatch = tempValue % divisor;
                        tempValue /= divisor;
                        bool isInValid = true;
                        while (isInValid && tempValue > 0)
                        {
                            long nextBlock = tempValue % divisor;
                            isInValid = toMatch == nextBlock;
                            if (isInValid)
                            {
                                tempValue /= divisor;
                            }
                        }
                        if (isInValid)
                        {
                            invalidSum += saveValue;
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
                _values.Add(new NumRange(long.Parse(subParts[0]), long.Parse(subParts[1])));
            }

            file.Close();
        }
    }

}
