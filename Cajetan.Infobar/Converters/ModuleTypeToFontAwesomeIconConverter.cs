using Cajetan.Infobar.Domain.Models;
using FontAwesome5;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Cajetan.Infobar.Converters
{
    public class ModuleTypeToFontAwesomeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not EModuleType moduleType) 
                return value;

            return moduleType switch
            {
                EModuleType.Uptime => EFontAwesomeIcon.Regular_Clock,
                EModuleType.BatteryStatus => EFontAwesomeIcon.Solid_BatteryThreeQuarters,
                EModuleType.ProcessorUsage => EFontAwesomeIcon.Solid_Microchip,
                EModuleType.MemoryUsage => EFontAwesomeIcon.Solid_Memory,
                EModuleType.NetworkUsage => EFontAwesomeIcon.Solid_NetworkWired,
                _ => EFontAwesomeIcon.Solid_Question
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
