using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfApiClient2022.Models;

namespace WpfApiClient2022.ViewModels
{
    internal class Convertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Actor)
            {
                return ((Actor)value).FirstName + " " + ((Actor)value).LastName;
            }  
            if(value is Movie)
            {
                return ((Movie)value).Title + " " + ((Movie)value).Description;
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
