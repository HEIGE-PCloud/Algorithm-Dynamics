using Microsoft.UI.Xaml.Data;
using System;

namespace Algorithm_Dynamics.Helpers
{
    internal class IntToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Enum enumValue = default;
            if (parameter is Type)
            {
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }
    }
}
