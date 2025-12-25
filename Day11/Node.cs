using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Day11
{
    public class Node
    {
        public Node(string id)
        {
            Id = id;
            Connected = new List<Node>();
        }
        public string Id { get; private set; }
        public List<Node> Connected { get; private set; }
        public long NPaths { get; set; } = 0;
        public bool Visited { get; set; } = false;
        public override string ToString()
        {
            return String.Format("Id: {0}, NConnected: {1}, NPaths: {2}, Visited: {3}", Id, Connected.Count, NPaths, Visited);
        }
    }
}
