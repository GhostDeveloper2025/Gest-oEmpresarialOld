using GestãoEmpresarial.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace GestãoEmpresarial.CustomControls
{
    /// <summary>
    /// Interação lógica para CelularTextBox.xam
    /// </summary>
    public partial class CelularTextBox : TextBox
    {
        public CelularTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                string phoneNumber = textBox.Text;

                // Remove caracteres não numéricos
                phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]", "");

                // Garante que o comprimento máximo do número de celular seja respeitado
                if (phoneNumber.Length > 11)
                {
                    phoneNumber = phoneNumber.Substring(0, 11);
                }

                // Insere caracteres no formato desejado
                if (phoneNumber.Length >= 3)
                {
                    phoneNumber = $"({phoneNumber.Substring(0, 2)}) {phoneNumber.Substring(2)}";
                }
                if (phoneNumber.Length >= 10)
                {
                    phoneNumber = $"{phoneNumber.Substring(0, 9)}-{phoneNumber.Substring(9)}";
                }

                textBox.Text = phoneNumber;
                textBox.SelectionStart = phoneNumber.Length; // Mantém o cursor no final
            }
        }
    }
}
