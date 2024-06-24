using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class OrdemServicoModel
    {
        //cada vez que é chamado o new, este construtor é chamado
        public OrdemServicoModel()
        {
            ListItensOs = new List<ItemOrdemServicoModel>();
            // Defina o valor inicial da propriedade ObjectoEditar.Status para "O.S ABERTA!"
            //Status = EstadoOS.Aberta; //"O.S ABERTA!";
        }

        public IList<ItemOrdemServicoModel> ListItensOs { get; set; }

        public ColaboradorModel Cadastrante { get; set; }
        public ColaboradorModel Responsavel { get; set; }
        public ColaboradorModel Tecnico { get; set; }
        public ClienteModel Cliente { get; set; }

        public int? IdResponsavel
        {
            get
            {
                if (Responsavel == null)
                    return null;
                else
                    return Responsavel.IdFuncionario;
            }
        }
        public int? IdTecnico
        {
            get
            {
                if (Tecnico == null)
                    return null;
                else
                    return Tecnico.IdFuncionario;
            }
        }

        public int IdOs { get; set; }
        public bool Finalizado { get; set; }
        public int Status { get; set; }
        public string Box { get; set; }
        public int Marca { get; set; }
        public string Modelo { get; set; }
        public string Ferramenta { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataFinalizacao { get; set; }

        public decimal SubTotalProduto { get; set; }
        public decimal TotalProduto { get; set; }
        public decimal TotalDescontoProduto { get; set; }
        public decimal TotalOS { get; set; }
        public decimal TotalMaoObra { get; set; }

        public string Obs { get; set; }
        public bool Garantia { get; set; }
        public int IdCadastrante { get; set; }
        public int IdFuncionario { get; set; }
    }
}
