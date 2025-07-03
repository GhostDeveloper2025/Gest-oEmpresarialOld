using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GestãoEmpresarial.CustomControls;

namespace GestãoEmpresarial.Utils
{
    // Criei aqui para você
    public static class MenuStateManager
    {
        private static Expander _lastExpandedMenu;

        public static void RegisterMenu(Expander expander)
        {
            expander.Expanded += (sender, e) =>
            {
                if (_lastExpandedMenu != null && _lastExpandedMenu != sender)
                {
                    _lastExpandedMenu.IsExpanded = false;
                }
                _lastExpandedMenu = (Expander)sender;
            };
        }
    }

}
