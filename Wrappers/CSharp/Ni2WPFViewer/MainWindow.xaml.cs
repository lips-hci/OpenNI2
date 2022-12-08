using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using OpenNIWrapper;

namespace Ni2WPFViewer
{
    public partial class MainWindow : Window
    {
        private Bitmap mBitmap = null;
        private BitmapImage mImage = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenNI2_Initialize();
            this.mBitmap = new Bitmap(1, 1);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            OpenNI2_Terminate();
        }

        private void BtDeviceOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenNI2_OpenDevice();
        }

        private void BtDepthStart_Click(object sender, RoutedEventArgs e)
        {
            OpenNI2_StartDepth();
        }

        private void BtDepthStop_Click(object sender, RoutedEventArgs e)
        {
            OpenNI2_StopDepth();
        }

        private bool bIsDeviceOpened = false;
        private string strDevice = "", strVideoMode = "";
        private Device mDevice = null;
        private VideoStream mStream = null;

        private void OpenNI2_Initialize()
        {
            OpenNI.Status status = OpenNI.Initialize();
            if (status != OpenNI.Status.Ok)
            {
                string errMsg = string.Format(@"Failed to initialize OpenNI2: {0} - {1}", status, OpenNI.LastError);
                MessageBox.Show(errMsg, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                this.txtDevice.Text = "OpenNI2 Module is ready.\n";
            }));
        }

        private void OpenNI2_Terminate()
        {
            if (this.mStream != null && this.mStream.IsValid)
            {
                this.mStream.Stop();
                this.mStream = null;
            }

            if (this.mDevice != null && this.mDevice.IsValid)
            {
                this.mDevice.Close();
                this.bIsDeviceOpened = false;

                this.mDevice.Dispose();
                this.mDevice = null;
            }

            OpenNI.Shutdown();
        }

        private void OpenNI2_OpenDevice()
        {
            DeviceInfo[] devices = OpenNI.EnumerateDevices();
            if (devices.Length > 0)
            {
                this.mDevice = devices[0].OpenDevice();
                if (this.mDevice != null && this.mDevice.IsValid)
                {
                    this.strDevice = string.Format("Device {0} is now connected and opened.\n", devices[0].Name);
                    this.bIsDeviceOpened = true;
                }
                else
                {
                    this.strDevice = "Sorry. Available device NOT found.\n";
                    this.bIsDeviceOpened = false;
                }
            }
            else
            {
                this.strDevice = "Sorry. Available device NOT found.\n";
                this.bIsDeviceOpened = false;
            }

            this.Dispatcher.Invoke((Action)(() =>
            {
                this.txtDevice.Text = this.strDevice;
            }));
        }

        private void OpenNI2_StartDepth()
        {
            if (!this.bIsDeviceOpened)
            {
                MessageBox.Show("Opps! Please open the device first.", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (this.mDevice != null && this.mDevice.IsValid)
            {
                if (this.mDevice.HasSensor(Device.SensorType.Depth))
                {
                    if (this.mStream == null || !this.mStream.IsValid)
                    {
                        this.mStream = this.mDevice.CreateVideoStream(Device.SensorType.Depth);
                        if (this.mStream == null || !this.mStream.IsValid)
                        {
                            MessageBox.Show("Failed to create depth stream.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    OpenNI.Status status = this.mStream.Start();
                    if (status != OpenNI.Status.Ok)
                    {
                        string errMsg = string.Format(@"Failed to start depth stream: {0} - {1}", status, OpenNI.LastError);
                        MessageBox.Show(errMsg, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        this.mStream.OnNewFrame += this.OpenNI2_OnNewFrame;
                    }

                    VideoMode mode = this.mStream.VideoMode;
                    this.strVideoMode = string.Format("Depth is now streaming: {0} x {1} @ {2}", mode.Resolution.Width, mode.Resolution.Height, mode.Fps);
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.txtDevice.Text = this.strDevice + this.strVideoMode;
                    }));
                }
                else
                {
                    MessageBox.Show("Sorry, the device does not support depth stream.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Sorry, the device is not valid.\nPlease try opening device again.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void OpenNI2_StopDepth()
        {
            if (this.mStream != null && this.mStream.IsValid)
            {
                this.mStream.Stop();
                this.mStream.OnNewFrame -= this.OpenNI2_OnNewFrame;
                this.strVideoMode = "Depth stream is stopped.";
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtDevice.Text = this.strDevice + strVideoMode;
                }));
            }
        }

        private void OpenNI2_OnNewFrame(VideoStream stream)
        {
            if (stream != null && stream.IsValid && stream.IsFrameAvailable())
            {
                using (VideoFrameRef frame = stream.ReadFrame())
                {
                    if (frame.IsValid)
                    {
                        VideoFrameRef.CopyBitmapOptions options = VideoFrameRef.CopyBitmapOptions.Force24BitRgb
                                                                | VideoFrameRef.CopyBitmapOptions.DepthFillShadow;

                        try
                        {
                            frame.UpdateBitmap(this.mBitmap, options);
                        }
                        catch (Exception)
                        {
                            // Happens when Bitmap object is not compatible with returned Frame
                            this.mBitmap = frame.ToBitmap(options);
                        }

                        using (MemoryStream memory = new MemoryStream())
                        {
                            this.mBitmap.Save(memory, ImageFormat.Bmp);
                            memory.Position = 0;
                            this.mImage = new BitmapImage();
                            this.mImage.BeginInit();
                            this.mImage.StreamSource = memory;
                            this.mImage.CacheOption = BitmapCacheOption.OnLoad;
                            this.mImage.EndInit();
                            this.mImage.Freeze();
                        }
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            this.ImgDepth.Source = this.mImage;
                        }));
                    }
                }
            }
        }
    }
}
