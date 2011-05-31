using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Xml;
using WowMvc.Models;
using ArmoryLib;
using ArmoryLib.Deserialization;

namespace WowMvc.Controllers
{
    public class GuildController : Controller
    {
        //
        // GET: /Guild/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Guild/Details/5

        public ActionResult Details(Guild guild)
        {
            return View(guild);
        }

        //
        // GET: /Guild/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Guild/Create

        [HttpPost]
        public ActionResult Create(Guild guild)
        {
            try
            {
                Armory armory = new Armory();
                XmlDocument guildDoc;
                try
                {
                    guildDoc = armory.RequestGuild(guild.Realm, guild.Name);
                }
                catch //(System.Web.HttpException ex)
                {
                    //return RedirectToAction("Error", ex.Message);
                    string errorString = "Ошибка запроса";
                    return View("GuildError", (object)errorString);
                }

                try
                {
                    XmlArmoryDeserializer ds = new XmlArmoryDeserializer();
                    ds.Init(typeof(Guild), guildDoc);
                    Guild resultGuild = (Guild)ds.Deserialize();

                    return View("Details",resultGuild);
                }
                catch
                {
                    string errorString ="Ошибка обработки запроса";
                    return View("GuildError",(object) errorString );
                }

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Guild/Error/5

        public ActionResult GuildError(string errorText)
        {
            return View(errorText);
        }
        
    }
}
