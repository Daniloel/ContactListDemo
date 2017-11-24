namespace ContactListDemo.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        List<Contact> FindContacts(string searchInContactsString);
    }
}
