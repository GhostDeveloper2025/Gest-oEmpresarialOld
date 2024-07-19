using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaColaboradorViewModel : PesquisaViewModel<ColaboradorModel, ColaboradorModel>
    {
        public PesquisaColaboradorViewModel(IDAL<ColaboradorModel> Repositorio) : base(Repositorio)
        {
        }

        public override int Id => ObjectoSelecionado.IdFuncionario;

        public override string NomeEditarView => nameof(CadastroColaboradorViewModel);

        public override ColaboradorModel GetDataGridModel(ColaboradorModel item)
        {
            return item;
        }
    }
}
