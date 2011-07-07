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
   
    /// <summary>
    /// Get XmlResponse from WowArmory
    /// </summary>
    public class Armory
    {
        // Contains indexes of strings in _regions array
        public enum ArmoryRegions : int
        {
            Europe = 0,
        }

        private string[] _regions = new string[] { "http://eu.wowarmory.com/" };

        private string DefaultUserAgent=  "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.5.20404)";

        private XmlDocument Request(ArmoryRegions region, string searchString)
        {
            XmlDocument armoryResponse = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string Url = _regions[ (int) region];
                string armoryRequest = Url + searchString;

                client.Headers.Set("User-Agent", DefaultUserAgent);
                client.Encoding = System.Text.Encoding.UTF8;
                armoryResponse.LoadXml(client.DownloadString(armoryRequest));
            }

            return armoryResponse;
        }

        public XmlDocument RequestCharacter(ArmoryRegions region, string characterName, string realmName)
        {     
            string searchString = string.Format("character-sheet.xml?r={0}&n={1}",
                                                 HttpUtility.UrlEncode(realmName),
                                                 HttpUtility.UrlEncode(characterName));
            return Request(region, searchString);
        }

        public XmlDocument RequestGuild(ArmoryRegions region, string realmName, string guildName)
        {
            string searchString = string.Format("guild-info.xml?r={0}&n={1}",
                                                 HttpUtility.UrlEncode(realmName),
                                                 HttpUtility.UrlEncode(guildName));
            return Request(region, searchString);
        }
    }
}
