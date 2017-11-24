﻿using System;
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
using Xamarin.Contacts;
using Android.Provider;
using Android.Database;

[assembly: Xamarin.Forms.Dependency(typeof(ContactService))]
namespace ContactListDemo.Droid.Service
{
    public class ContactService : IContactService
    {
        private readonly Xamarin.Contacts.AddressBook _book;
        private static IEnumerable<Contact> _contacts;
        public ContactService()
        {
            _book = new Xamarin.Contacts.AddressBook(Forms.Context.ApplicationContext);
        }

        public List<Contact> FindContacts(string searchInContactsString)
        {

            var ResultContacts = new List<Contact>();

            foreach (var currentContact in _contacts)
            {
                if ((currentContact.Contact_FirstName != null && currentContact.Contact_FirstName.ToLower().Contains(searchInContactsString.ToLower())) ||
                    (currentContact.Contact_LastName != null && currentContact.Contact_LastName.ToLower().Contains(searchInContactsString.ToLower())) ||
                    (currentContact.Contact_EmailId != null && currentContact.Contact_EmailId.ToLower().Contains(searchInContactsString.ToLower())))
                {
                    ResultContacts.Add(currentContact);
                }
            }

            return ResultContacts;
        }

        public async Task<IEnumerable<Contact>> GetAllContacts()
        {
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

            
           

            
            _contacts = (from c in contacts orderby c.Contact_FirstName select c).ToList();

            return _contacts;

            /*if (_contacts != null) return _contacts;


            var contacts = new List<Contact>();
            await _book.RequestPermission().ContinueWith(t =>
            {
                if (!t.Result)
                {
                    Console.WriteLine("Sorry ! Permission was denied by user or manifest !");
                    return;
                }
                foreach (var contact in _book.ToList())
                {
                    Xamarin.Contacts.Phone number = new Phone();
                    //var firstOrDefault = contact.Emails.FirstOrDefault();
                    if (contact.Phones.FirstOrDefault() == null)
                    {
                        continue;
                    }

                    number = contact.Phones.FirstOrDefault();
                    contacts.Add(new Contact()
                    {
                        Contact_FirstName = contact.FirstName + "====>" + number.Number,
                        Contact_LastName = contact.LastName,
                        //Contact_DisplayName = contact.DisplayName,
                        //Contact_EmailId = firstOrDefault != null ? firstOrDefault.Address : String.Empty,
                        //Contact_Number = number != null ? number.Number : String.Empty
                    });
                }
            });

            _contacts = (from c in contacts orderby c.Contact_FirstName select c).ToList();

            return _contacts;*/
        }

        }
    }