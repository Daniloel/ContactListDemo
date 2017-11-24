using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ContactListDemo.Service;
using ContactListDemo.Droid.Service;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Provider;
using Android.Database;

[assembly: Xamarin.Forms.Dependency(typeof(ContactService))]
namespace ContactListDemo.Droid.Service
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

            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            var contacts = new List<Contact>();
            var ctx = Forms.Context;
            string[] projection = { ContactsContract.Contacts.InterfaceConsts.Id,
                                    ContactsContract.Contacts.InterfaceConsts.DisplayName,
                                    ContactsContract.CommonDataKinds.Phone.Number,
                                    ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber
            };


            var loader = new CursorLoader(ctx, uri, projection, null, null, null);
            using (var cursor = (ctx.ApplicationContext.ContentResolver.Query(uri, projection, null, null, null)))
            {
                if (cursor != null)
                {
                    if (cursor.MoveToFirst())
                    {
                        do
                        {
                            if (cursor.GetString(cursor.GetColumnIndex(projection[3])) == "1")
                            {
                                contacts.Add(new Contact
                                {
                                    Contact_Id = cursor.GetString(
                                        cursor.GetColumnIndex(projection[0])),

                                    Contact_DisplayName = cursor.GetString(
                                    cursor.GetColumnIndex(projection[1])),

                                    Contact_Number = cursor.GetString(
                                    cursor.GetColumnIndex(projection[2]))
                                    
                                });
                            }
                        } while (cursor.MoveToNext());
                    }
                }
            }
            _contacts = (from c in contacts orderby c.Contact_DisplayName select c).ToList();

            return _contacts;
        }

        }
    }