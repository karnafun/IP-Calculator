using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubnetMasking
{
    public partial class Form1 : Form
    {
        public int[] LeftSideBitArr = new int[] { 128, 192, 224, 240, 248, 252, 254, 255 };
        public int[] RightSideBitArr = new int[] { 1, 2, 4, 8, 16, 32, 64, 128 };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Find the relevant octat


            //Check the number for the octat
            //create subnet mask array
            //calculate the splits/segments
            //check which segment we are on.
            // calculate the segment results (ID, First host, last host, broadcast.)

        }



        public string GetIPClass(int[] ip, int prefix)
        {

            switch (prefix)
            {
                case 6:
                    return "A";
                case 16:
                    return "B";
                case 24:
                    return "C";


                default:
                    return "Not CIDER";
                    break;
            }
        }

       
        private int[] GetSubnetFromPrefix(int prefix)
        {
            int[] subnetMask = new int[4];
            int octat;
            if (prefix <= 8)
                octat = 0;
            else if (prefix <= 16)
                octat = 1;
            else if (prefix <= 24)
                octat = 2;
            else
                octat = 3;

            //get number
            int subnetBits = prefix % 8;

            int octatValue;
            if (subnetBits>0)
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
                if (i<octat )
                {
                    subnetMask[i] = 255;
                }
                else if (i == octat)
                {
                    subnetMask[i] = octatValue;
                }
                else
                {
                    subnetMask[i] = 0;
                }

            }
            return subnetMask;
        }

        public int[] GetIpFromStr(string ipStr)
        {

            int[] ipAddress = new int[4];
            string[] ipStrArr = ipStr.Split('.');
            try
            {
                for (int i = 0; i < ipStrArr.Length; i++)
                {
                    ipAddress[i] = int.Parse(ipStrArr[i]);
                }
                return ipAddress;
            }
            catch (Exception)
            {
                lbl_res.Text = "Invalid ip address ! ";
                return null;
            }
        }


        public string Stringify(int[] arr)
        {
            string res = "";
            for (int i = 0; i < arr.Length; i++)
            {
                res += arr[i];
                if (i!=arr.Length-1)
                {
                    res += '.';
                }
            }

            return res;
        }

        public int[] GetNetId(int[] ip, int[] sm)
        {
            int[] netId = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (sm[i]==255)
                {
                    netId[i] = ip[i];
                }
                else if (sm[i]>0)
                {

                }
                else
                {
                    netId[i] = 0;
                }
            }
            return null;
        }
        private void btn_submit_Click(object sender, EventArgs e)
        {

            int[] ip = GetIpFromStr(txt_ip.Text);
            int prefix = int.Parse(txt_prefix.Text);
            IpCalculator ipCalc = new IpCalculator(ip, prefix);
            string res = ipCalc.StringResults();
            MessageBox.Show(res);

        }
    }
}

