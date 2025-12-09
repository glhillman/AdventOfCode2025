// See https://aka.ms/new-console-template for more information
using Day08;

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
    List<Junction> _junctions = new();
    List<DistRec> _distances = new();
    List<HashSet<int>> _circuits = new();
    public DayClass()
    {
        LoadData();
        CalculateDistances();
        _distances.Sort((a, b) => { long diff = a.Dist - b.Dist; return diff < 0 ? -1 : diff > 0 ? 1 : 0; });
    }

    public void Part1()
    {
         ProcessRecords(0, 999, false);

        long rslt = _circuits[0].Count * _circuits[1].Count * _circuits[2].Count;

        Console.WriteLine("Part1: {0}", rslt);
    }

    public void Part2()
    {
        long rslt = ProcessRecords(1000, 5000, true);// solution will be found before this many loops

        Console.WriteLine("Part2: {0}", rslt);
    }

    private long ProcessRecords(int start, int end, bool runningMerge)
    {
        long result = 0;
        long holdID1 = _distances[0].ID1;
        long holdID2 = _distances[0].ID2;

        for (int i = start; i <= end; i++)
        {
            bool recUsed = false;
            
            DistRec rec = _distances[i];
            if (_circuits.Count > 0 && _circuits[0].Count == 1000) // 1000 indicates all connections made
            {
                Junction j1 = _junctions.First(r => r.ID == holdID1);
                Junction j2 = _junctions.First(r => r.ID == holdID2);
                result = j1.X * j2.X;
                break;
            }
            else
            {
                holdID1 = rec.ID1;
                holdID2 = rec.ID2;
            }
                // check all the circuit lists for either/both ids
                foreach (HashSet<int> circuit in _circuits)
                {
                    bool contains1 = circuit.Contains(rec.ID1);
                    bool contains2 = circuit.Contains(rec.ID2);

                    if (contains1 && contains2)
                    {
                        recUsed = true; // do nothing with it - already used
                    }
                    else if (contains1)
                    {
                        circuit.Add(rec.ID2);
                        recUsed = true;
                    }
                    else if (contains2)
                    {
                        circuit.Add(rec.ID1);
                        recUsed = true;
                    }
                }
            if (!recUsed)
            {
                HashSet<int> newCircuit = new();
                newCircuit.Add(rec.ID1);
                newCircuit.Add(rec.ID2);
                _circuits.Add(newCircuit);
            }

            if (runningMerge || i == end)
            {
                // merge the circuits for overlapping entries
                for (int ii = 0; ii < _circuits.Count; ii++)
                {
                    for (int j = 0; j < _circuits.Count; j++)
                    {
                        if (ii != j)
                        {
                            foreach (int box in _circuits[ii])
                            {
                                if (_circuits[j].Contains(box))
                                {
                                    foreach (int box2 in _circuits[ii])
                                    {
                                        _circuits[j].Add(box2);
                                    }
                                    _circuits[ii].Clear();
                                }
                            }
                        }
                    }
                }

                _circuits.Sort((a, b) => b.Count - a.Count);
                int firstEmpty = _circuits.FindIndex(r => r.Count == 0);
                if (firstEmpty > 0)
                {
                    _circuits.RemoveRange(firstEmpty, _circuits.Count - firstEmpty);
                }
            }

        }

        return result;
    }

    public void CalculateDistances()
    {
        for (int i = 0; i < _junctions.Count-1; i++)
        {
            for (int j = i + 1; j < _junctions.Count; j++)
            {
                long dist = _junctions[i].DistanceTo(_junctions[j]);
                _distances.Add(new DistRec(_junctions[i].ID, _junctions[j].ID, dist));
            }
        }
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            int id = 1;
            string? line;
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                _junctions.Add(new Junction(id++, long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
            }

            file.Close();
        }
    }

}
