using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class TreeviewMenuItem
    {
        public PackIconKind Icon { get; set; }
        public PackIconKind InnerIcon { get; set; }

        public string Header { get; set; }
        public string InnerHeader { get; set; }

        public Type Screen { get; set; }
        public object[] ScreenParameters { get; set; }

        public ObservableCollection<TreeviewMenuItem> Items { get; set; }
    }
}
