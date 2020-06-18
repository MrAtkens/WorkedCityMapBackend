using AuthJWT.Models;
using AuthJWT.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthJWT.Helpers
{
    public static class AuthHelpers
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
            user.Role = null;
            return user;
        }

        public static List<Admin> AdminsWithoutPasswords(this List<Admin> admins)
        {
            if (admins == null) return null;

            return admins.Select(x => x.AdminWithoutPassword()).ToList();
        }

        public static Admin AdminWithoutPassword(this Admin admin)
        {
            if (admin == null) return null;

            admin.Password = null;
            admin.Role = null;
            return admin;
        }

        public static IEnumerable<Moderator> ModeratorsWithoutPasswords(this IEnumerable<Moderator> moderators)
        {
            if (moderators == null) return null;

            return moderators.Select(x => x.ModeratorWithoutPassword());
        }

        public static Moderator ModeratorWithoutPassword(this Moderator moderator)
        {
            if (moderator == null) return null;

            moderator.Password = null;
            moderator.Role = null;
            return moderator;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
