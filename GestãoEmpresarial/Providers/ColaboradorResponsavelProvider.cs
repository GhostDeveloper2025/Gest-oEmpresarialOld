using AutoCompleteTextBox.Editors;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Utils;
using GestãoEmpresarial.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GestãoEmpresarial.Providers
{
    internal class ColaboradorResponsavelProvider : ColaboradorProvider
    {
        protected override string TipoColaborador => "RESPONSAVEL";
    }
}
