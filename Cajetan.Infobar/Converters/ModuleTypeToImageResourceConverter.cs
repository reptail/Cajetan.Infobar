using Cajetan.Infobar.Domain.Models;
using System;
using System.Windows.Data;

namespace Cajetan.Infobar.Converters
{
    class ModuleTypeToImageResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string size = "16";

            if (parameter is string strSize)
                size = strSize;

            string image = value switch
            {
                EModuleType.BatteryStatus => "battery_charging",
                EModuleType.MemoryUsage => "memory",
                EModuleType.NetworkUsage => "network",
                EModuleType.ProcessorUsage => "cpu",
                EModuleType.Uptime => "uptime",
                _ => null
            };

            if (image is null)
                return "pack://application:,,,/Resources/Images/empty.png";

            return $"pack://application:,,,/Resources/Images/{size}/{image}_{size}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();
    }
}
