using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using System;
using System.Diagnostics;
using System.Management;

namespace Cajetan.Infobar.Services
{

    public class SystemInfoService : ISystemInfoService
    {
        private readonly SysInfo _sys;
        //private NetworkMonitor _net;

        private double _processorUsage = 0;

        private double _memoryTotal = 0;
        private double _memoryUsed = 0;
        private double _memoryPercentage = 0;

        private readonly double _networkDown = 0;
        private readonly double _networkUp = 0;

        private TimeSpan _uptime;

        private bool _batteryShowTime = true;
        private int _batteryPercentage = 0;
        private TimeSpan _batteryTimeRemaining;
        private EBatteryChargeState _batteryChargeState;

        public SystemInfoService()
        {
            _sys = new SysInfo();

            //_net = new NetworkMonitor();
            //_net.StartMonitoring();
        }

        public void UpdateInfo()
        {
            _processorUsage = _sys.GetProcessorPercentage();

            _memoryTotal = _sys.GetMemoryTotal();
            _memoryUsed = _sys.GetMemoryUsed();
            _memoryPercentage = (_memoryUsed / _memoryTotal) * 100;

            //_networkDown = _net.GetDownloadRate();
            //_networkUp = _net.GetUploadRate();


            _uptime = _sys.GetSystemUptime();

            _batteryPercentage = _sys.GetBatteryPercent();
            _batteryTimeRemaining = _sys.GetBatteryTimeRemaining();
            _batteryChargeState = _sys.GetBatteryChargeState();
        }

        private string ConvertNetworkValue(double bytes)
        {
            //var d = bytes / 1024;
            double d = bytes;
            string u = "K";
            if (d >= 1024)
            {
                // Convert to MB
                d /= 1024;
                u = "M";
            }
            return d.ToString("0.0") + u;
        }

        /// <summary>
        /// Gets the current Processor Usage, as percentage.
        /// </summary>
        public int ProcessorUsage { get { return Convert.ToInt32(_processorUsage); } }
        public string ProcessorUsageString { get { return ProcessorUsage + " %"; } }


        /// <summary>
        /// Gets the total amount of system memory, in MB.
        /// </summary>
        public int MemoryTotal { get { return Convert.ToInt32(_memoryTotal); } }

        /// <summary>
        /// Gets the current amount of used memory, in MB.
        /// </summary>
        public int MemoryUsed { get { return Convert.ToInt32(_memoryUsed); } }

        /// <summary>
        /// Gets the current avmount of used memory, as percentage.
        /// </summary>
        public int MemoryUsedPercentage { get { return Convert.ToInt32(_memoryPercentage); } }
        public string MemoryUsageString { get { return (MemoryUsed + "MB") + " / " + (MemoryTotal + "MB"); } }


        /// <summary>
        /// Gets the current network downstream, in KB.
        /// </summary>
        public double NetworkDownload { get { return _networkDown; } }
        public string NetworkDownloadString { get { return ConvertNetworkValue(_networkDown); } }

        /// <summary>
        /// Gets the current network upstream, in KB.
        /// </summary>
        public double NetworkUpload { get { return _networkUp; } }
        public string NetworkUploadString { get { return ConvertNetworkValue(_networkUp); } }


        /// <summary>
        /// Gets the current system uptime, as TimeSpan.
        /// </summary>
        public TimeSpan Uptime { get { return _uptime; } }
        public string UptimeString
        {
            get
            {
                string d = _uptime.Days + "d";
                string h = _uptime.Hours.ToString("00");
                string m = _uptime.Minutes.ToString("00");
                string s = _uptime.Seconds.ToString("00");
                return d + " " + h + ":" + m + ":" + s;
                //return string.Format("{}", _uptime);
            }
        }


        public bool BatteryShowTime { get { return _batteryShowTime; } set { _batteryShowTime = value; } }
        public int BatteryPercentage { get { return _batteryPercentage; } }
        public TimeSpan BatteryTimeRemaining { get { return _batteryTimeRemaining; } }
        public EBatteryChargeState BatteryChargingState { get { return _batteryChargeState; } }

