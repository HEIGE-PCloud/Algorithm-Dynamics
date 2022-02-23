using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Tag
    {
        private Tag(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; }
        public string Name { get; set; }

        /// <summary>
        /// Create a new <see cref="Tag"/> with a name, and an auto-generated <see cref="Id"/> and save to the Database.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Tag Create(string name)
        {
            // TODO: save to the database and retrieve the id somehow.
            var id = 1;
            return new Tag(id, name);
        }
        public static Tag Load(int id)
        {
            // TODO: retrieve the tag name somehow
            var name = "Tag name";
            return new Tag(id, name);
        }
    }
}
