using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GestãoEmpresarial.CustomControls
{
    public class DataGridAuto : DataGrid
    {
        public DataGridAuto()
        {
            AutoGeneratingColumn += OnAutoGeneratingColumn;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var prop = ((PropertyDescriptor)e.PropertyDescriptor);
            if (prop.Attributes.OfType<DisplayNameAttribute>().Any() == false)
                e.Cancel = true;

            e.Column.Header = prop.DisplayName;

            //e.Column.Header = ((PropertyDescriptor)e.PropertyDescriptor).DisplayName;
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = GetDataFormato(prop);
            else if (e.PropertyType == typeof(System.DateTime?))
                (e.Column as DataGridTextColumn).Binding.StringFormat = GetDataFormato(prop);
        }

        private string GetDataFormato(PropertyDescriptor propertyDescriptor)
        {
            //DisplayFormat
            var lista = propertyDescriptor.Attributes.OfType<DisplayFormatAttribute>(); //.Any()
            if (lista.Any())
                return lista.First().DataFormatString;
            else
                return "dd/MM/yyyy HH:mm:ss";
        }
    }
}
