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
        public PackIconKind Icon { get; set; }

        public Type Screen { get; set; }
        public object[] ScreenParameters { get; set; }

        public TreeviewMenuCollection Items { get; set; }
    }
}
