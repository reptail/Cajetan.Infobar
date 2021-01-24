using Cajetan.Infobar.Domain.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Cajetan.Infobar.Converters
{
    public class ModuleTypeToFontAwesomeRotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not EModuleType moduleType) 
                return value;

            return moduleType == EModuleType.BatteryStatus ? 270 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
