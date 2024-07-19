using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaCategoriaViewModel : PesquisaViewModel<CategoriaModel, CategoriaModel>
    {
        public PesquisaCategoriaViewModel(IDAL<CategoriaModel> Repositorio) : base(Repositorio)
        {
        }

        public override int Id => ObjectoSelecionado.IdCategoria;

        public override string NomeEditarView => nameof(CadastroCategoriaViewModel);

        public override CategoriaModel GetDataGridModel(CategoriaModel item)
        {
            return item;
        }
    }
}