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
        static ServerManager serverMgr = new ServerManager();

        static void Main(string[] args)
       {
            
            string line = "";
            //file where f00#'s are
            int count = File.ReadLines(@"c:\\Users\\csc313\\Desktop\\WebsitesToBeGenerated.txt").Count();

            //take in from text file
            System.IO.StreamReader file = new System.IO.StreamReader("c:\\Users\\csc313\\Desktop\\WebsitesToBeGenerated.txt");

            //read each line from file
            //reads does this for each username (F00 #)
            string[] usernames = new string[count];

            //input each line from the file to a string array
            for (int i = 0; i <= count; i++)
            {
                line = file.ReadLine();
                usernames[i] = line;
            }

            for(int j = 0; j <= count; j++)
            {
                
                Console.WriteLine(usernames[j] + " ----------    "+ j + " ---------------------");
                string siteName = usernames[j];

                string applicationPoolName = "DefaultAppPool";
                string virtualDirectoryPhysicalPath = "C:\\inetpub\\wwwroot\\";
                string ipAddress = "*";
                string tcpPort = "81";
                string hostHeader = "";

                //create folder for each website
                string subPath = virtualDirectoryPhysicalPath + "\\" + siteName; // your code goes here
                System.IO.Directory.CreateDirectory(subPath);
            
                
                try
                {
                    string bindinginfo = ipAddress + tcpPort + hostHeader;

                    //check if website name already exists in IIS
                    Boolean bWebsite = IsWebsiteExists(siteName);

                    if (!bWebsite)
                    {
                        //create a website 
                        Site mySite = serverMgr.Sites.Add(siteName, "http", bindinginfo, "C:\\inetpub\\wwwroot\\" +siteName);
                        mySite.ApplicationDefaults.ApplicationPoolName = applicationPoolName;
                        mySite.TraceFailedRequestsLogging.Enabled = true;
                        mySite.TraceFailedRequestsLogging.Directory = virtualDirectoryPhysicalPath;
                        serverMgr.CommitChanges();
                        Console.WriteLine("New website  " + siteName + " added sucessfully");
                    }
                    else
                    {
                        Console.WriteLine("Name should be unique, " + siteName + "  already exists. ");
                    }
                }
                catch (Exception ae)
                {
                    Console.WriteLine(ae.Message);
                }
            }
            
            file.Close();
            Console.WriteLine("There were {0} lines.", count);
            // Suspend the screen.
            Console.ReadLine();
        }

        public static bool IsWebsiteExists(string strWebsitename)
        {
            Boolean flagset = false;
            SiteCollection sitecollection = serverMgr.Sites;

            //checks to see if the site name given already exists
            foreach (Site site in sitecollection)
            {
                if (site.Name == strWebsitename.ToString())
                {
                    flagset = true;
                    break;
                }
                else
                {
                    flagset = false;
                }
            }
            return flagset;
        }

    }
}
