using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.TeamFoundation;
using ServiceStack;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;

namespace SelfHost2.ServiceModel
{
    [Route("/hello/{Name}")]
    public class TFS : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }


    public class LoginRequest
    {
        public String username { get; set; }
        public String password { get; set; }
        public String uri { get; set; }
    }

    /* Class for LoginResponse. Takes List of Strings from TFS_ServiceLogic.cs
 * and creates Data Transfer object to be sent to client.
 */
    public class LoginResponse
    {
        public List<String> WI_List { get; set; }
    }

    public class Project
    {
        public String ID { get; set; }
        public String Title { set; get; }
        public String Description { set; get; }
        public String Team { set; get; }
        public String Status { set; get; }

        // List of tasks associated with the Project. "Tasks" are a concatenated string of all task data.
        public List<String> TaskList = new List<String>();

        // Constructor using title and status of project
        public Project(String title, String status)
        {
            Title = title;
            Status = status;
        }

        // Add a task to the TaskList
        public void AddTask(String task)
        {
            TaskList.Add(task);
        }

        // Display information in user friendly format (Testing purposes)
        public void toString()
        {
            Console.WriteLine("Project Title:  " + Title + "\n" +
                              "Project Status: " + Status);
            foreach (String task in TaskList)
            {
                Console.WriteLine(task);
            }
        }
    }



    // Class that contains all logic regarding pulling information from TFS
    public class Driver
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String Uri { get; set; }

        // Constructor that uses user credentials and TFS IP address
        public Driver(String username, String password, String uri)
        {
            // Current version of program does not do anything with user credentials. All projects and tasks in TFS are pulled.
            Username = username;
            Password = password;
            Uri = uri;
        }

        public List<String> getTFS(String URI)
        {
            // List of projects to wrap everything up in
            List<Project> ProjectList = new List<Project>();

            // Connect to Team Foundation Server
            Uri tfsUri = new Uri(URI);

            TfsConfigurationServer configurationServer =
                TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);

            // Get the catalog of team project collections
            ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(
                new[] { CatalogResourceTypes.ProjectCollection },
                false, CatalogQueryOptions.None);

            foreach (CatalogNode collectionNode in collectionNodes)
            {
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                // Print the name of the team project collection
                Console.WriteLine("Collection: " + teamProjectCollection.Name);

                // Create a WorkItemStore object from TFS Work Item Store
                var wis = teamProjectCollection.GetService<WorkItemStore>();

                ICommonStructureService Iis = (ICommonStructureService)teamProjectCollection.GetService(typeof(ICommonStructureService));
                ProjectInfo[] projectInfo = Iis.ListAllProjects();

                // Iterate through the project list and get list of Work Items (Tasks) in each project.
                foreach (var project in projectInfo)
                {
                    // Create generic project object from each TFS Project
                    Project p = new Project(project.Name.ToString(), project.Status.ToString());

                    WorkItemCollection wic = wis.Query(" SELECT [System.Id], [System.WorkItemType]," +
                                        " [System.State], [System.AssignedTo], [System.Title]" +
                                        " FROM WorkItems" +
                                        " WHERE [System.TeamProject] = \"" + project.Name + "\"" +
                                        " ORDER BY [System.WorkItemType], [System.Id]");

                    // Iterate through each task in each project to get some data
                    foreach (WorkItem wi in wic)
                    {
                        // Create "Task" String for each TFS Work Item
                        String workItemString = "";
                        foreach (Field field in wi.Fields)
                        {
                            if (field.Name.Equals("ID")) workItemString += (field.Value.ToString() + "/");
                            else if (field.Name.Equals("Title")) workItemString += (field.Value.ToString() + "/");
                            else if (field.Name.Equals("State")) workItemString += (field.Value.ToString() + "/");
                            else if (field.Name.Equals("Assigned To")) workItemString += (field.Value.ToString() + "/");
                            else if (field.Name.Equals("Effort")) workItemString += (field.Value.ToString() + "/");
                            else ;
                        }
                        // Add Task String to TaskList
                        p.AddTask(workItemString);
                    }
                    // Add project to ProjectList
                    ProjectList.Add(p);
                }
            }

            // Iterate through each Project in ProjectList and extract each Task.
            List<String> list = new List<String>();
            foreach (Project p in ProjectList)
            {
                // Add each Task String to List to be packed into Data Transfer Object defined in TFS_ServiceModel.cs
                foreach (String task in p.TaskList)
                {
                    list.Add(task);
                }
            }

            // Return List of Task Strings
            return list;
        }
    }


}