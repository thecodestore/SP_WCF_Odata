using System;
using System.Collections.Generic;
using System.Linq;

namespace ILX.EF.DL
{
	public class EOLEntities
	{
		private AdventureWorks2012Entities db;
		public EOLEntities()
		{
			db = new AdventureWorks2012Entities();// (EFHelper.BuildConnectionString("WIN2012DEV\\SQLWIN2010DEV", "AdventureWorks2012"));
			db.Configuration.AutoDetectChangesEnabled = true;
		}
		public List<TableA> GetTableA()
		{
			return db.TableAs.ToList();
		}
		public void UpdateTableA(TableA tblA)
		{
			using (var dbContextTransaction = db.Database.BeginTransaction())
			{

				var ent = db.TableAs.Find(tblA.ID);
				
				tblA.Description = ent.Title;
				ent.Description = ent.Title;
				ent.Title = tblA.Title;

				foreach (var item in ent.TableBs.ToList())
				{
					var tblB = tblA.TableBs.Where(a => a.ID == item.ID).FirstOrDefault();
					if (tblB == null)
					{
						db.TableBs.Remove(item);
					}
				}
				var ee = tblA.TableBs.Where(a => a.ID == 0).FirstOrDefault();
				ent.TableBs.Add(ee);
				//var diff2 = ent.TableBs
				//	.Where(a =>
				//	a.ID == tblA.TableBs.Where(ss => ss.ID == a.ID).FirstOrDefault().ID).ToList();
				//var diff = ent.TableBs.ToList().Except(tblA.TableBs);
				//var torem = ent.TableBs.Where(aaa => aaa.ID == 11).FirstOrDefault();
				//ent.TableBs.Remove(ent.TableBs.Where(aaa=> aaa.ID == 9).FirstOrDefault());
				//db.TableBs.Remove(torem);
				//var ss = ent.TableBs.Where(a =>
				//  tblA.TableBs == null ||
				//  a.TableAID != (tblA.TableBs.Where(b => b.TableAID == a.TableAID).FirstOrDefault().ID));
				////
				//
				//var diff = from aa in tblA.TableBs
				//		   join bb in ent.TableBs on aa.ID equals bb.ID select bb;
				//
				////
				//foreach (var itm  in ent.TableBs)
				//{
				//
				//	if (itm == null)
				//	{
				//		db.TableBs.Remove(itm);
				//	}
				//}

				//db.Entry(ent).State = System.Data.Entity.EntityState.Detached;
				//db.TableAs.Attach(tblA);//.Entry(ent).State = System.Data.Entity.EntityState.Modified;
				//db.Entry(ent).State = System.Data.Entity.EntityState.Modified;//.CurrentValues.SetValues(tblA);
				//db.Entry(ent).CurrentValues.SetValues(tblA);
				db.SaveChanges();
				dbContextTransaction.Commit();
			}
		}

		public IQueryable<Address> getAddress(System.Linq.Expressions.Expression<Func<Address, bool>> filter, out object obj)
		{
			//obj = null;
			//var outdata =db  (a => new { addresss = db.Addresses.Count(), people = db.People.Count() });
			var aobj = new
			{
				RestaurantsCount = db.Addresses.Count(),
				ShopsCount = db.BusinessEntities.Count(),
				ProductsCount = db.BusinessEntityAddresses.Count()
			};
			obj = from dummyRow in new List<string> { "X" }
						 join product in db.Addresses on 1 equals 1 into pg
						 join shop in db.BusinessEntities on 1 equals 1 into sg
						 join restaurant in db.BusinessEntityAddresses on 1 equals 1 into rg
						 select new
						 {
							 productsCount = pg.Count(),
							 shopsCount = sg.Count(),
							 restaurantsCount = rg.Count()
						 };

			return db.Addresses.Where(filter).OrderBy(aa => aa.City).Take(50);
		}
	}

	public class EntityWrapper<T>
	{
		public int TotalRecord { get; set; }
		public object objRows { get; set; }
		public List<T> Rows { get; set; }
		public string Message { get; set; }
	}
	public class AddressEntity
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
