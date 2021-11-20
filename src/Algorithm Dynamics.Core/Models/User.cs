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
        public User(string name, string email, Role role)
        {
            Uid = Guid.NewGuid();
            Name = name;
            Email = email;
            Role = role;
        }
    }
    public enum Role
    {
        Student,
        Teacher
    }
}
