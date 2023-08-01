using System;
using Xamarin.Forms;

namespace PriseDeCommande.Converters
{
    public class SelectedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                // Return "Blue" color when IsSelected is true
                return Color.Blue;
            }

            // Return a default color when IsSelected is false
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
