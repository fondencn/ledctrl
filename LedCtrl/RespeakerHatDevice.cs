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
    public class RespeakerHatDevice : LedDevice, ILEDService, IDisposable
    {
        /// <summary>
        /// LED Power on Respeaker Core V2
        /// </summary>
        private const int POWER_PIN = 66; // see MRAA-12 mapping
        /// <summary>
        /// LED Power on Respeaker 4mics HAT on a Raspberry Pi 3b+
        /// </summary>
        private const int POWER_PIN_RASPI = 5; // see MRAA-12 mapping

        private readonly int _LedCount;

        public RespeakerHatDevice(int ledCount = 12)
            : base()
        {
            if (!IsOnRaspi)
            {
                throw new NotSupportedException("This device is only supported on Raspberry Pi with Respeaker Hat.");
            }
            _LedCount = ledCount;
            Voltage_On();
        }

        public void Voltage_On()
        {
            int powerPin = IsOnRaspi ? POWER_PIN_RASPI : POWER_PIN;
            System.Device.Gpio.PinValue pinVal = IsOnRaspi ? System.Device.Gpio.PinValue.High : System.Device.Gpio.PinValue.Low;

            System.Device.Gpio.GpioController gpio = new System.Device.Gpio.GpioController();
            gpio.OpenPin(powerPin, System.Device.Gpio.PinMode.Output);
            Thread.Sleep(250);
            gpio.Write(powerPin, pinVal);

            Logger.Instance.LogDebug("Voltage_On(): GPIO " + powerPin + " to " + pinVal);
        }

        public void Voltage_Off()
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


        public void SetSolidColor(Color color)
        {
            var spi = SpiDevice.Create(new SpiConnectionSettings(0));
            using (Apa102 apa102 = new Apa102(spi, _LedCount))
            {
                for (var i = 0; i < apa102.Pixels.Length; i++)
                {
                    apa102.Pixels[i] = color;
                    apa102.Flush();
                    Thread.Sleep(100);
                }
            }
        }

        public void Spin(Color color, int count = 1)
        {
            var spi = SpiDevice.Create(new SpiConnectionSettings(0));
            using (Apa102 apa102 = new Apa102(spi, _LedCount))
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

        public void SetLeds(Color[] colors)
        {
            if (colors == null || colors.Length != _LedCount)
            {
                throw new ArgumentException($"Colors array must have exactly {_LedCount} elements.");
            }

            var spi = SpiDevice.Create(new SpiConnectionSettings(0));
            using (Apa102 apa102 = new Apa102(spi, _LedCount))
            {
                for (int i = 0; i < _LedCount; i++)
                {
                    apa102.Pixels[i] = colors[i];
                }
                apa102.Flush();
            }
        }
        
        public void Trace()
        {
            Spin(Color.FromArgb(100, 0, 0, 255), 1); // Blue trace
        }
        public void Listen()
        {
            Spin(Color.FromArgb(100, 0, 255, 0)); // Green listen
        }
        public void Wait()
        {
            Spin(Color.FromArgb(100, 255, 255, 0)); // Yellow wait
        }
        public void Speak()
        {
            Spin(Color.FromArgb(100, 255, 0, 0)); // Red speak
        }

        public void Dispose()
        {
            try
            {
                Voltage_Off();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogException("Error while turning off voltage", ex);
            }

            Logger.Instance.LogDebug("RespeakerHatDevice disposed.");
        }
    }
}
