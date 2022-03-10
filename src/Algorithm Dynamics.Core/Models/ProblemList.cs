using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Algorithm_Dynamics.Core.Models
{
    public class ProblemList
    {
        internal ProblemList(int id, string name, string description, List<Problem> problems)
        {
            _id = id;
            _name = name;
            _description = description;
            _problems = problems;
        }
        private void UpdateDatabase()
        {
            DataAccess.EditProblemList(_id, _name, _description);
        }
        private int _id;
        private string _name;
        private string _description;
        [JsonIgnore]
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
        public static List<ProblemList> All
        {
            get => DataAccess.GetAllProblemLists();
        }
        private List<Problem> _problems;
        public ReadOnlyCollection<Problem> Problems { get => _problems.AsReadOnly(); }

        public override bool Equals(object obj)
        {
            ProblemList problemList = obj as ProblemList;
            if (problemList == null)
                return false;
            if (problemList.Id != Id || problemList.Description != Description || problemList.Name != Name) 
                return false;
            if (problemList.Problems.Count != Problems.Count) 
                return false;
            for (int i = 0; i < _problems.Count; i++)
            {
                if (Equals(Problems[i], problemList.Problems[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description);
        }

        public static ProblemList Create(string name, string description, List<Problem> problems)
        {
            var problemList = DataAccess.AddProblemList(name, description, problems);
            if (problems != null)
            {
                foreach(var problem in problems)
                {
                    problem.AttachTo(problemList.Id);
                }
            }
            else
            {
                problemList._problems = new() { };
            }

            return problemList;
        }
        public void AddProblem(Problem problem)
        {
            DataAccess.AddProblemListRecord(Id, problem.Id);
            _problems.Add(problem);
        }

        public void RemoveProblem(Problem problem)
        {
            DataAccess.DeleteProblemListRecord(Id, problem.Id);
            _problems.Remove(problem);
        }

        public void Delete()
        {
            while (_problems.Count != 0) RemoveProblem(_problems[0]);
            DataAccess.DeleteProblemList(_id);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
