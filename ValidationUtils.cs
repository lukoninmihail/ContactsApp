namespace ContactsApp.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Length <= 50;
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 50;
        }

        public static bool IsValidVKId(string vkId)
        {
            return vkId.Length <= 15;
        }
    }
}
