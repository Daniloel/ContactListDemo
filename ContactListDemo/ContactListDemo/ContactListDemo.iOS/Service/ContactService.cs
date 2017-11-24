using System.Collections.Generic;
using System.Linq;
using Contacts;
using Foundation;
using ContactListDemo.Service;
using ContactListDemo.iOS.Service;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(ContactService))]
namespace ContactListDemo.iOS.Service
{
    public class ContactService : IContactService
    {
        private static IEnumerable<Contact> _contacts;
        public ContactService()
        {
        }

        public List<Contact> FindContacts(string searchInContactsString)
        {
            var ResultContacts = new List<Contact>();

            foreach (var currentContact in _contacts)
            {
                if ((currentContact.Contact_DisplayName != null && currentContact.Contact_DisplayName.ToLower().Contains(searchInContactsString.ToLower())) ||
                    (currentContact.Contact_Number != null && currentContact.Contact_Number.ToLower().Contains(searchInContactsString.ToLower())))
                {
                    ResultContacts.Add(currentContact);
                }
            }

            return ResultContacts;
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
            if (_contacts != null) return _contacts;

            var contacts = new List<Contact>();

            var keysTOFetch = new[] { CNContactKey.GivenName, CNContactKey.PhoneNumbers };
            NSError error;
            CNContact[] contactList;
            var ContainerId = new CNContactStore().DefaultContainerIdentifier;
            using (var predicate = CNContact.GetPredicateForContactsInContainer(ContainerId))

            using (var store = new CNContactStore())
            {
                contactList = store.GetUnifiedContacts(predicate, keysTOFetch, out error);
            }

            foreach (var item in contactList)
            {
                if (null != item && null != item.PhoneNumbers)
                {
                    contacts.Add(new Contact
                    {
                        Contact_DisplayName = item.GivenName,
                        Contact_Number = item.PhoneNumbers.FirstOrDefault().ToString()
                    });//TODO prove it in iOS, please
                }
            }
            
            _contacts = (from c in contacts orderby c.Contact_DisplayName select c).ToList();
            
            return _contacts;
        }
    }
}