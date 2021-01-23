using Cajetan.Infobar.Domain.Models;
using Cajetan.Infobar.Domain.Services;
using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cajetan.Infobar.Services
{
    public class SystemMonitorService : ISystemMonitorService, IDisposable
    {

        private bool _isDisposed;

        public SystemMonitorService()
        {
            Info = new SysInfo();
            Battery = new BatteryInfo();
            Processor = new ProcessorInfo();
            Memory = new MemoryInfo();
            Network = new NetworkInfo();
        }

        public ISysInfo Info { get; }
        public IBatteryInfo Battery { get; }
        public IProcessorInfo Processor { get; }
        public IMemoryInfo Memory { get; }
        public INetworkInfo Network { get; }

        public void Update()
        {
            if (_isDisposed) return;

            Info.Update();
            Battery.Update();
            Processor.Update();
            Memory.Update();
            Network.Update();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Info?.Dispose();
                    Battery?.Dispose();
                    Processor?.Dispose();
                    Memory?.Dispose();
                    Network?.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal static double ConvertToUnit(double value, EUnit sourceUnit, EUnit targetUnit)
        {
            int numberOfStepsWithDirection = (int)sourceUnit - (int)targetUnit;
            int numberOfSteps = Math.Abs(numberOfStepsWithDirection);

            if (numberOfStepsWithDirection < 0)
                return ConvertToUnitByDivision(value, numberOfSteps);
            else if (numberOfStepsWithDirection > 0)
                return ConvertToUnitByMultiplication(value, numberOfSteps);
            else
                return value;

        }
        private static double ConvertToUnitByDivision(double value, int numberOfSteps)
        {
            for (int i = 0; i < numberOfSteps; i++)
                value /= 1024;

            return value;
        }
        private static double ConvertToUnitByMultiplication(double value, int numberOfSteps)
        {
            for (int i = 0; i < numberOfSteps; i++)
                value *= 1024;

            return value;
        }

        internal class SysInfo : ISysInfo
        {
            private PerformanceCounter _uptime;

            public SysInfo()
            {
                _uptime = new PerformanceCounter("System", "System Up Time");
                _uptime.NextValue();

                Update();
            }

            public TimeSpan Uptime { get; internal set; }


            public void Update()
            {
                float time = _uptime.NextValue();
                Uptime = TimeSpan.FromSeconds(time);
            }

            public void Dispose()
            {
                _uptime?.Dispose();
                _uptime = null;
            }
        }

        internal class BatteryInfo : IBatteryInfo
        {
            private HardwareInfo _hardwareInfo;

            public BatteryInfo()
            {
                _hardwareInfo = new HardwareInfo();

                Update();
            }

            public double Percentage { get; internal set; }
            public TimeSpan TimeToDepleted { get; internal set; }
            public TimeSpan TimeToFullCharge { get; internal set; }
            public EBatteryChargeState State { get; internal set; }

            public void Update()
            {
                _hardwareInfo.RefreshBatteryList();

                if (_hardwareInfo.BatteryList is null || !_hardwareInfo.BatteryList.Any())
                {
                    Percentage = 0;
                    TimeToDepleted = TimeSpan.Zero;
                    TimeToFullCharge = TimeSpan.Zero;
                    State = EBatteryChargeState.NoBattery;
                }
                else
                {
                    Battery battery = _hardwareInfo.BatteryList[0];

                    TimeToDepleted = TimeSpan.FromMinutes(battery.EstimatedRunTime);
                    TimeToFullCharge = TimeSpan.FromMinutes(battery.TimeToFullCharge);
                    Percentage = battery.EstimatedChargeRemaining;

                    State = battery.BatteryStatus switch
                    {
                        1 => EBatteryChargeState.Discharging,
                        2 => EBatteryChargeState.FullyCharged,
                        3 => EBatteryChargeState.FullyCharged,
                        4 => EBatteryChargeState.Discharging,
                        5 => EBatteryChargeState.Discharging,
                        6 => EBatteryChargeState.Charging,
                        7 => EBatteryChargeState.Charging,
                        8 => EBatteryChargeState.Charging,
                        9 => EBatteryChargeState.Charging,
                        10 => EBatteryChargeState.NoBattery,
                        11 => EBatteryChargeState.Discharging,
                        _ => EBatteryChargeState.Unknown
                    };
                }
            }

            public void Dispose()
            {
                _hardwareInfo = null;
            }
        }

        internal class ProcessorInfo : IProcessorInfo
        {
            private PerformanceCounter _processorPercentage;

            public ProcessorInfo()
            {
                _processorPercentage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _processorPercentage.NextValue();

                Update();
            }

            public double Percentage { get; internal set; }

            public void Update()
            {
                float valPercentage = _processorPercentage.NextValue();
                Percentage = Convert.ToDouble(valPercentage);
            }

            public void Dispose()
            {
                _processorPercentage?.Dispose();
                _processorPercentage = null;
            }
        }

        internal class MemoryInfo : IMemoryInfo
        {
            private HardwareInfo _hardwareInfo;
            private readonly EUnit _unit;

            public MemoryInfo()
            {
                _hardwareInfo = new HardwareInfo();
                _unit = EUnit.MB;

                Update();
            }

            public double Total { get; internal set; }
            public double Available { get; internal set; }
            public double Used { get; internal set; }
            public string Unit => $"{_unit}";
            public double Percentage { get; internal set; }

            public void Update()
            {
                _hardwareInfo.RefreshMemoryStatus();

                Total = ConvertToUnit(_hardwareInfo.MemoryStatus.TotalPhysical, EUnit.B, _unit);
                Available = ConvertToUnit(_hardwareInfo.MemoryStatus.AvailablePhysical, EUnit.B, _unit);

                Used = Total - Available;
                Percentage = Total > 0
                    ? (Used / Total) * 100
                    : 0;
            }

            public void Dispose()
            {
                _hardwareInfo = null;
            }
        }

        public class NetworkInfo : INetworkInfo
        {
            const string CATEGORY_NAME = "Network Interface";
            const string COUNTER_NAME_DOWN = "Bytes Received/sec";
            const string COUNTER_NAME_UP = "Bytes Sent/sec";
            const int MINUTES_BETWEEN_COUNTER_REFRESHES = 5;

            private static readonly EUnit[] _convertToUnits = new[] { EUnit.MB, EUnit.GB, EUnit.TB };

            private PerformanceCounter[] _downstreamPerformanceCounters;
            private PerformanceCounter[] _upstreamPerformanceCounters;
            private DateTime _lastCounterRefreshUtc;

            public NetworkInfo()
            {
                _downstreamPerformanceCounters = GetPerformanceCounters(COUNTER_NAME_DOWN);
                _upstreamPerformanceCounters = GetPerformanceCounters(COUNTER_NAME_UP);
                _lastCounterRefreshUtc = DateTime.UtcNow;

                Update();
            }

            public double DownloadRate { get; internal set; }
            public double UploadRate { get; internal set; }

            public void Update()
            {
                TimeSpan timeSinceLastCounterRefresh = DateTime.UtcNow - _lastCounterRefreshUtc;
                if (timeSinceLastCounterRefresh > TimeSpan.FromMinutes(MINUTES_BETWEEN_COUNTER_REFRESHES))
                {
                    _downstreamPerformanceCounters = GetPerformanceCountersAndDisposeExisting(COUNTER_NAME_DOWN, _downstreamPerformanceCounters);
                    _upstreamPerformanceCounters = GetPerformanceCountersAndDisposeExisting(COUNTER_NAME_UP, _upstreamPerformanceCounters);
                    _lastCounterRefreshUtc = DateTime.UtcNow;
                }

                DownloadRate = GetRate(_downstreamPerformanceCounters);
                UploadRate = GetRate(_upstreamPerformanceCounters);
            }

            public (double rate, string unit) GetDownloadRate(ENetworkDisplayFormat displayFormat)
                => GetRate(DownloadRate, displayFormat);
            public (double rate, string unit) GetUploadRate(ENetworkDisplayFormat displayFormat)
                => GetRate(UploadRate, displayFormat);

            public void Dispose()
            {
                DisposePerformanceCounters(_downstreamPerformanceCounters);
                _downstreamPerformanceCounters = null;

                DisposePerformanceCounters(_upstreamPerformanceCounters);
                _upstreamPerformanceCounters = null;
            }

            private PerformanceCounter[] GetPerformanceCounters(string counterName)
            {
                PerformanceCounterCategory networkInterfaces = new PerformanceCounterCategory(CATEGORY_NAME);
                string[] networkInterfaceNames = networkInterfaces.GetInstanceNames();
                PerformanceCounter[] counters = networkInterfaceNames.Select(n => new PerformanceCounter(CATEGORY_NAME, counterName, n)).ToArray();
                return counters;
            }

            private PerformanceCounter[] GetPerformanceCountersAndDisposeExisting(string counterName, PerformanceCounter[] existingPerfCounters)
            {
                DisposePerformanceCounters(existingPerfCounters);
                return GetPerformanceCounters(counterName);
            }

            private void DisposePerformanceCounters(IEnumerable<PerformanceCounter> performanceCounters)
            {
                if (performanceCounters is null) return;

                foreach (PerformanceCounter pc in performanceCounters)
                    pc.Dispose();
            }

            private double GetRate(IEnumerable<PerformanceCounter> counters)
            {
                if (counters is null) return 0.0;

                float rate = counters.Sum(c => c.NextValue());

                return Convert.ToDouble(rate);
            }

            private (double rate, string unit) GetRate(double bytes, ENetworkDisplayFormat displayFormat)
            {
                return displayFormat switch
                {
                    ENetworkDisplayFormat.Auto => GetAutoRate(bytes),
                    ENetworkDisplayFormat.Bytes => (bytes, MapUnit(EUnit.B)),
                    ENetworkDisplayFormat.Kilobytes => (ConvertToUnit(bytes, EUnit.B, EUnit.KB), MapUnit(EUnit.KB)),
                    ENetworkDisplayFormat.Megabytes => (ConvertToUnit(bytes, EUnit.B, EUnit.MB), MapUnit(EUnit.MB)),
                    ENetworkDisplayFormat.Gigabytes => (ConvertToUnit(bytes, EUnit.B, EUnit.GB), MapUnit(EUnit.GB)),
                    _ => throw new InvalidOperationException($"DisplayFormat '{displayFormat}' is NOT supported!")
                };
            }

            private static string MapUnit(EUnit unit)
                => unit switch
                {
                    EUnit.B => "B",
                    EUnit.KB => "K",
                    EUnit.MB => "M",
                    EUnit.GB => "G",
                    EUnit.TB => "T",
                    _ => ""
                };

            private (double rate, string unit) GetAutoRate(double bytes)
            {
                const double DIVISOR = 1024;

                double outValue = bytes / DIVISOR;
                string outUnit = MapUnit(EUnit.KB);

                foreach (EUnit unit in _convertToUnits)
                {
                    if (outValue < DIVISOR)
                        break;

                    outValue /= DIVISOR;
                    outUnit = MapUnit(unit);
                }

                return (outValue, outUnit);
            }
        }
    }
}
