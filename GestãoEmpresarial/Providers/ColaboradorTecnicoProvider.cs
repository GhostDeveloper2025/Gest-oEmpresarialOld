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

namespace GestãoEmpresarial.Providers
{
    internal class ColaboradorTecnicoProvider : ColaboradorProvider
    {
        protected override string TipoColaborador => "TECNICO";
    }
}
