using ContactListDemo.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContactListDemo.ViewModel
{
    public class ContactViewModel : BaseModel
    {
        #region fields
        private bool _isBusy;

        IContactService _contactService = null;
        #endregion

        #region Properties
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged("IsBusy"); }
        }

        private string _searchContact;

        public string SearchContact
        {
            get { return _searchContact; }
            set
            {
                if (_searchContact != value)
                {
                    _searchContact = value;
                    OnPropertyChanged("SearchContact");
                    GetContactsFilter();
                }
            }
        }
        private ObservableCollection<Contact> contactList;
        public ObservableCollection<Contact> ContactList
        {
            get
            {
                return contactList;
            }
            set
            {
                contactList = value; OnPropertyChanged("ContactList");
            }
        }
        #endregion

        #region Contructor
        public ContactViewModel()
        {
        }
        #endregion

        #region Command
        private Command searchCommand;
        public Command SearchCommand
        {
            get
            {
                return searchCommand ?? new Command(async (obj) =>
                {
                    IsBusy = true;
                    await GetContactsFilter();
                    IsBusy = false;
                });
            }
        }
        private Command fetchContactCommand;
        public Command FetchContactCommand
        {
            get
            {
                return fetchContactCommand ?? new Command(async(obj) =>
                {
                    await GetContacts();
                });
            }
        }
        #endregion

        #region methods
        public async Task GetContactsFilter()
        {
            try
            {
                _contactService = DependencyService.Get<IContactService>();
                IEnumerable<Contact> contacts = await _contactService.FindContacts(SearchContact);
                var _contactList = new ObservableCollection<Contact>(contacts);
                ContactList = _contactList;
            }
            catch (System.Exception ex)
            {
            }
        }
        public async Task GetContacts()
        {
            try
            {
                _contactService = DependencyService.Get<IContactService>();
                IEnumerable<Contact> contacts = await _contactService.GetAllContacts();
                var _contactList = new ObservableCollection<Contact>(contacts);
                ContactList = _contactList;
            }
            catch (System.Exception ex)
            {
            }
        }
        #endregion
    }
}
