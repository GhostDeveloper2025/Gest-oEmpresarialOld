using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GestãoEmpresarial.Utils
{
    public class DecimalConverterSimples : MarkupExtensionGestaoEmpresarial, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return 0m; // Retorna 0 se a string estiver vazia
            }

            if (decimal.TryParse(value.ToString(), out decimal result))
            {
                return result;
            }

            throw new FormatException("A cadeia de caracteres de entrada não estava em um formato correto.");
        }

    }
}
