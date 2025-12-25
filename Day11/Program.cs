// See https://aka.ms/new-console-template for more information
using Day11;
using System.IO;
using System.Linq;

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
    public List<Node> _nodes = new();

    public DayClass()
    {
        LoadData();
        ResolveConnections(_nodes);
    }

    public void Part1()
    {
        int nPaths = 0;
        Node startNode = _nodes.First(r => r.Id == "you");
        CountPaths(startNode, ref nPaths);

        Console.WriteLine("Part1: {0}", nPaths);
    }

    public void Part2()
    {
        long totalPaths = 0;

        long svrTofft = CountPaths2("svr", "fft", false);
        long fftTodac = CountPaths2("fft", "dac", true);
        long dacToout = CountPaths2("dac", "out", true);

        long svrTodac = CountPaths2("svr", "dac", true);
        long dacTofft = CountPaths2("dac", "fft", true);
        long fftToout = CountPaths2("fft", "out", true);

        totalPaths = (svrTofft * fftTodac * dacToout) + (svrTodac * dacTofft * fftToout);

        Console.WriteLine("Part2: {0}",totalPaths);
    }

    private void ResolveConnections(List<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            foreach (Node node2 in nodes)
            {
                for (int i = 0; i < node2.Connected.Count; i++)
                {
                    if (node2.Connected[i].Id == node.Id)
                    {
                        node2.Connected[i] = node;
                    }
                }
            }
        }
    }

    private void CountPaths(Node topNode, ref int nPaths)
    {
        if (topNode.Id == "out")
        {
            nPaths++;
        }
        else
        {
            foreach (Node node in topNode.Connected)
            {
                CountPaths(node, ref nPaths);
            }
        }
    }

    private long CountPaths2(string from, string target, bool init)
    {
        Node startNode = _nodes.First(r => r.Id == from);
        if (init)
        {
            foreach (Node node in _nodes)
            {
                node.Visited = false;
                node.NPaths = 0;
            }
        }

        CountPaths2Recurse(startNode, target);

        return startNode.NPaths;
    }

    private void CountPaths2Recurse(Node parentNode, string target)
    {
        foreach (Node node in parentNode.Connected)
        {
            if (node.Id == target)
            {
                parentNode.NPaths =  1;
            }
            else
            {
                if (node.Visited == false)
                {
                    CountPaths2Recurse(node, target);
                }
                parentNode.NPaths += node.NPaths;
                node.Visited = true;
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
            while ((line = file.ReadLine()) != null)
            {
                string[] parts = line.Split(':', ' ');
                Node node = new Node(parts[0]);
                for (int i = 2; i < parts.Length; i++)
                {
                    Node node2 = new Node(parts[i]);
                    node.Connected.Add(node2);
                }
                _nodes.Add(node);
            }

            file.Close();
        }
    }

}
