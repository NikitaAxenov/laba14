using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _14_лаба
{
    public partial class Form1 : Form
    {
        public class ForInterval
        {
            public ForInterval(int start, int end)
            {
                this.start = start;
                this.end = end;
            }
            public int start;
            public int end;
            public int value = 0;
        }
        const double b = 4294967299;
        const double m = 9223372036854775808;
        double xNext = b;
        double xBefore, xNow;
        double mean, variance, min, max; 
        int experiment, interval, size;
        double randValue;
        List<double> stat = new List<double>();
        List<ForInterval> forinterval = new List<ForInterval>();
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 20; i++)
            {
                xBefore = xNext;
                xNext = (b * xBefore) % m;
                xNow = xNext / m;
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            stat.Clear();
            forinterval.Clear();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            mean = (double)prob1.Value;
            variance = (double)prob2.Value;
            experiment = (int)numericUpDown1.Value;
            min = mean;
            max = mean;
            interval = (int)Math.Log(experiment) + 1;
            for (int i = 0; i < experiment; i++)
            {
                double tempNow;
                xBefore = xNext;
                xNext = (b * xBefore) % m;
                xNow = xNext / m;
                tempNow = xNow;
                xNext = (b * xBefore) % m;
                xNow = xNext / m;
                randValue = Math.Sqrt(variance) * (Math.Sqrt(-2 * Math.Log(tempNow)) * Math.Cos(2 * Math.PI * xNow));
                stat.Add(randValue);
                if(randValue < min)
                {
                    min = randValue;
                }
                if(randValue > max)
                {
                    max = randValue;
                }
            }
            size = (int)(max - min) / interval;
            int firstValue = (int)Math.Floor(min);
            int lastValue = firstValue + size - 1;
            for(int i = 0; i < interval; i++)
            {
                forinterval.Add(new ForInterval(firstValue, lastValue));
                firstValue = lastValue;
                lastValue = firstValue + interval - 1;
                for (int j = 0; j < stat.Count; j++)
                {
                    if (stat[j] >= forinterval[i].start && stat[j] < forinterval[i].end)
                        forinterval[i].value++;
                }
            }
            for(int i = 0; i < forinterval.Count; i++)
            {
                chart1.Series[0].Points.AddXY(forinterval[i].start, forinterval[i].value);
            }
        }
    }
}
