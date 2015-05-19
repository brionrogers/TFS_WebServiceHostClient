using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using SelfHost2.ServiceModel;
//using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Server;
using System.Collections.ObjectModel;


namespace SelfHost2.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(TFS request)
        {
            return new HelloResponse { Result = "Hello, {0}!".Fmt(request.Name) };
        }
    }

    public class TFSService : Service
    {
        public void Any (TFSService request)
        {
            
        }

        // It then returns the response to the client.
        public object Post(LoginRequest request)
        {
            Driver driver = new Driver(request.username, request.password, request.uri);

            List<String> pList = new List<String>();

            pList = driver.getTFS(driver.Uri);

            var response = new LoginResponse
            {
                WI_List = pList
            };

            return response;
        }

    }

}