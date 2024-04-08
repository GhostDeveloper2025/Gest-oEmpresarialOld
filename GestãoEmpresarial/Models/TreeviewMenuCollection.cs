using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class TreeviewMenuCollection : ObservableCollection<TreeviewMenu>
    {
        public void Add(string header, PackIconKind icon)
        {
            var obj = new TreeviewMenu()
            {
                Header = header,
                Icon = icon,
            };
            Add(obj);
        }

        public void Add(string header, PackIconKind icon, Type screen)
        {
            var obj = new TreeviewMenu()
            {
                Header = header,
                Icon = icon,
                Screen = screen,
            };
            Add(obj);
        }

        public void Add(string header, PackIconKind icon, Type screen, object[] parameters)
        {
            var obj = new TreeviewMenu()
            {
                Header = header, 
                Icon = icon,
                Screen  = screen,
                ScreenParameters = parameters
            };
            Add(obj);
        }
    }
}
