using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Softplan.RDO.WebApi{

  public class SQLDataBase{

    private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();

    public SQLDataBase() {
      _builder.DataSource = @"172.21.8.81\iSAJ01";
      _builder.UserID = "saj";
      _builder.Password = "agesune1";
      _builder.InitialCatalog = "NETTINT";
    }

    public List<KeyValuePair<string, byte[]>> Select() {

        List<KeyValuePair<string, byte[]>> lista = new List<KeyValuePair<string, byte[]>>();

        using(SqlConnection connection = new SqlConnection(_builder.ConnectionString)){
          connection.Open();
          StringBuilder sb = new StringBuilder();

          sb.Append(@"select nuControleUnico, nmxml, blxml from SAJ.eproXML where tipoPeticao = 'ajuizamento'");
          String sql = sb.ToString();
          using (SqlCommand command = new SqlCommand(sql, connection)){
            using(SqlDataReader reader = command.ExecuteReader()){
              while(reader.Read()){
                lista.Add(new KeyValuePair<string, byte[]>((string)reader["nmxml"], (byte[])reader["blxml"]));
              }
            }
          }

        }

        return lista;
    }

  }

}