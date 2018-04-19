using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using xConnectPOC.Models;
namespace xConnectPOC.Services.ContactService
{
    public class xDBContact
    {
        public void AddContact(XConnectClient client, ContactModel contactModel)
        {
            
                var identifiers = new ContactIdentifier[]
                {
                    new ContactIdentifier("twitter", contactModel.FirstName+contactModel.LastName, ContactIdentifierType.Known)                  
                };
                var contact = new Contact(identifiers);

                var personalInfoFacet = new PersonalInformation
                {
                    FirstName = contactModel.FirstName,
                    LastName = contactModel.LastName
                };
                client.SetFacet<PersonalInformation>(contact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                var emailFacet = new EmailAddressList(new EmailAddress(contactModel.Email, true), "twitter");
                client.SetFacet<EmailAddressList>(contact, EmailAddressList.DefaultFacetKey, emailFacet);

                client.AddContact(contact);

                client.Submit();
  
        }
        public void AddInteraction(XConnectClient client, ContactModel contactModel)
        {
            var contactReference = new IdentifiedContactReference("twitter", contactModel.FirstName + contactModel.LastName);
            var contact = client.Get(contactReference, new ExpandOptions () { FacetKeys = { "Personal"} });
            if (contact!=null)
            {
                // Item ID of the "Enter Store" Offline Channel at 
                // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Offline/Store/Enter store
                var enterStoreChannelId = Guid.Parse("{3FC61BB8-0D9F-48C7-9BBD-D739DCBBE032}");
                var userAgent = "xConnect MVC app";
                var interaction = new Interaction(contact, InteractionInitiator.Contact, enterStoreChannelId, userAgent);

                var productPurchaseOutcomeId = Guid.Parse("{9016E456-95CB-42E9-AD58-997D6D77AE83}");
                var outcome = new Outcome(productPurchaseOutcomeId, DateTime.UtcNow, "USD", 42.90m);

                interaction.Events.Add(outcome);
                client.AddInteraction(interaction);
                client.Submit();
            }

        }

        public bool GetContact(XConnectClient client, ContactModel contactModel)
        {
            var contactReference = new IdentifiedContactReference("twitter", contactModel.FirstName + contactModel.LastName);
            var contact = client.Get(contactReference,new ExpandOptions() { FacetKeys = { "Personal" } });
            if (contact != null)
                return true;

            return false;
        }

        // needs to be used:
        public async void  SearchContacts(XConnectClient client)
        {
  
            var queryable = client.Contacts
                .Where(c => c.Interactions.Any(x => x.StartDateTime > DateTime.UtcNow.AddDays(-30)))
                .WithExpandOptions(new ContactExpandOptions("Personal"));
            var results = await queryable.ToSearchResults();
            var contacts = await  results.Results.Select(x => x.Item).ToList();                       
            
        }
    }
}