using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class User
    {
        private Guid _uid;
        private string _name;
        private string _email;
        private Role _role;

        public Guid Uid { get => _uid; private set => _uid = value; }
        public string Name 
        { 
            get => _name;
            set 
            {
                if (value != _name)
                {
                    _name = value;
                    DataAccess.EditUser(this, value, _email, _role);
                }
            }
        }
        public string Email 
        { 
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    DataAccess.EditUser(this, _name, value, _role);
                }
            }
        }
        public Role Role 
        {
            get => _role;
            set
            {
                if (value != _role)
                {
                    _role = value;
                    DataAccess.EditUser(this, _name, _email, value);
                }
            }
        }
        internal User(Guid uid, string name, string email, Role role)
        {
            _uid = uid;
            _name = name;
            _email = email;
            _role = role;
        }
        /// <summary>
        /// Create a new <see cref="User"/> and save it into the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static User Create(string name, string email, Role role)
        {
            User user = new(Guid.NewGuid(), name, email, role);
            DataAccess.AddUser(user);
            return user;
        }
        public static User Get(Guid Uid)
        {
            return DataAccess.GetUser(Uid);
        }
        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (user == null)
                return false;

            return user.Uid == Uid && user.Name == Name && user.Email == Email && user.Role == Role;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Uid, Name, Email, Role);
        }
    }
    public enum Role
    {
        Student,
        Teacher
    }
}
