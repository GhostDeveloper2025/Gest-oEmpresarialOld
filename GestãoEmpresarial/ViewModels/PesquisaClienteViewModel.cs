using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System.Collections.Generic;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaClienteViewModel : PesquisaViewModel<ClienteModel, DataGridClienteModel, RClienteDAL>
    {
        public PesquisaClienteViewModel(RClienteDAL Repositorio) : base(Repositorio)
        {
        }

        public override int Id => ObjectoSelecionado.Idcliente;
        public override string NomeEditarView => nameof(CadastroClienteViewModel);

        public string PesquisaCPF { get; set; }
        public string PesquisaCNPJ { get; set; }
        public string PesquisaCelular { get; set; }
        public string PesquisaNome { get; set; }

        public override DataGridClienteModel GetDataGridModel(ClienteModel item)
        {
            return new DataGridClienteModel(item);
        }

        public override List<ClienteModel> GetLista()
        {
            return Repositorio.ListAsync(null, PesquisaCPF, PesquisaCNPJ, PesquisaCelular, PesquisaNome).Result;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return string.IsNullOrWhiteSpace(PesquisaCelular) == false
                || string.IsNullOrWhiteSpace(PesquisaCPF) == false
                || string.IsNullOrWhiteSpace(PesquisaCNPJ) == false
                || string.IsNullOrWhiteSpace(PesquisaNome) == false;
        }
    }
}