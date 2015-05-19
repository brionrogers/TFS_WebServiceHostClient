using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;
using SelfHost2.ServiceModel;

namespace SelfHost2
{
    class Program
    {
        static void Main(string[] args)
        {
            new AppHost().Init().Start("http://*:8088/");
            "ServiceStack SelfHost listening at http://localhost:8088 ".Print();
            Process.Start("http://localhost:8088/");

            List<WorkItem> WI_List = new List<WorkItem>();

            // Obtain user credentials and TFS IP address
            Console.Write("Enter Username: ");
            String uname = Console.ReadLine();
            Console.Write("Enter Password: ");
            String pword = Console.ReadLine();
            Console.Write("Enter TFS IP address: ");
            String URI = Console.ReadLine();
            String actualURI = "http://" + URI;

            // Create client
            var Client = new JsonServiceClient("http://localhost:8088");

            // Build response from ServiceModel
            var Response = Client.Post<LoginResponse>(
                                                       new LoginRequest
                                                            {
                                                                username = uname,
                                                                password = pword,
                                                                uri = actualURI
                                                            });

            // Display response in user friendly format (For testing purposes)
            Console.WriteLine();
            Console.WriteLine("Response Details");
            Console.WriteLine("================");

            foreach (String item in Response.WI_List)
            {
                Console.WriteLine(item);
                Console.WriteLine("----------------------------------");

                String[] stringList = item.Split('/');
                WorkItem wi = new WorkItem();
                wi.ID = stringList[3];
                wi.Title = stringList[2];
                wi.State = stringList[1];
                wi.AssignedTo = stringList[0];
                wi.Effort = stringList[4];

                WI_List.Add(wi);
            }

            foreach (WorkItem wi in WI_List)
            {
                wi.toString();
                Console.WriteLine("===================================");
            }

            Console.ReadLine();
        }

        // WorkItem class to pass to View Controller for displaying purposes
        class WorkItem
        {
            public String ID { get; set; }
            public String Title { get; set; }
            public String State { get; set; }
            public String AssignedTo { get; set; }
            public String Effort { get; set; }

            // toString function used for testing
            public void toString()
            {
                Console.WriteLine(string.Format("{0}: {1}", "ID", ID));
                Console.WriteLine(string.Format("{0}: {1}", "Title", Title));
                Console.WriteLine(string.Format("{0}: {1}", "State", State));
                Console.WriteLine(string.Format("{0}: {1}", "AssignedTo", AssignedTo));
                Console.WriteLine(string.Format("{0}: {1}", "Effort", Effort));

            }

        }
    }
}
