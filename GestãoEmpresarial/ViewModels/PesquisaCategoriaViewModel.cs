using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System.Collections.Generic;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaCategoriaViewModel : PesquisaViewModel<CategoriaModel, CategoriaModel, RCategoriaDAL>
    {
        public PesquisaCategoriaViewModel(RCategoriaDAL Repositorio) : base(Repositorio)
        {
        }

        public override int Id => ObjectoSelecionado.IdCategoria;

        public override string NomeEditarView => nameof(CadastroCategoriaViewModel);

        public override CategoriaModel GetDataGridModel(CategoriaModel item)
        {
            return item;
        }

        public override List<CategoriaModel> GetLista()
        {
            throw new System.NotImplementedException();
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}