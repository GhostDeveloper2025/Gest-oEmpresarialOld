using GestãoEmpresarial.Models.Atributos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;


namespace GestãoEmpresarial.Utils
{
    public static class Methods
    {
        //Recursividade - é a habilidade de um metodo/funcao efetuar um ciclo chamando se a si mesmo
        //temos de ter cuidado, porque temos de colocar uma condicao de paragem!
        public static void AddColumnToDataGrid(DataGrid dgItems, Type tt, string propriedadePai = "")
        {
            //Crie as colunas com base nos atributos personalizados
            //Este código criará automaticamente colunas na DataGrid com base nos atributos personalizados da classe Person.
            //Certifique-se de definir o DataContext como this para vincular a DataGrid aos dados na classe MainWindow.
            foreach (var property in tt.GetProperties())
            {
                var columnHeaderAttribute = property.GetCustomAttribute<ColumnHeaderAttribute>();
                if (columnHeaderAttribute != null)
                {
                    //vinculo = "CodProduto"
                    string vinculo = property.Name;
                    if (string.IsNullOrWhiteSpace(propriedadePai) == false)
                    {
                        //vinculo = "ProdutoObj" + "." + "CodProduto"
                        vinculo = propriedadePai + "." + vinculo;
                    }

                    var column = new DataGridTextColumn
                    {
                        Header = columnHeaderAttribute.TextoCabecalho,
                        Binding = new Binding(vinculo) //Name //CodProduto 
                    };
                    dgItems.Columns.Add(column);
                }
                else if (IsNonStringClass(property.PropertyType))
                    //para fazer recursividade, e adicionar mais colunas das classes que eu quero
                    AddColumnToDataGrid(dgItems, property.PropertyType, property.Name);
            }
        }

        //verifica se o tipo é classe, exclui string
        private static bool IsNonStringClass(Type type)
        {
            if (type == null || type == typeof(string))
                return false;
            return type.IsClass;
        }

        //metodo para usar moeda na textebox no formato en-US para nao conflitar
        public static void Moeda(TextBox text)
        {
            try
            {
                string input = text.Text
                    .Replace(".", "")
                    .Replace(",", "")
                    .Trim();

                if (string.IsNullOrEmpty(input))
                {
                    text.Text = "0.00";
                    text.SelectionStart = text.Text.Length;
                    return;
                }

                // Converte para decimal para evitar imprecisões
                decimal value = decimal.Parse(input) / 100;

                // Aplica o formato no estilo en-US
                text.Text = value.ToString("N2", CultureInfo.GetCultureInfo("en-US"));

                // Garante que o cursor fique no final do texto
                text.SelectionStart = text.Text.Length;
            }
            catch
            {
                // Lida com erros de forma silenciosa (não recomendado para produção)
            }
        }

        //Método para formatar CPF
        public static void FormatCPF(ref TextBox text)
        {
            if (text != null)
            {
                string cpf = text.Text;

                // Remove caracteres não numéricos
                cpf = Regex.Replace(cpf, @"[^0-9]", "");

                //Garante que o comprimento máximo do CPF seja respeitado
                if (cpf.Length > 11)
                {
                    cpf = cpf.Substring(0, 11);
                }

                //Insere pontos e traço nos lugares apropriados
                if (cpf.Length >= 3)
                {
                    cpf = cpf.Insert(3, ".");
                }
                if (cpf.Length >= 7)
                {
                    cpf = cpf.Insert(7, ".");
                }
                if (cpf.Length >= 11)
                {
                    cpf = cpf.Insert(11, "-");
                }

                text.Text = cpf;
                text.CaretIndex = cpf.Length; // Mantém o cursor no final
            }
        }
        //Método para formatar CNPJ
        public static void FormatCNPJ(ref TextBox text)
        {
            if (text != null)
            {
                string cnpj = text.Text;

                // Remove caracteres não numéricos
                cnpj = Regex.Replace(cnpj, @"[^0-9]", "");

                // Garante que o comprimento máximo do CNPJ seja respeitado
                if (cnpj.Length > 14)
                {
                    cnpj = cnpj.Substring(0, 14);
                }

                // Insere pontos, barras e traço nos lugares apropriados
                if (cnpj.Length >= 2)
                {
                    cnpj = cnpj.Insert(2, ".");
                }
                if (cnpj.Length >= 6)
                {
                    cnpj = cnpj.Insert(6, ".");
                }
                if (cnpj.Length >= 10)
                {
                    cnpj = cnpj.Insert(10, "/");
                }
                if (cnpj.Length >= 15)
                {
                    cnpj = cnpj.Insert(15, "-");
                }

                text.Text = cnpj;
                text.CaretIndex = cnpj.Length; // Mantém o cursor no final
            }
        }
        //Método para formatar CEP
        public static void FormatCEP(ref TextBox text)
        {
            if (text != null)
            {
                string cep = text.Text;

                // Remove caracteres não numéricos
                cep = Regex.Replace(cep, @"[^0-9]", "");

                // Garante que o comprimento máximo do CEP seja respeitado
                if (cep.Length > 8)
                {
                    cep = cep.Substring(0, 8);
                }

                // Insere traço no lugar apropriado
                if (cep.Length >= 5)
                {
                    cep = cep.Insert(5, "-");
                }

                text.Text = cep;
                text.CaretIndex = cep.Length; // Mantém o cursor no final
            }
        }

