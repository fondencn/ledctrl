using System;
using System.Drawing;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace LedCtrl
{
    public class RespeakerUsbDevice : LedDevice, ILEDService, IDisposable
    {
        private UsbDevice _device;
        //private UsbEndpointWriter _writer;
        private const int VendorId = 0x2886;
        private const int ProductId = 0x0018;
        private const int InterfaceNumber = 3; // Interface number for ReSpeaker USB device
        private const int LedCount = 12; // Number of LEDs on the ReSpeaker USB device


        public RespeakerUsbDevice()
        {
            UsbDeviceFinder finder = new UsbDeviceFinder(VendorId, ProductId);
            _device = UsbDevice.OpenUsbDevice(finder);

            if (_device == null)
            {
                Console.WriteLine("ReSpeaker USB device not found.");
                Console.WriteLine("Listing all connected USB devices:");
                foreach (UsbRegistry usbRegistry in UsbDevice.AllDevices)
                {
                    Console.WriteLine($"VID: 0x{usbRegistry.Vid:X4}, PID: 0x{usbRegistry.Pid:X4}, Name: {usbRegistry.Name}");
                }
            }

            if (_device == null)
            {
                throw new Exception("Could not open ReSpeaker device.");
            }
            IUsbDevice wholeUsbDevice = _device as IUsbDevice;
            if (wholeUsbDevice != null)
            {
                wholeUsbDevice.SetConfiguration(1);
                Console.WriteLine("DriverMode=" + wholeUsbDevice.DriverMode);
                try
                {
                    if (wholeUsbDevice.DriverMode == UsbDevice.DriverModeType.MonoLibUsb)
                    {
                        wholeUsbDevice.DetachKernelDriver(InterfaceNumber);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DetachKernelDriver failed: " + ex.Message);
                }
                //wholeUsbDevice.ClaimInterface(InterfaceNumber);
            }

            // Find the first OUT endpoint (usually 0x01)
            //_writer = _device.OpenEndpointWriter(WriteEndpointID.Ep01);
            Console.WriteLine("Opened ReSpeaker USB device using LibUsbDotNet.");
        }

        /* command 	data 	note
        0 	[0] 	trace mode, LEDs changing depends on VAD* and DOA*
        1 	[red, green, blue, 0] 	mono mode, set all RGB LED to a single color
        2 	[0] 	listen mode, similar with trace mode, but not turn LEDs off
        3 	[0] 	wait mode
        4 	[0] 	speak mode
        5 	[0] 	spin mode
        6 	[r, g, b, 0] * 12 	custom mode, set each LED to its own color
        0x20 	[brightness] 	set brightness, range: 0 ~ 0x1F
        0x21 	[r1, g1, b1, 0, r2, g2, b2, 0] 	set color palette
        0x22 	[vad_led] 	set center LED: 0 - off, 1 - on, else - depends on VAD
        0x23 	[volume] 	show volume, range: 0 ~ 12
        */

        // Set all LEDs to a color (RGB)
        public void SetSolidColor(Color c)
        {

            short command = 0x0001; // Command for setting all LEDs
            byte[] data = new byte[4];
            data[0] = c.R; // Red
            data[1] = c.G; // Green
            data[2] = c.B; // Blue
            data[3] = 0; // Reserved byte, set to 0

            SetcontrolCommand(command, data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetLeds(Color[] colors)
        {
            if(colors == null || colors.Length != LedCount)
            {
                throw new ArgumentException($"Colors array must have exactly {LedCount} elements.");
            }
            // Prepare data for setting all LEDs to their own colors
            // Each LED color is represented by 4 bytes: [r, g, b, 0]
            byte[] data = new byte[LedCount * 4]; // 4 bytes per LED, total for all LEDs
            for (int i = 0; i < LedCount; i++)
            {
                data[i * 4] = colors[i].R;     // Red
                data[i * 4 + 1] = colors[i].G; // Green
                data[i * 4 + 2] = colors[i].B; // Blue
                data[i * 4 + 3] = 0;           // Reserved byte, set to 0
            }
            short command = 0x0006; // Command for setting all LEDs
            

            SetcontrolCommand(command, data);
        }

        public void Spin(Color color, int count = 1)
        {
            short command = 0x0005; // Command for spin mode
            byte[] data = new byte[] { 0 };

            SetcontrolCommand(command, data);
        }

        public void Trace()
        {
            short command = 0x0000; // Command for trace mode
            byte[] data = new byte[] { 0 };

            SetcontrolCommand(command, data);
        }

        public void Listen()
        {
            short command = 0x0002; // Command for Listen mode
            byte[] data = new byte[] { 0 };

            SetcontrolCommand(command, data);
        }
        
        public void Wait()
        {
            short command = 0x0003; // Command for Wait mode
            byte[] data = new byte[] { 0 };

            SetcontrolCommand(command, data);
        }
        
        public void Speak()
        {
            short command = 0x0004; // Command for  speak mode
            byte[] data = new byte[] { 0 };

            SetcontrolCommand(command, data);
        }



        private void SetcontrolCommand(short command, byte[] data)
        {
            UsbSetupPacket packet = new UsbSetupPacket(
                            (byte)(UsbCtrlFlags.Direction_Out | UsbCtrlFlags.RequestType_Vendor | UsbCtrlFlags.Recipient_Device), // bmRequestType
                            0,         // bRequest
                            command, // wValue
                            0x1C,      // wIndex
                            (short)data.Length // wLength
                        );

            int transferred;
            bool success = _device.ControlTransfer(ref packet, data, data.Length, out transferred);
            if (!success)
            {
                throw new Exception("USB control transfer failed.");
            }
        }
        

        public void Dispose()
        {
            if (_device != null)
            {
                if (_device.IsOpen)
                {
                    this.Trace(); // Reset to trace mode before closing
                    IUsbDevice wholeUsbDevice = _device as IUsbDevice;
                    if (wholeUsbDevice != null)
                    {
                        wholeUsbDevice.ReleaseInterface(InterfaceNumber);
                    }
                    _device.Close();
                }
                _device = null;
            }
            UsbDevice.Exit();
        }

    }
}