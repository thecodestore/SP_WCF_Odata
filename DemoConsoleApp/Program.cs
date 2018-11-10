using ILX.EF.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			EOLEntities ent = new EOLEntities();
			List<TableA> data = ent.GetTableA();
			var aa = new TableA();
			aa.ID = 1;
		
			aa.Title = System.DateTime.Now.ToString("yyyy-MM-dd (HH:mm:ss)");
			var tlA = new List<TableB>();
			tlA.Add(new TableB { ID=9,  TableAID=1, TableA =aa });
			tlA.Add(new TableB {  TableAID = 1 });
			aa.TableBs = tlA;
			aa.TableDs = default(List<TableD>);

			var curdata = data.FirstOrDefault();
			//Console.WriteLine("---------------------------");
			//if (curdata.TableBs.Count >0)
			//{
			//	var entTbl = curdata.TableBs.FirstOrDefault();
			//	curdata.TableBs.Remove(entTbl);
			//	Console.WriteLine(entTbl.ID);
			//	Console.WriteLine("---------------------------");
			//}

			ent.UpdateTableA(aa);
		 	Console.WriteLine("Total rows: " + data.Count.ToString());
			Console.ReadLine();
		}
	}
}
