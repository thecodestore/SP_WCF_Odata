/*****************************************************************************
 * IPresidentsService.cs
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
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
//using Barkes.Services.Presidents.Model;

namespace SharePointProject1.ISAPI.Insta.EF
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPortalService" in both code and config file together.
	[ServiceContract]
	public interface IPortalService { 
		[OperationContract]
		[WebGet(UriTemplate = "GetEmployees",
			ResponseFormat = WebMessageFormat.Json)]
		EntityWrapper<AddressEntity> GetEmployees();

		
	}
}

