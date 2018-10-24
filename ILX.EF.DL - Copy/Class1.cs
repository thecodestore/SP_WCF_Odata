using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILX.EF.DL
{
    public class EFHelper
    {
		public static String BuildConnectionString(String DataSource, String Database)
		{
			SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder()
			{
				DataSource = DataSource,
				InitialCatalog = Database,
				UserID = "spd\\svc.sql",
				Password = "Mypass@word1"
			};

			return sConnB.ConnectionString;
		}
	}
}
