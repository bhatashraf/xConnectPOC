using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xConnectPOC.Models;
using xConnectPOC.Services.Client;
using xConnectPOC.Services.ContactService;

namespace xConnectPOC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Create()
        {
            ContactModel contactModel = new ContactModel();
            return View(contactModel);
        }
        [HttpPost]
        public ActionResult Create(ContactModel contactInfo)
        {
            if (!ModelState.IsValid)
                return View(contactInfo);

            XConnect xconnect = new XConnect();
            XConnectClient client = xconnect.GetClient();

            if (client == null)
            {
                ViewBag.message = "Something went wrong while connecting with xConnect, please retry!..";
                return View(contactInfo);
            }

            xDBContact contact = new xDBContact();
            contact.AddContact(client, contactInfo);
            contact.AddInteraction(client, contactInfo);
            ViewBag.message = contact.GetContact(client, contactInfo) ? "Contact added successfully!" : "Something went wrong while creating contact in xDB";
            return View(contactInfo);
        }
    }
}