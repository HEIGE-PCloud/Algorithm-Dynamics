using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class User
    {
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        private User(Guid uid, string name, string email, Role role)
        {
            Uid = uid;
            Name = name;
            Email = email;
            Role = role;
        }
        public static User Create(string name, string email, Role role)
        {
            return new User(Guid.NewGuid(), name, email, role);
        }
        public static User Load(Guid uid, string name, string email, Role role)
        {
            return new User(uid, name, email, role);
        }
        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (user == null)
                return false;

            return user.Uid == Uid && user.Name == Name && user.Email == Email && user.Role == Role;
        }
    }
    public enum Role
    {
        Student,
        Teacher
    }
}
