using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Cajetan.Infobar.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        private static readonly BrushConverter _converter = new BrushConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not string hex)
                return Brushes.Black;

            object convertedBrush = _converter.ConvertFromString(hex);

            if (convertedBrush is not SolidColorBrush solidColorBrush)
                return Brushes.Black;
            
            return solidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();

    }
}
