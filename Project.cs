using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactsApp.Models
{
    public class Project
    {
        public List<Contact> Contacts { get; private set; } = new List<Contact>();

        public List<Contact> GetContactsBySubstring(string substring)
        {
            return Contacts
                .Where(c => c.Surname.Contains(substring, StringComparison.OrdinalIgnoreCase) ||
                            c.Name.Contains(substring, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Surname)
                .ToList();
        }

        public List<Contact> GetSortedContacts()
        {
            return Contacts.OrderBy(c => c.Surname)
                          .ThenBy(c => c.Name)
                          .ToList();
        }

        public List<Contact> GetBirthdays(DateTime date)
        {
            return Contacts
                .Where(c => c.BirthDate.Day == date.Day && c.BirthDate.Month == date.Month)
                .ToList();
        }
    }
}
