/*****************************************************************************
*                                                                            *
*  OpenNI 1.x Alpha                                                          *
*  Copyright (C) 2012 PrimeSense Ltd.                                        *
*                                                                            *
*  This file is part of OpenNI.                                              *
*                                                                            *
*  Licensed under the Apache License, Version 2.0 (the "License");           *
*  you may not use this file except in compliance with the License.          *
*  You may obtain a copy of the License at                                   *
*                                                                            *
*      http://www.apache.org/licenses/LICENSE-2.0                            *
*                                                                            *
*  Unless required by applicable law or agreed to in writing, software       *
*  distributed under the License is distributed on an "AS IS" BASIS,         *
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  *
*  See the License for the specific language governing permissions and       *
*  limitations under the License.                                            *
*                                                                            *
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using OpenNIWrapper;

namespace SimpleViewer.net
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            OpenNI.Initialize();
            Console.WriteLine("OpenNI2 " + OpenNI.Version.ToString() + " is ready.\n");

            Device device = Device.Open(Device.AnyDevice);

            m_stream = device.CreateVideoStream(Device.SensorType.Depth);

            m_stream.Start();

            VideoMode mode = m_stream.VideoMode;
            Console.WriteLine("Depth is now streaming: " + mode.ToString());

            this.histogram = new int[m_stream.MaxPixelValue + 1];

            this.bitmap = new Bitmap(mode.Resolution.Width, mode.Resolution.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            this.shouldRun = true;
            this.readerThread = new Thread(ReaderThread);
            this.readerThread.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            lock (this)
            {
                e.Graphics.DrawImage(this.bitmap,
                    this.panelView.Location.X,
                    this.panelView.Location.Y,
                    this.panelView.Size.Width,
                    this.panelView.Size.Height);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Don't allow the background to paint
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.shouldRun = false;
            this.readerThread.Join();
            base.OnClosing(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                Close();
            }
            base.OnKeyPress(e);
        }

        private unsafe void CalcHist(VideoFrameRef frame)
        {
            // reset
            for (int i = 0; i < this.histogram.Length; ++i)
                this.histogram[i] = 0;

            ushort* pDepth = (ushort*)frame.Data.ToPointer();

            int points = 0;
            for (int y = 0; y < frame.FrameSize.Height; ++y)
            {
                for (int x = 0; x < frame.FrameSize.Width; ++x, ++pDepth)
                {
                    ushort depthVal = *pDepth;
                    if (depthVal != 0)
                    {
                        this.histogram[depthVal]++;
                        points++;
                    }
                }
            }

            for (int i = 1; i < this.histogram.Length; i++)
            {
                this.histogram[i] += this.histogram[i - 1];
            }

            if (points > 0)
            {
                for (int i = 1; i < this.histogram.Length; i++)
                {
                    this.histogram[i] = (int)(256 * (1.0f - (this.histogram[i] / (float)points)));
                }
            }
        }

        private unsafe void ReaderThread()
        {
            while (this.shouldRun)
            {
                if (OpenNI.WaitForStream(m_stream) != OpenNI.Status.Ok)
                {
                    continue;
                }

                VideoFrameRef frame = m_stream.ReadFrame();
                if (frame.IsValid)
                {
                    CalcHist(frame);

                    lock (this)
                    {
                        Rectangle rect = new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height);
                        BitmapData data = this.bitmap.LockBits(rect, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                        ushort* pDepth = (ushort*)frame.Data.ToPointer();

                        // set pixels
                        for (int y = 0; y < frame.FrameSize.Height; ++y)
                        {
                            byte* pDest = (byte*)data.Scan0.ToPointer() + y * data.Stride;
                            for (int x = 0; x < frame.FrameSize.Width; ++x, ++pDepth, pDest += 3)
                            {
                                byte pixel = (byte)this.histogram[*pDepth];
                                pDest[0] = 0;
                                pDest[1] = pixel;
                                pDest[2] = pixel;
                            }
                        }

                        this.bitmap.UnlockBits(data);
                    }
                }

                this.Invalidate();
            }
        }

        private VideoStream m_stream;
        private Thread readerThread;
        private bool shouldRun;
        private Bitmap bitmap;
        private int[] histogram;
    }
}