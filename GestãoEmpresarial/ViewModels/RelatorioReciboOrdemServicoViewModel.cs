using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Repositorios.GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioReciboOrdemServicoViewModel : ObservableObject
    {
        //private readonly RRelatorioReciboDAL _relatorioReciboDAL;

        public RelatorioReciboOrdemServicoViewModel(RRelatorioReciboDAL relatorioReciboDAL /*, ROsDAL ROsDAL, int id*/)
        {
            //_relatorioReciboDAL = relatorioReciboDAL;

            //1. obter objecto da BD e colocar no propriedade
            //2. a propriedade faz bind na view
            //3. a impressao acontece
        }

        //AQUI NUMA PROP

        //public ClienteModel Cliente { get; set; }

        //public ProdutoModel Produto { get; set; }

        public OrdemServicoModel OrdemServicoModel { get; set; } // precisa do Repositorio ROsDAL

        // FAZER UMA PROP COM O FALTAR
    }
}
