namespace ContactsApp.Models
{
    public class PhoneNumber
    {
        public string Number { get; set; } = string.Empty;

        public bool IsValid()
        {
            return Number.StartsWith("7") && Number.Length == 11 && long.TryParse(Number, out _);
        }
    }
}
