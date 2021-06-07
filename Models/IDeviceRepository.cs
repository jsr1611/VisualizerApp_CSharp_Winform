using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleDataVisualizerApp.Models
{
    public interface IDeviceRepository
    {
        Device GetDevice(int id, string type);
    }
}
