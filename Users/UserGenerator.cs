using System.Collections.Generic;

namespace Users
{
    internal class UserGenerator
    {
        private static readonly string[] Names =
        {
            // Generates the 5 users required to properly test our software
            "shambo30", "mdickson05", "iancheu052", "lebron_james", "giveus100%"
        };

        // Returns a dictionary (aka hashmap) of usernames mapped to specific users
        // Dictionary allows usernames to be passed to return a User object; better than List<User
        public static Dictionary<string, User> GenerateUsers()
        {
            Dictionary<string, User> UserMap = new Dictionary<string, User>();

            foreach (string name in Names)
            {
                User User = new User(name);
                UserMap.Add(name, User);
            }

            return UserMap;

        }
    }
}
