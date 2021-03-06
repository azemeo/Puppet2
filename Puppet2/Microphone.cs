﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace Puppet2
{
    public partial class Microphone
    {
        private int deviceCount = WaveIn.DeviceCount;
        public int DeviceCount
        {
            get { return deviceCount; }
        }

        private int deviceNumber = Properties.Settings.Default.MicrophoneDeviceNumber;
        public int DeviceNumber
        {
            get { return deviceNumber; }
            set
            {
                deviceNumber = value;
                Properties.Settings.Default.MicrophoneDeviceNumber = deviceNumber;
            }
        }

        private WaveInEvent waveInEvent;
        public WaveInEvent WaveInEvent
        {
            get { return waveInEvent; }
        }

        public string deviceProductName(int deviceNumber)
        {
            WaveInCapabilities capabilities = WaveIn.GetCapabilities(deviceNumber);
            return capabilities.ProductName;
        }

        public Microphone()
        {
            if (deviceNumber + 1 > deviceCount)
            {
                deviceNumber = 0;
                Properties.Settings.Default.MicrophoneDeviceNumber = deviceNumber;
                Properties.Settings.Default.Save();
            }
            if (deviceCount > 0)
            {
                waveInEvent = new WaveInEvent();
                waveInEvent.DataAvailable += waveInEvent_DataAvailable;
                waveInEvent.DeviceNumber = deviceNumber;
                Setup();
                Start();
            }
        }

        public void Setup()
        {
            WaveInCapabilities capabilities = WaveIn.GetCapabilities(deviceNumber);
            foreach (int waveFormat in Enum.GetValues(typeof(SupportedWaveFormat)))
            {                
                if (capabilities.SupportsWaveFormat((SupportedWaveFormat)waveFormat))
                {
                    SetWaveFormat((SupportedWaveFormat)waveFormat);
                    break;
                }
            }
        }

        public void Start()
        {
            waveInEvent.StartRecording();
        }

        public void Stop()
        {
            if (waveInEvent != null)
            {
                waveInEvent.StopRecording();
            }
        }

        public void Dispose()
        {
            if (waveInEvent != null)
            {
                waveInEvent.Dispose();
            }
        }

    }
}