        public string BatteryStatusString
        {
            get
            {
                //var f = "{0} ({1})";
                int p = _sys.GetBatteryPercent();
                EBatteryChargeState s = _sys.GetBatteryChargeState();
                TimeSpan t = _sys.GetBatteryTimeRemaining();
                bool ac = _sys.GetBatteryIsOnAC();

                string str = "Unknown";
                string extra = null;
                if (s == EBatteryChargeState.NoBattery)
                {
                    str = "No Battery";
                }
                else
                {
                    if (ac)
                        extra = p >= 100 ? null : "Charging";
                    else
                        if (_batteryShowTime)
                        extra = (t.TotalSeconds >= 0) ? t.Hours.ToString("00h") + " " + t.Minutes.ToString("00m") : "Calculating";
                    str = p + "%";
                }

                return String.Format((extra == null ? "{0}" : "{0} ({1})"), str, extra);
            }
        }

        /*
         * Some methods and functionality originally created by "Zuoliu Ding, 05/20/2006" in the class SystemData.
         * All credit for that work goes to him.
         */
        private class SysInfo
        {
            private readonly PerformanceCounter _uptime;
            private readonly PerformanceCounter _cpu;
            private readonly PerformanceCounter _mem;
            //private PowerStatus _ps;
            private double _totalMemory = -1;

            public SysInfo()
            {
                _cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _mem = new PerformanceCounter("Memory", "Available Bytes");
                _uptime = new PerformanceCounter("System", "System Up Time");
                //_ps = SystemInformation.PowerStatus;
            }

            public double GetProcessorPercentage()
            {
                float val = _cpu.NextValue();
                return val;
            }

            public double GetMemoryTotal()
            {
                if (_totalMemory < 0)
                {
                    double total = -1;

                    ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);

                    foreach (ManagementObject item in searcher.Get())
                    {
                        total = double.Parse(item["TotalPhysicalMemory"].ToString());
                    }

                    total = ConvertBytes(total, Unit.MB);
                    _totalMemory = total;
                    return _totalMemory;
                }
                else
                {
                    return _totalMemory;
                }
            }

            public double GetMemoryUsed()
            {
                double total = GetMemoryTotal();
                double available = ConvertBytes(_mem.NextValue(), Unit.MB);
                double used = total - available;
                return used;
            }

            public TimeSpan GetSystemUptime()
            {
                _uptime.NextValue();
                float time = _uptime.NextValue();
                return TimeSpan.FromSeconds(time);
            }

            public int GetBatteryPercent()
            {
                return 100;
                //int p = Convert.ToInt32(_ps.BatteryLifePercent * 100, CultureInfo.InvariantCulture);
                //return p;
            }

            public TimeSpan GetBatteryTimeRemaining()
            {
                return TimeSpan.Zero;
                //TimeSpan ts = new TimeSpan(0, 0, _ps.BatteryLifeRemaining);
                //return ts;
            }

            public EBatteryChargeState GetBatteryChargeState()
            {
                return EBatteryChargeState.NoBattery;
                //if (_ps.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
                //    return BatteryChargeState.NoBattery;

                //else if (_ps.BatteryChargeStatus < BatteryChargeStatus.Charging)
                //    return BatteryChargeState.Discharging;

                //else if (_ps.BatteryChargeStatus >= BatteryChargeStatus.Charging && _ps.BatteryChargeStatus < BatteryChargeStatus.NoSystemBattery)
                //    return BatteryChargeState.Charging;

                //else
                //    return BatteryChargeState.Unknown;
            }

            public bool GetBatteryIsOnAC()
            {
                return true;
                //if (_ps.PowerLineStatus == PowerLineStatus.Online)
                //    return true;
                //return false;

            }

            public enum Unit : int { B = 0, KB, MB, GB, TB }

            public double ConvertBytes(double bytes, Unit unit)
            {
                double d = bytes;
                for (int i = 0; i < (int)unit; i++)
                {
                    d /= 1024;
                }
                return d;
            }

            private double GetValue(PerformanceCounter pc, string category, string counter, string instance)
            {
                return pc.NextValue();
            }
        }
    }
}
