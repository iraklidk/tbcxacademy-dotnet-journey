using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserManagement.Domain.Entity;

namespace UserManagement.Infrastructure
{
    public class FileStorage
    {
        private static string filePath = Path.Combine(AppContext.BaseDirectory, "users.json");

        public static List<User> ReadUsers(ILogger logger)
        {
            try
            {
                if (!File.Exists(filePath)) return new List<User>();
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error reading users from file.");
                throw;
            }
        }

        public static void WriteUsers(List<User> users, ILogger logger)
        {
            try
            {
                var json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error writing users to file.");
                throw;
            }
        }
    }
}
