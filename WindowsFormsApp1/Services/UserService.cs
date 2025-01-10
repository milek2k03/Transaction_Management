using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LiteDB;

namespace InvoiceApp.Services
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class UserService
    {
        private const string DatabasePath = "InvoiceApp.db";
        private User currentUser;

        public bool Register(string email, string password)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var users = db.GetCollection<User>("users");
                if (users.FindOne(u => u.Email == email) != null)
                {
                    return false;
                }

                var user = new User
                {
                    Email = email,
                    PasswordHash = HashPassword(password)
                };

                users.Insert(user);
                return true;
            }
        }

        public bool Login(string email, string password)
        {
            using (var db = new LiteDatabase(DatabasePath))
            {
                var users = db.GetCollection<User>("users");
                var user = users.FindOne(u => u.Email == email);
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                    currentUser = user;
                    return true;
                }
                return false;
            }
        }

        public User GetCurrentUser() => currentUser;

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
