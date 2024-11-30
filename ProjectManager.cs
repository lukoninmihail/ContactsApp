using System.IO;
using System.Text.Json;

namespace ContactsApp.Models
{
    public class ProjectManager
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ContactsApp.json");

        public static Project Load()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<Project>(json) ?? new Project();
            }
            return new Project();
        }

        public static void Save(Project project)
        {
            var json = JsonSerializer.Serialize(project);
            File.WriteAllText(FilePath, json);
        }
    }
}
