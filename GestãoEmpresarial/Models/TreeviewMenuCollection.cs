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
    public class NavigationMenuCollection : ObservableCollection<NavigationMenu>
    {
        public void Add(string header, PackIconKind icon)
        {
            var obj = new NavigationMenu()
            {
                Header = header,
                Icon = icon,
            };
            Add(obj);
        }

        public void Add(string header, PackIconKind icon, Func<UserControl> getView)
        {
            var obj = new NavigationMenu()
            {
                Header = header,
                Icon = icon,
                GetView = getView,
            };
            Add(obj);
        }

    }
}