        //Método para formatar Telefone
        //public static void FormatTelefoneFixo(ref TextBox text)
        //{
        //    if (text != null)
        //    {
        //        string phoneNumber = text.Text;

        //        // Remove caracteres não numéricos
        //        phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]", "");

        //        // Garante que o comprimento máximo do número de telefone seja respeitado
        //        if (phoneNumber.Length > 10)
        //        {
        //            phoneNumber = phoneNumber.Substring(0, 10);
        //        }

        //        // Insere caracteres no formato desejado
        //        if (phoneNumber.Length >= 2)
        //        {
        //            phoneNumber = $"({phoneNumber.Substring(0, 2)}) {phoneNumber.Substring(2)}";
        //        }
        //        if (phoneNumber.Length >= 9)
        //        {
        //            phoneNumber = $"{phoneNumber.Substring(0, 9)}-{phoneNumber.Substring(9)}";
        //        }

        //        text.Text = phoneNumber;
        //        text.SelectionStart = phoneNumber.Length; // Mantém o cursor no final
        //    }
        //}

        //Novo Testes
        public static void FormatTelefoneFixo(TextBox textBox)
        {
            if (textBox != null)
            {
                string phoneNumber = textBox.Text;

                // Remove caracteres não numéricos
                phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]", "");

                // Garante que o comprimento máximo do número de telefone seja respeitado
                if (phoneNumber.Length > 10)
                {
                    phoneNumber = phoneNumber.Substring(0, 10);
                }

                // Insere caracteres no formato desejado
                StringBuilder formattedPhoneNumber = new StringBuilder();
                if (phoneNumber.Length >= 2)
                {
                    formattedPhoneNumber.Append($"({phoneNumber.Substring(0, 2)}) ");
                    if (phoneNumber.Length > 2)
                    {
                        formattedPhoneNumber.Append(phoneNumber.Substring(2));
                    }
                }
                else
                {
                    formattedPhoneNumber.Append(phoneNumber);
                }

                if (formattedPhoneNumber.Length >= 9)
                {
                    formattedPhoneNumber.Insert(9, "-");
                }

                textBox.Text = formattedPhoneNumber.ToString();
                textBox.SelectionStart = textBox.Text.Length; // Mantém o cursor no final
            }
        }


        //Método para formatar Celular
        public static void FormatTelefoneCelular(ref TextBox text)
        {
            if (text != null)
            {
                string phoneNumber = text.Text;

                // Remove caracteres não numéricos
                phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]", "");

                // Garante que o comprimento máximo do número de telefone seja respeitado (11 dígitos)
                if (phoneNumber.Length > 11)
                {
                    phoneNumber = phoneNumber.Substring(0, 11);
                }

                // Insere caracteres no formato desejado
                if (phoneNumber.Length >= 2)
                {
                    phoneNumber = $"({phoneNumber.Substring(0, 2)}) {phoneNumber.Substring(2)}";
                }
                if (phoneNumber.Length >= 10)
                {
                    phoneNumber = $"{phoneNumber.Substring(0, 10)}-{phoneNumber.Substring(10)}";
                }

                text.Text = phoneNumber;
                text.SelectionStart = phoneNumber.Length; // Mantém o cursor no final
            }
        }
    }
}
