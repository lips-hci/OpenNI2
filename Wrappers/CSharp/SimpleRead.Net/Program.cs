using System;
using System.Threading;
using OpenNIWrapper;

namespace SimpleRead
{
    #region

    #endregion

    public static class Program
    {
        #region Public Methods and Operators

        public static bool HandleError(OpenNI.Status status)
        {
            if (status == OpenNI.Status.Ok)
            {
                return true;
            }

            Console.WriteLine("Error: " + status + " - " + OpenNI.LastError);
            Console.ReadLine();

            return false;
        }

        #endregion

        #region Static Fields

        #endregion

        #region Methods

        public static void Main(string[] args)
        {
            Device device = null;

            try
            {
                OpenNI.Initialize();
                Console.WriteLine("OpenNI2 " + OpenNI.Version.ToString() + " is ready.\n");

                OpenNI.OnDeviceConnected += OpenNiOnDeviceConnected;
                OpenNI.OnDeviceDisconnected += OpenNiOnDeviceDisconnected;

                device = Device.Open(Device.AnyDevice);

                VideoStream depthStream = device.CreateVideoStream(Device.SensorType.Depth);

                depthStream.Start();

                VideoMode mode = depthStream.VideoMode;
                Console.WriteLine("Depth is now streaming: " + mode.ToString());

                while (!Console.KeyAvailable)
                {
                    if (OpenNI.WaitForStream(depthStream) == OpenNI.Status.Ok)
                    {
                        VideoFrameRef frame = depthStream.ReadFrame();
                        if (frame.IsValid)
                        {
                            unsafe
                            {
                                ushort* pDepth = (ushort*)frame.Data.ToPointer();
                                int middleIndex = (frame.FrameSize.Height + 1) * (frame.FrameSize.Width / 2);
                                Console.WriteLine("Distance is {0} mm", pDepth[middleIndex]);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\nOops! something is wrong (" + e.Message + ")");
                Console.WriteLine("Error: " + OpenNI.LastError);
            }

            if (device != null)
            {
                device.Close();
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