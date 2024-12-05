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
        public string FiltroGlobal { get; set; }
        public override CategoriaModel GetDataGridModel(CategoriaModel item)
        {
            return item;
        }

        public override List<CategoriaModel> GetLista()
        {
            var lista = Repositorio.ListAsync(FiltroGlobal).Result;

            FiltroGlobal = null;
            RaisePropertyChanged(nameof(FiltroGlobal));
            return lista;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return string.IsNullOrWhiteSpace(FiltroGlobal) == false;
        }

    }
}