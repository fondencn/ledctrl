using Iot.Device.Apa102;
using LedCtrl;
using System;
using System.Device.Spi;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Core
{
    public class IoTcoreLedService
    {
        /// <summary>
        /// LED Power on Respeaker Core V2
        /// </summary>
        private const int POWER_PIN = 66; // see MRAA-12 mapping
        /// <summary>
        /// LED Power on Respeaker 4mics HAT on a Raspberry Pi 3b+
        /// </summary>
        private const int POWER_PIN_RASPI = 5; // see MRAA-12 mapping

        public static void Voltage_On()
        {
            int powerPin = IsOnRaspi ? POWER_PIN_RASPI : POWER_PIN;
            System.Device.Gpio.PinValue pinVal = IsOnRaspi ? System.Device.Gpio.PinValue.High : System.Device.Gpio.PinValue.Low;

            System.Device.Gpio.GpioController gpio = new System.Device.Gpio.GpioController();
            gpio.OpenPin(powerPin, System.Device.Gpio.PinMode.Output);
            Thread.Sleep(250);
            gpio.Write(powerPin, pinVal);

            Logger.Instance.LogDebug("Voltage_On(): GPIO " + powerPin + " to " + pinVal);
        }

        public static void Voltage_Off()
        {
            //SolidColor(Color.Black);

            int powerPin = IsOnRaspi ? POWER_PIN_RASPI : POWER_PIN;
            System.Device.Gpio.PinValue pinVal = IsOnRaspi ? System.Device.Gpio.PinValue.Low : System.Device.Gpio.PinValue.High;

            System.Device.Gpio.GpioController gpio = new System.Device.Gpio.GpioController();
            gpio.OpenPin(powerPin, System.Device.Gpio.PinMode.Output);
            Thread.Sleep(250);
            gpio.Write(powerPin, pinVal);

            Logger.Instance.LogDebug("Voltage_Off(): GPIO " + powerPin + " to " + pinVal);
        }

        private static bool IsOnRaspi
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

        public static void SolidColor(Color color)
        {
            var spi = SpiDevice.Create(new SpiConnectionSettings(0));
            using (Apa102 apa102 = new Apa102(spi, 12))
            {
                for (var i = 0; i < apa102.Pixels.Length; i++)
                {
                    apa102.Pixels[i] = color;
                    apa102.Flush();
                    Thread.Sleep(100);
                }
            }
        }

        public static void Spin(Color color, int count = 1)
        {
            var spi = SpiDevice.Create(new SpiConnectionSettings(0));
            using (Apa102 apa102 = new Apa102(spi, 12))
            {
                for (int c = 0; c < count; c++)
                {

                    for (var i = 0; i < apa102.Pixels.Length; i++)
                    {
                        for (var j = 0; j < apa102.Pixels.Length; j++)
                        {
                            apa102.Pixels[j] = Color.Black;
                        }

                        if (i > 0)
                        {
                            apa102.Pixels[i - 1] = Color.FromArgb(200, color.R, color.G, color.B);
                        }
                        if (i > 1)
                        {
                            apa102.Pixels[i - 2] = Color.FromArgb(100, color.R, color.G, color.B);
                        }
                        apa102.Pixels[i] = color;
                        apa102.Flush();
                        Thread.Sleep(100);
                    }
                }
            }
        }


        public static void AnimateRandom()
        {
            Random random = new Random();

            using (Apa102 apa102 = new Apa102(SpiDevice.Create(new SpiConnectionSettings(0)), 12))
            {

                for (int count = 0; count < 3; count++)
                {
                    for (var i = 0; i < apa102.Pixels.Length; i++)
                    {
                        apa102.Pixels[i] = Color.FromArgb(255, random.Next(256), random.Next(256), random.Next(256));
                    }

                    apa102.Flush();
                    Thread.Sleep(500);
                }
            }
        }
    }
}
