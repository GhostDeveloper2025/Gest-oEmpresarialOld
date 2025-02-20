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
        // Cultura opcional para formatação
        public string CultureName { get; set; } = "en-US"; // Padrão: Inglês (EUA)

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                var targetCulture = CultureInfo.GetCultureInfo(CultureName);
                return decimalValue.ToString("N2", targetCulture); // Formata com 2 casas decimais
            }

            return value?.ToString() ?? string.Empty; // Retorna como string se não for decimal
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return 0m; // Retorna 0 se a string estiver vazia
            }

            var targetCulture = CultureInfo.GetCultureInfo(CultureName);

            if (decimal.TryParse(value.ToString(), NumberStyles.Currency, targetCulture, out decimal result))
            {
                return result;
            }

            throw new FormatException("O valor informado não está em um formato decimal válido.");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
