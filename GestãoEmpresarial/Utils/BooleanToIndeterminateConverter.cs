﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GestãoEmpresarial.Utils
{
    public class BooleanToIndeterminateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
                return booleanValue; // Retorna o valor booleano para a propriedade IsIndeterminate

            return false; // Caso o valor não seja booleano, retorna false por padrão
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
