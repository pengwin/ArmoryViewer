using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Xml;
using System.Web;
using System.Net;

using System.Xml.Serialization;

namespace ArmoryLib
{


    public class Armory
    {
        public string DefaultUserAgent
        {
            get
            {
                return "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.5.20404)";
            }
        }

        void Decode(string file, string outFile)
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(file), Encoding.UTF8))
            {
                using (StreamWriter writer = new StreamWriter(File.OpenWrite(outFile), Encoding.GetEncoding(1251)))
                    writer.Write(reader.ReadToEnd());
            }
        }

        public XmlDocument Request(string armoryRequest)
        {
            XmlDocument armoryResponse = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                client.Headers.Set("User-Agent", DefaultUserAgent);
                client.Encoding = System.Text.Encoding.UTF8;
                armoryResponse.LoadXml(client.DownloadString(armoryRequest));
            }

            return armoryResponse;
        }



        public XmlDocument RequestCharacter(string realmName, string characterName)
        {
            string Url = "http://eu.wowarmory.com/";
            string searchString = string.Format("character-sheet.xml?r={0}&n={1}",
                                                 HttpUtility.UrlEncode(realmName),
                                                 HttpUtility.UrlEncode(characterName));
            string armoryRequest = Url + searchString;
            return Request(armoryRequest);
        }

        public XmlDocument RequestGuild(string realmName, string guildName)
        {
            string Url = "http://eu.wowarmory.com/";
            string searchString = string.Format("guild-info.xml?r={0}&n={1}",
                                                 HttpUtility.UrlEncode(realmName),
                                                 HttpUtility.UrlEncode(guildName));
            string armoryRequest = Url + searchString;
            return Request(armoryRequest);
        }
    }
}
