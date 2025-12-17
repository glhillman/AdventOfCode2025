using System;
using System.Collections.Generic;
using System.Text;

namespace Day10
{
    public class Machine
    {
        public Machine((int target, int targetMax) targetInfo, List<int> buttons)
        {
            Target = targetInfo.target;
            TargetMax = targetInfo.targetMax;
            Buttons = buttons;

            if (_nBits.Count == 0)
            {
                for (int i = 0; i <= 8192; i++)
                {
                    int nBits = 0;
                    int num = i;
                    while (num > 0)
                    {
                        nBits += num & 1;
                        num >>= 1;
                    }
                    _nBits.Add((i, nBits));
                }
            }

            // sort by ascending nBits, to try fewest button combinations first
            _nBits.Sort((a, b) => a.nBits < b.nBits ? -1 : a.nBits > b.nBits ? 1 : a.num - b.num);

        }
        public static List<(int num, int nBits)> _nBits = new();
        public int Target { get; private set; }
        public int TargetMax { get; private set; }
        public List<int> Buttons { get; set; }
        public int LowestPresses()
        {
            int testTarget = 0;
            while (testTarget != Target)
            {
                foreach ((int num, int nBits) testNum in _nBits)
                {
                    if (testNum.num <= TargetMax)
                    {
                        testTarget = 0;
                        int num = testNum.num;
                        // bit positions in num are used as index into button array
                        int offset = 0;
                        while (num > 0)
                        {
                            if ((num & 1) == 1)
                            {
                                testTarget ^= Buttons[offset];
                            }
                            num >>= 1;
                            offset++;
                        }
                        if (testTarget == Target)
                        {
                            return testNum.nBits;
                        }
                    }
                }
            }

            return -1; // Placeholder return value
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Target: {0}/{1}, Buttons: ", Target, TargetMax);
            for (int i = 0; i < Buttons.Count; i++)
            {
                sb.Append(Buttons[i]);
                if (i < Buttons.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }
    }
}
