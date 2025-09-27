using BCrypt.Net;
using ChatTool.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTool.Database;

public static class DbSeeder
{
    public static void Seed(DBContext db)
    {
        if (!db.Users.Any())
        {
            db.Users.AddRange([
                new User
                {
                    Username = "Brandon",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                },
                new User
                {
                    Username = "Jared",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                },
                new User
                {
                    Username = "Emily",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                },
                new User
                {
                    Username = "Shannon",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                },
                new User
                {
                    Username = "Ian",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
                },
            ]);
            db.SaveChanges();
        }
    }
}
