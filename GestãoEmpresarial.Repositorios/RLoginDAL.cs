using GestãoEmpresarial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Repositorios
{
    public class RLoginDAL : DatabaseConnection
    {
        public RLoginDAL() : base(0)
        {
        }

        public ColaboradorModel AutenticaoValida(string usuario, string senha)
        {
            string query = "SELECT * FROM tb_funcionario WHERE Email = @usuario AND Senha = @senha";

            List<MySqlParameter> lista = new List<MySqlParameter>();
            AddParameter(lista, "@usuario", usuario);
            AddParameter(lista, "@senha", senha);

            using (var reader = ExecuteReader(query, lista.ToArray(), false))
            {
                while (reader.Read())
                {
                    return Mapeador.Map(new ColaboradorModel(), reader);
                }
            }
            return null;
        }
    }
}
