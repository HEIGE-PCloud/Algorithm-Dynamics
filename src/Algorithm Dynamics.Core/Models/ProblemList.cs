using Algorithm_Dynamics.Core.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Core.Models
{
    public class ProblemList
    {
        internal ProblemList(int id, string name, string description, List<Problem> problems)
        {
            Id = id;
            Name = name;
            Description = description;
            _problems = problems;
        }
        private void UpdateDatabase()
        {
            DataAccess.EditProblemList(_name, _description);
        }
        private int _id;
        private string _name;
        private string _description;
        public int Id 
        { 
            get => _id;
            private set => _id = value;
        }
        public string Name 
        { 
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    UpdateDatabase();
                }
            }
        }
        public string Description 
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    UpdateDatabase();
                }
            }
        }
        private List<Problem> _problems;
        public ReadOnlyCollection<Problem> Problems { get => _problems.AsReadOnly(); }
    }
}
