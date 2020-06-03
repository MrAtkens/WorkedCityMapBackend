using AuthJWT.Models;
using System.Collections.Generic;
using System.Linq;

namespace AuthJWT.Helpers
{
    public static class UserAuthHelpers
    {
        public static IEnumerable<User> UsersWithoutPasswords(this IEnumerable<User> users)
        {
            if (users == null) return null;

            return users.Select(x => x.UserWithoutPassword());
        }

        public static User UserWithoutPassword(this User user)
        {
            if (user == null) return null;

            user.Password = null;
            return user;
        }
    }
}
