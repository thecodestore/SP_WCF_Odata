using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILX.EF.DL
{
	public class EOLEntities
	{
		AdventureWorks2012Entities db;
		public EOLEntities()
		{
			db = new AdventureWorks2012Entities();// (EFHelper.BuildConnectionString("WIN2012DEV\\SQLWIN2010DEV", "AdventureWorks2012"));
		}
		public IQueryable<Address> getAddress(System.Linq.Expressions.Expression< Func<Address, bool> >filter)
		{
			
			return db.Addresses.Where(filter).OrderBy(aa=> aa.City).Take(50);
		}
	}

	public class EntityWrapper<T>
	{
		public int TotalRecord { get; set; }
		public object objRows { get; set; }
		public List<T> Rows { get; set; }
		public string Message { get; set; }
	}
		public  class AddressEntity
	{
		public int AddressID { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public int StateProvinceID { get; set; }
		public string PostalCode { get; set; }
		public System.Data.Entity.Spatial.DbGeography SpatialLocation { get; set; }
		public System.Guid rowguid { get; set; }
		public System.DateTime ModifiedDate { get; set; }
	}
}
