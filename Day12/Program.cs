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

internal class Region
{
    public Region(int width, int length)
    {
        Area = width * length;
    }
    public int Area { get; private set; } 
    public List<int> Presents = new();
    public override string ToString()
    {
        return $"Area: {Area}";
    }
}

internal class DayClass
{
    List<int> _giftAreas = new();
    List<Region> _regions = new();

    public DayClass()
    {
        LoadData();
    }

    public void Part1()
    {
        int canFit = 0;
        
        foreach (Region region in _regions)
        {
            int areaNeed = 0;
            for (int i = 0; i < region.Presents.Count; i++)
            {
                areaNeed += region.Presents[i] * _giftAreas[i];
            }
            canFit += areaNeed <= region.Area ? 1 : 0;
        }

        Console.WriteLine("Part1: {0}", canFit);
    }

    public void Part2()
    {

        long rslt = 0;

        Console.WriteLine("Part2: {0}", rslt);
    }

    private void LoadData()
    {
        string inputFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\input.txt";

        if (File.Exists(inputFile))
        {
            string? line;
            StreamReader file = new StreamReader(inputFile);
            line = file.ReadLine();
            for (int i = 0; i < 6; i++)
            {
                line = file.ReadLine();
                int area = 0;
                for (int j = 0; j < 3; j++)
                {
                    foreach (char c in line)
                    {
                        area += c == '#' ? 1 : 0;
                    }
                    line = file.ReadLine();
                }
                _giftAreas.Add(area);
                if (i < 5)
                {
                    line = file.ReadLine();
                }
            }

            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split('x',':',' ');
                if (parts.Length == 9)
                {
                    int width = int.Parse(parts[0]);
                    int length = int.Parse(parts[1]);
                    Region region = new Region(width, length);
                    for (int i = 3; i <= 8; i++)
                    {
                        region.Presents.Add(int.Parse(parts[i]));
                    }
                    _regions.Add(region);
                }
            }

            file.Close();
        }
    }

}
