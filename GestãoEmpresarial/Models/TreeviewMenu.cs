using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class TreeviewMenu
    {
        public string Header { get; set; }
        public PackIconKind Icon { get; set; } // Adicionando a propriedade Icon

        public ObservableCollection<TreeviewMenuItem> Items { get; set; }
    }
}
