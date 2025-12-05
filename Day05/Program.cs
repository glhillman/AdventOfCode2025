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
    public class IDRange
    {
        public IDRange(long first, long last)
        {
            First = first;
            Last = last;
        }
        public long First { get; set; }
        public long Last { get; set; }

        public int SortOrder(IDRange other)
        {
            long diff = other.First - First;
            diff = diff < 0 ? -1 : diff > 0 ? 1 : 0;
            
            if (diff == 0)
            {
                diff = other.Last - Last;
                diff = diff < 0 ? -1 : diff > 0 ? 1 : 0;
            }

            return (int)diff;  
        }

        public bool SameAs(IDRange other)
        {
            return First == other.First && Last == other.Last;
        }

        public override string ToString()
        {
            return string.Format("first: {0}, last: {1}", First, Last);
        }
    }

    List<IDRange> _idRanges = new();
    List<long> _ids = new();

    public DayClass()
    {
        LoadData();
        _idRanges.Sort((a,b) => b.SortOrder(a));
        ConsolidateRanges();
    }

    public void Part1()
    {
        int nFresh = 0;
        foreach (long id in _ids)
        {
            try
            {
                var foundRange = _idRanges.First(r => id >= r.First && id <= r.Last);
                nFresh++;
            }
            catch
            {
            }
        }

        Console.WriteLine("Part1: {0}", nFresh);
    }

    public void Part2()
    {
        long nFresh = 0;
        foreach (IDRange idRange in _idRanges)
        {
            nFresh += (idRange.Last - idRange.First) + 1;
        }

       Console.WriteLine("Part2: {0}", nFresh);
    }

    private void ConsolidateRanges()
    {
        RemoveDuplicates();
        ExpandSameFirst();
        JoinConsecutives();
        JoinOverlappingLast();
        DeleteFullyContained();
    }

    private void RemoveDuplicates()
    {
        int index = 0;
        while (index < _idRanges.Count - 1)
        {
            if (_idRanges[index].SameAs(_idRanges[index + 1]))
            {
                _idRanges.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
    }

    private void ExpandSameFirst()
    {
        int index = 0;
        while (index < _idRanges.Count - 1)
        {
            if (_idRanges[index].First == _idRanges[index + 1].First)
            {
                _idRanges[index].Last = Math.Max(_idRanges[index].Last, _idRanges[index + 1].Last);
                _idRanges.RemoveAt(index + 1);
            }
            else
            {
                index++;
            }
        }
    }

    private void JoinConsecutives()
    {
        int index = 0;
        while (index < _idRanges.Count - 1)
        {
            if (_idRanges[index+1].First == _idRanges[index].Last + 1)
            {
                _idRanges[index].Last = _idRanges[index + 1].Last;
                _idRanges.RemoveAt(index + 1);
            }
            else
            {
                index++;
            }
        }
    }

    private void JoinOverlappingLast()
    {
        int index = 0;
        while (index < _idRanges.Count - 1)
        {
            if (_idRanges[index].Last >= _idRanges[index + 1].First)
            {
                _idRanges[index].Last = Math.Max(_idRanges[index].Last, _idRanges[index + 1].Last);
                _idRanges.RemoveAt(index + 1);
            }
            else
            {
                index++;
            }
        }
    }

    private void DeleteFullyContained()
    {
        int index = 0;
        while (index < _idRanges.Count - 1)
        {
            if (_idRanges[index].First <= _idRanges[index+1].First &&
                _idRanges[index].Last >= _idRanges[index+1].Last)
            {
                _idRanges.RemoveAt(index + 1);
            }
            else
            {
                index++;
            }
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null && line.Length > 0)
            {
                string[] parts = line.Split('-');
                _idRanges.Add(new IDRange(long.Parse(parts[0]), long.Parse(parts[1])));
            }

            while ((line = file.ReadLine()) != null)
            {
                _ids.Add(long.Parse(line));
            }

            file.Close();
        }
    }

}
