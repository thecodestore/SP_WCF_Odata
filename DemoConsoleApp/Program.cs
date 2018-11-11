using ILX.EF.DL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoConsoleApp
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			EOLEntities ent = new EOLEntities();
			List<TableA> data = ent.GetTableA();
			TableA aa = new TableA();
			aa.ID = 1;

			aa.Title = System.DateTime.Now.ToString("yyyy-MM-dd (HH:mm:ss)");
			List<TableB> tlA = new List<TableB>();
			tlA.Add(new TableB { TableAID = 1, UserID=45});
			tlA.Add(new TableB { TableAID = 1, UserID=2 });
			tlA.Add(new TableB { TableAID = 1, UserID = 20 });
			//tlA.Add(new TableB { TableAID = 1, UserID = 3 });
			aa.TableBs = tlA;
			aa.TableDs = default(List<TableD>);

			TableA curdata = data.FirstOrDefault();
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
			//Console.ReadLine();
		}
	}
}
