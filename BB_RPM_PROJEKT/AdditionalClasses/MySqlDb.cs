using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB_RPM_PROJEKT.AdditionalClasses
{
    internal class MySqlDb
    {
        public static MySqlConnection GetDBConnection()
        {
            string connStr = "server=localhost;user=root;database=bbkai;password=87654321;";
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }
    }
}
