using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class RelatorioReciboOsModel //em vez de criar aqui meter na ViewModel
    {
        public OrdemServicoModel OsModel { get; set; }

        public string NomeResponsavel { get; set; }
        public string NomeTecnico { get; set; }
        public string NomeCliente { get; set; }
        public string NomeMarca { get; set; }
        public string NomeStatus { get; set; }
        public string NomeCadastrante { get; set; }

        public string TotalProduto { get { return "Total Produto: " + OsModel.TotalProduto.ToString("C"); } }
        public string TotalOS { get { return "Total OS: " + OsModel.TotalOS.ToString("C"); } }
        public string Desconto { get { return "Desconto: " + OsModel.TotalDescontoProduto.ToString("C"); } }
        public string MaoDeObra { get { return "Mão De Obra: " + OsModel.TotalMaoObra.ToString("C"); } }
    }
}
