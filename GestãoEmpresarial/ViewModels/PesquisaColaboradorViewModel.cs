using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaColaboradorViewModel : PesquisaViewModel<ColaboradorModel, ColaboradorModel, RColaboradorDAL>
    {
        public PesquisaColaboradorViewModel(RColaboradorDAL Repositorio) : base(Repositorio)
        {
        }

        public override int Id => ObjectoSelecionado.IdFuncionario;

        public override string NomeEditarView => nameof(CadastroColaboradorViewModel);

        public override ColaboradorModel GetDataGridModel(ColaboradorModel item)
        {
            return item;
        }

        public override List<ColaboradorModel> GetLista()
        {
            throw new NotImplementedException();
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
