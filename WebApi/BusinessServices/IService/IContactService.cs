using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    /// <summary>
    /// Goods Service Contract
    /// </summary>
    public interface IContactService
    {
        List<ContactResponseEntity> GetAllContacts(List<int> roadlines);
        List<ContactResponseEntity> GetAllContacts();

        ContactResponseEntity GetContact(int contactId);
        long CreateContact(ContactRequestEntity contactEntity);
        bool UpdateContact(int contactId, ContactRequestEntity contactEntity);
        bool DeleteContact(int contactId);
    }
}
