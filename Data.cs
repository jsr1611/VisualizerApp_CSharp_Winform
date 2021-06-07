using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleDataVisualizerApp
{
    public class Data
    {
        private int sensorId;
        public int sID
        {
            get { return sensorId; }
            set { sensorId = value; }
        }

        private string sensorType;

        public string sType
        {
            get { return sensorType; }
            set { sensorType = value; }
        }


        private string[] sensorDateTime;
        private double[] sensorData;

        public Data()
        {
            sID = 0;
            sType = string.Empty;
            sensorData = new double[1];
            sensorDateTime = new string[1];

        }
        public Data(int size)
        {
            if (size < 1)
            {
                size = 1;
            }
            sID = 0;
            sType = string.Empty;
            sensorData = new double[size];
            sensorDateTime = new string[size];

        }
        public string GetDateTime(int index)
        {
            return sensorDateTime[index];
        }
        public void SetDateTime(int index, string value)
        {
            sensorDateTime[index] = value;
        }
       
        public string[] GetDateTime()
        {
            return sensorDateTime;
        }

        public void SetData(int index, double value)
        {
            sensorData[index] = value;
        }
       
        public double GetData(int index)
        {
            return sensorData[index];
        }
        public double[] GetData()
        {
            return sensorData;
        }
    }
}
