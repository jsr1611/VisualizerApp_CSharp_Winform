using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleDataVisualizerApp.Models
{
    public class DeviceRepository : IDeviceRepository
    {  
        public List<Device> devices = null;
        public DeviceRepository()
        {
            devices = new List<Device>();
        }
        public Device GetDevice(int id, string type)
        {
            return devices.FirstOrDefault(device => device.S_ID.Equals(id) && device.S_Type.Equals(type));
        }
    }
}
