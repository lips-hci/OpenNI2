using System;
using System.Threading;
using OpenNIWrapper;

namespace NiHello
{
    #region

    #endregion

    public static class Program
    {
        #region Public Methods and Operators

        #endregion

        #region Static Fields

        #endregion

        #region Methods

        private static void Main()
        {
            var status = OpenNI.Initialize();
            if (status != OpenNI.Status.Ok)
            {
                Console.WriteLine("OpenNI2 Initialize Failed.\n");
                Environment.Exit(0);
            }

            Console.WriteLine("OpenNI2 " + OpenNI.Version.ToString() + " is ready.\n");
            Console.Out.Flush();

            OpenNI.OnDeviceConnected += OpenNiOnDeviceConnected;
            OpenNI.OnDeviceDisconnected += OpenNiOnDeviceDisconnected;

            DeviceInfo[] devices = OpenNI.EnumerateDevices();

            if (devices.Length == 0)
            {
                Console.WriteLine("Cannot find avaliable OpenNI2 device.\n");
            }
            else
            {
                DeviceInfo info = devices[0];
                Console.WriteLine("\nDevice found.");
                Console.WriteLine("Name: " + info.Vendor + info.Name);
                Console.WriteLine("URI: " + info.Uri);
                Console.WriteLine("USB Product ID: " + info.UsbProductId);
                Console.WriteLine("USB Vendor ID: " + info.UsbVendorId);
                Console.WriteLine("Vendor: " + info.Vendor);

                // Open the first device and print information
                Device device = devices[0].OpenDevice();

                if (device != null && device.IsValid)
                {
                    Console.WriteLine("\nOpen device successful: " + info.Vendor + info.Name);
                    device.Close();
                }
                else
                {
                    Console.WriteLine("\nCannot open device: " + info.Name);
                }
            }

            Console.WriteLine("\nShutdown OpenNI2 framework...");
            OpenNI.Shutdown();

            Console.WriteLine("\nPress key Enter to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }
        private static void OpenNiOnDeviceConnected(DeviceInfo device)
        {
            Console.WriteLine(device.Name + " Connected ...\n");
        }

        private static void OpenNiOnDeviceDisconnected(DeviceInfo device)
        {
            Console.WriteLine(device.Name + " Disconnected ...\n");
        }

        #endregion
    }
}