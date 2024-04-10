using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace GestãoEmpresarial.Utils
{
    public class DecimalConverter : MarkupExtensionGestaoEmpresarial, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal valor)
            {
                return valor.ToString("N", culture); // Formato monetário (por exemplo, 100,00)
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Converte o valor de volta para decimal (ou o tipo que você estiver usando)
            if (decimal.TryParse(value as string, NumberStyles.Currency, culture, out decimal result))
            {
                return result;
            }
            return value;
        }

        //

        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value is decimal amount)
        //    {
        //        return string.Format(culture, "{0:N}", amount); // Formata o valor como moeda
        //    }
        //    return value;
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value is string text)
        //    {
        //        decimal result;
        //        if (decimal.TryParse(text, NumberStyles.Currency, culture, out result))
        //        {
        //            return result;
        //        }
        //    }
        //    return value;
    }
}
