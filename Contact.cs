using System;

namespace ContactsApp.Models
{
    public class Contact : ICloneable
    {
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public PhoneNumber PhoneNumber { get; set; } = new PhoneNumber();
        public DateTime BirthDate { get; set; } = DateTime.Now; 
        public string Email { get; set; } = string.Empty;
        public string VKId { get; set; } = string.Empty;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Surname} {Name}";
        }
    }
}
