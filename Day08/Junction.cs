using System;
using System.Collections.Generic;
using System.Text;

namespace Day08
{
    public class Junction
    {
        public Junction(int iD, long x, long y, long z)
        {
            ID = iD;
            X = x;
            Y = y;
            Z = z;
        }

        public int ID { get; private set; }
        public long X { get; private set; }
        public long Y { get; private set; }
        public long Z { get; private set; }
        public long DistanceTo(Junction other)
        {
            // true distance is the square root of this full expression, but is unnecessary for our purposes. Nice to stay with longs & not doubles
            return ((X - other.X) * (X - other.X)) + ((Y - other.Y) * (Y - other.Y)) + ((Z - other.Z) * (Z - other.Z));
        }
        public override string ToString()
        {
            return string.Format("ID: {0}, X: {1}, Y: {2}, Z: {3}", ID, X, Y, Z);
        }
    }

    public class DistRec
    {
        public DistRec(int id1, int id2, long dist)
        {
            ID1 = id1;
            ID2 = id2;
            Dist = dist;
        }
        public int ID1 { get; private set; }
        public int ID2 { get; private set; }
        public long Dist { get; private set; }
        public override string ToString()
        {
            return string.Format("ID1: {0}, ID2: {1}, Dist: {2}", ID1, ID2, Dist);
        }
    }
}
