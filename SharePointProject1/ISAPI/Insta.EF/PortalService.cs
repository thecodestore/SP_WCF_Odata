
/*****************************************************************************
 * PresidentsService.svc.cs
 * WCF REST service hosted in SharePoint 2013
 * 
 * Copyright (c) Jason Barkes.
 * All rights reserved.
 * http://jbarkes.blogspot.com
 *
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 *****************************************************************************/

using ILX.EF.DL;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.ServiceModel.Activation;
//using Barkes.Services.Presidents.Model;

namespace SharePointProject1.ISAPI.Insta.EF
{
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PortalService" in both code and config file together.
	public class PortalService : IPortalService
	{
		#region IPresidentsService Implementation

		public EntityWrapper<AddressEntity> GetEmployees()
		{
			var output = new EntityWrapper<AddressEntity>();
			List<AddressEntity> data = null;
			try
			{
				//IQueryable<T> Filter(Expression<Func<T, bool>> filter);
				List<Filter> filter = new List<Filter>()
					{
						new Filter { PropertyName = "AddressLine1" ,
							Operation = Op .Contains, Value = "Dr"  },
						new Filter { PropertyName = "City" ,
							Operation = Op .StartsWith, Value = "Ke"  },
						//new Filter { PropertyName = "Salary" ,
						//	Operation = Op .GreaterThan, Value = 9000.0 }
					};

				var deleg = ExpressionBuilder.GetExpression<Address>(filter);
				//var filteredCollection = persons.Where(deleg).ToList();

				SPSecurity.RunWithElevatedPrivileges(delegate ()
				{
					EOLEntities ent = new EOLEntities();
					var adata = ent.getAddress(deleg)
					.Select(aaa => new AddressEntity
					{   AddressLine1 = aaa.AddressLine1,
						AddressLine2 = aaa.AddressLine2,
						AddressID = aaa.AddressID }).ToList();
					output.TotalRecord = adata.Count;
					output.Message = "Successful";
					output.Rows = adata;//.ToList<Object>().Select(a=> (AddressEntity)a).ToList();//.ToList();//.Select(a=> (AddressEntity)a);
				});
			}
			catch (Exception ex)
			{
				output.Message = ex.ToString();
			}

			return output;//.Select(a=>(Address)a).ToList();

			#endregion

		}
		
		T Cast<T>(object obj, T type)
		{
			return (T)obj;
		}
	}
}
