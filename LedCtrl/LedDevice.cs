
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace LedCtrl
{
    public abstract class LedDevice
    {

        protected static bool IsOnRaspi
        {
            get
            {
                bool isRaspi;
                bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                if (isLinux)
                {
                    try
                    {
                        string cpuinfo = File.ReadAllText("/proc/cpuinfo");
                        isRaspi = cpuinfo.Contains("Raspberry Pi", StringComparison.OrdinalIgnoreCase);
                    }
                    catch
                    {
                        isRaspi = false;
                    }
                }
                else
                {
                    isRaspi = false;
                }
                return isRaspi;
            }
        }
    }
}