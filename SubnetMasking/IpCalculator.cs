using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnetMasking
{
    public class IpCalculator
    {
        readonly public int[] LeftSideBitArr = new int[] { 128, 192, 224, 240, 248, 252, 254, 255 };
        readonly public int[] RightSideBitArr = new int[] { 1, 2, 4, 8, 16, 32, 64, 128 };

        public int[] IpAddress { get; set; }
        public int[] SubnetMask { get; set; }
        public int Prefix { get; set; }
        public int[] NetID { get; set; }
        public int[] FirstHost { get; set; }
        public int[] LastHost { get; set; }
        public int[] Broadcast { get; set; }


        private int currentNetworkEnd;
        private int octat;
        private int octatValue;
        private int segmentSize;
        public IpCalculator(int[] ip, int prefix)
        {
            IpAddress = ip;
            Prefix = prefix;
            CalculateAll();
        }

        public void CalculateSubnetFromPrefix()
        {
            SubnetMask = new int[4];
            if (Prefix <= 8)

                octat = 0;
            else if (Prefix <= 16)
                octat = 1;
            else if (Prefix <= 24)
                octat = 2;
            else
                octat = 3;

            //get number
            int subnetBits = Prefix % 8;
            if (subnetBits > 0)
            {
                octatValue = LeftSideBitArr[subnetBits - 1];
            }
            else
            {
                octatValue = 255;
            }

            //fill 255 and 0
            for (int i = 0; i < 4; i++)
            {
                if (i < octat)
                {
                    SubnetMask[i] = 255;
                }
                else if (i == octat)
                {
                    SubnetMask[i] = octatValue;
                }
                else
                {
                    SubnetMask[i] = 0;
                }

            }
            //return subnetMask;
        }

        public void CalculateNetId()
        {
            NetID = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (SubnetMask[i] == 255)
                {
                    NetID[i] = IpAddress[i];
                }
                else if (SubnetMask[i] > 0)
                {
                    //Devide to networks,
                    int netStart = 0;
                    Dictionary<int, int> networks = new Dictionary<int, int>();
                    while (netStart <= 256)
                    {
                        networks.Add(netStart, netStart + segmentSize);
                        netStart += segmentSize;
                    }

                    int _start = -420;

                    foreach (var startingNumber in networks.Keys)
                    {
                        if (IpAddress[i] >= startingNumber && IpAddress[i] < networks[startingNumber])
                        {
                            _start = startingNumber;
                            currentNetworkEnd = networks[startingNumber];
                        }
                    }
                    NetID[i] = _start;
                    //Get Current Network.
                }
                else
                {
                    NetID[i] = 0;
                }
            }
        }

        public int CalculateSegmentSize()
        {
            int res = 256 - octatValue;
            if (res == 1)
            {
                segmentSize = 255;
            }
            else
            {
                segmentSize = res;
            }
            return segmentSize;
        }

        public void CalculateHostsAndBroadcast()
        {
            Broadcast = new int[4];
            FirstHost = new int[4];
            LastHost = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (i < octat)
                {
                    FirstHost[i] = IpAddress[i];
                    LastHost[i] = IpAddress[i];
                    Broadcast[i] = IpAddress[i];
                }
                else if (i == octat)
                {
                    Broadcast[i] = currentNetworkEnd - 1;
                    if (i == 3)//last octat
                    {
                        FirstHost[i] = NetID[i] + 1;
                        LastHost[i] = Broadcast[i] - 1;
                    }
                    else
                    {
                        FirstHost[i] = NetID[i];
                        LastHost[i] = Broadcast[i]; 
                    }
                }
                else
                {
                    Broadcast[i] = 255;
                    LastHost[i] = 255;
                    if (i == 3)//last octat
                    {
                        FirstHost[i] = NetID[i] + 1;
                        LastHost[i] = Broadcast[i] - 1;
                    }                    
                }
            }
        }
        public void CalculateAll()
        {
            CalculateSubnetFromPrefix();
            CalculateSegmentSize();
            CalculateNetId();
            CalculateHostsAndBroadcast();
                        
        }

        public string StringResults()
        {
            string res = "Subnet mask:" + PrintArr(SubnetMask) + Environment.NewLine;
            res += "Net ID:" + PrintArr(NetID) + Environment.NewLine;
            res += "First Host:" + PrintArr(FirstHost) + Environment.NewLine;
            res += "Last Host:" + PrintArr(LastHost) + Environment.NewLine;
            res += "Broadcast:" + PrintArr(Broadcast) + Environment.NewLine;
            return res;
        }


        public string PrintArr(int[] arr)
        {
            string res = "";
            for (int i = 0; i < arr.Length; i++)
            {
                res += arr[i];
                if (i != arr.Length - 1)
                {
                    res += '.';
                }
            }

            return res;
        }
    }
}
