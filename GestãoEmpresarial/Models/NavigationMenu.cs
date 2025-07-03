using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GestãoEmpresarial.Models
{
    public class NavigationMenu
    {
        public string Header { get; set; }

        public PackIconKind Icon { get; set; }

        public Func<UserControl> GetView { get; set; }

        public NavigationMenuCollection Items { get; set; }

    }
}
