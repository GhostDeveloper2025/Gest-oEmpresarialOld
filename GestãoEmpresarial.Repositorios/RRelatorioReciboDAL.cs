using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RRelatorioReciboDAL : DatabaseConnection
    {
        public RRelatorioReciboDAL(int idFuncionario) : base(idFuncionario)
        {
        }

        public void ObterReciboOrdemServico()
        {
        }

        public void ObterReciboVenda()
        {
        }
    }
}
