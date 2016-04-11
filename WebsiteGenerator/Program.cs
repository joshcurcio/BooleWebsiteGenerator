using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Web.Administration;

namespace WebsiteGenerator
{
    class Program
    {
       static void Main(string[] args)
       {
            
            string line = "";
            int count = File.ReadLines(@"c:\\Users\\Josh31\\Desktop\\test.txt").Count();
            //take in from text file
            System.IO.StreamReader file = new System.IO.StreamReader("c:\\Users\\Josh31\\Desktop\\test.txt");

            //read each line from file
            //reads does this for each username (F00 #)
            string[] usernames = new string[File.ReadLines(@"c:\\Users\\Josh31\\Desktop\\test.txt").Count()];
            while ((line = file.ReadLine()) != null)
            {
                int i = 0;
                Console.WriteLine(line);
                usernames[i] = line;
                i++;
            }

            for(int j = 0; j < count; j++)
            {
                Console.WriteLine(usernames[j]);
                string siteName = usernames[j];

                string applicationPoolName = "DefaultAppPool";
                string virtualDirectoryPath = "/";
                string virtualDirectoryPhysicalPath = "C:\\inetpub\\wwwroot";
                string ipAddress = "*";
                string tcpPort = "81";
                string hostHeader = "";
                string applicationPath = "/";
                long highestId = 1;

                string subPath = virtualDirectoryPhysicalPath + "\\" + siteName; // your code goes here

                System.IO.Directory.CreateDirectory(subPath);
                Console.WriteLine("here");

                using (ServerManager mgr = new ServerManager())
                {
                    Console.WriteLine("here1");
                    Site site = mgr.Sites["sitename" + j];
                    //error is here ------ below
                    if (site != null)
                    { 
                        Console.WriteLine(site);
                        Console.ReadLine();
                        return; // Site bestaat al
                    }
                    Console.WriteLine("here2");
                    ApplicationPool appPool = mgr.ApplicationPools[applicationPoolName];
                    if (appPool == null)
                        throw new Exception(String.Format("Application Pool: { 0 } does not exist.", applicationPoolName));
                    Console.WriteLine("here3");
                    foreach (Site mysite in mgr.Sites)
                    {
                        if (mysite.Id > highestId)
                            highestId = mysite.Id;
                        Console.WriteLine("here4" + mysite.Id);
                    }
                    highestId++;

                    Console.WriteLine("here5");
                    site = mgr.Sites.CreateElement();
                    site.SetAttributeValue("name", siteName);
                    site.Id = highestId;
                    site.Bindings.Clear();
                    Console.WriteLine("here 6");
                    string bind = ipAddress + ":" + tcpPort + ":" + hostHeader;

                    Binding binding = site.Bindings.CreateElement();
                    binding.Protocol = "http";
                    binding.BindingInformation = bind;
                    site.Bindings.Add(binding);
                    //site.Bindings.Add(bind, "http");
                    Console.WriteLine("here 7");
                    Application app = site.Applications.CreateElement();
                    app.Path = applicationPath;
                    app.ApplicationPoolName = applicationPoolName;
                    VirtualDirectory vdir = app.VirtualDirectories.CreateElement();
                    vdir.Path = virtualDirectoryPath;
                    vdir.PhysicalPath = virtualDirectoryPhysicalPath;
                    app.VirtualDirectories.Add(vdir);
                    site.Applications.Add(app);
                    Console.WriteLine("here8");
                    mgr.Sites.Add(site);
                    mgr.CommitChanges();

                    Console.WriteLine("website for " + siteName + " was created");

                }
            }
            
            file.Close();
            Console.WriteLine("There were {0} lines.", count);
            // Suspend the screen.
            Console.ReadLine();
        }
    }
}
