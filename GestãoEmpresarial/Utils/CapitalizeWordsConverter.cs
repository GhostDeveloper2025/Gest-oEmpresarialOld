using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using static System.Resources.ResXFileRef;

namespace GestãoEmpresarial.Utils
{
    public class CapitalizeWordsConverter : MarkupExtensionGestaoEmpresarial, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return CapitalizeWords(str);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Se a conversão de volta não for necessária, você pode deixar este método vazio.
            return value;
        }

        private string CapitalizeWords(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string[] words = input.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    char[] letters = words[i].ToCharArray();
                    if (letters.Length > 0)
                    {
                        letters[0] = char.ToUpper(letters[0]);
                        words[i] = new string(letters);
                    }
                }
            }

            return string.Join(" ", words);
        }
    }
}
