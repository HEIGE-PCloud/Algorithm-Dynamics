using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Algorithm_Dynamics.Core.Models
{
    public class Tag
    {
        internal Tag(int id, string name)
        {
            Id = id;
            Name = name;
        }
        [JsonIgnore]
        public int Id { get; }
        public string Name { get; set; }
        public static List<Tag> All { get => DataAccess.GetAllTags(); }

        /// <summary>
        /// Attach the Tag to a problem
        /// </summary>
        /// <param name="id"></param>
        public void AttachTo(int problemId)
        {
            DataAccess.AddTagRecord(problemId, Id);
        }

        public void DeleteRecord(int problemId)
        {
            DataAccess.DeleteTagRecord(problemId, Id);
        }

        public void Delete()
        {
            DataAccess.DeleteTag(Id);
        }
        /// <summary>
        /// Create a new <see cref="Tag"/> with a name, and an auto-generated <see cref="Id"/> and save to the Database.
        /// The <see cref="Name"/> is unique, so if a tag exists in the database, it will be returned directly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Tag Create(string name)
        {
            if (DataAccess.TagExists(name))
            {
                return DataAccess.GetTag(name);
            }
            else
            {
                return DataAccess.AddTag(name);
            }
        }

        public override bool Equals(object obj)
        {
            var tag = obj as Tag;

            if (tag == null)
                return false;
            return Id == tag.Id && Name == tag.Name;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
        public override string ToString()
        {
            return Name;
        }

        [JsonConstructor]
        public Tag(string Name)
        {
            Create(Name);
        }
    }
}
