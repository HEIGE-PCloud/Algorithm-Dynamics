using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Models
{
    public class Language
    {
        internal Language(string displayName, string name, string fileExtension, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments)
        {
            _displayName = displayName;
            _fileExtension = fileExtension;
            _name = name;
            _needCompile = needCompile;
            _compileCommand = compileCommand;
            _compileArguments = compileArguments;
            _runCommand = runCommand;
            _runArguments = runArguments;
        }

        internal Language(string displayName, string name, string fileExtension, string runCommand, string runArguments)
        {
            _displayName = displayName;
            _fileExtension = fileExtension;
            _name = name;
            _needCompile = false;
            _runCommand = runCommand;
            _runArguments = runArguments;
            _compileCommand = "";
            _compileArguments = "";
        }

        internal Language(int id, string name, string displayName, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments, string fileExtension)
        {
            _id = id;
            _name = name;
            _displayName = displayName;
            _needCompile = needCompile;
            _compileCommand = compileCommand;
            _compileArguments = compileArguments;
            _runCommand = runCommand;
            _runArguments = runArguments;
            _fileExtension = fileExtension;
        }

        private int _id;
        public int Id { get => _id; private set => _id = value; }

        private string _name;
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

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (value != _displayName)
                {
                    _displayName = value;
                    UpdateDatabase();
                }
            }
        }

        private bool _needCompile;
        public bool NeedCompile
        {
            get => _needCompile;
            set
            {
                if (value != _needCompile)
                {
                    _needCompile = value;
                    UpdateDatabase();
                }
            }
        }

        private string _compileCommand;
        public string CompileCommand
        {
            get => _compileCommand;
            set
            {
                if (value != _compileCommand)
                {
                    _compileCommand = value;
                    UpdateDatabase();
                }
            }
        }

        private string _compileArguments;
        public string CompileArguments
        {
            get => _compileArguments;
            set
            {
                if (value != _compileArguments)
                {
                    _compileArguments = value;
                    UpdateDatabase();
                }
            }
        }

        private string _runCommand;
        public string RunCommand
        {
            get => _runCommand;
            set
            {
                if (value != _runCommand)
                {
                    _runCommand = value;
                    UpdateDatabase();
                }
            }
        }

        private string _runArguments;
        public string RunArguments
        {
            get => _runArguments;
            set
            {
                if (value != _runArguments)
                {
                    _runArguments = value;
                    UpdateDatabase();
                }
            }
        }

        private string _fileExtension;
        public string FileExtension
        {
            get => _fileExtension;
            set
            {
                if (value != _fileExtension)
                {
                    _fileExtension = value;
                    UpdateDatabase();
                }
            }
        }

        public static List<Language> All
        {
            get => DataAccess.GetAllLanguages();
        }

        /// <summary>
        /// Delete all submissions and then self destory in the database
        /// </summary>
        public void Delete()
        {
            // Delete all submissions related to this lang
            Submission.All.FindAll(submission => submission.Language.Id == Id).ForEach(submission => submission.Delete());
            // Then self destory
            DataAccess.DeleteLanguage(Id);
        }

        /// <summary>
        /// Update the current language in the database
        /// </summary>
        private void UpdateDatabase()
        {
            DataAccess.EditLanguage(_id, _name, _displayName, _needCompile, _compileCommand, _compileArguments, _runCommand, _runArguments, _fileExtension);
        }

        /// <summary>
        /// Create a new language config and save it to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="needCompile"></param>
        /// <param name="compileCommand"></param>
        /// <param name="compileArguments"></param>
        /// <param name="runCommand"></param>
        /// <param name="runArguments"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static Language Create(string name, string displayName, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments, string fileExtension)
        {
            return DataAccess.AddLanguage(name, displayName, needCompile, compileCommand, compileArguments, runCommand, runArguments, fileExtension);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Language lang)
                return false;
            return Id == lang.Id && Name == lang.Name && DisplayName == lang.DisplayName && NeedCompile == lang.NeedCompile && CompileCommand == lang.CompileCommand && CompileArguments == lang.CompileArguments && RunCommand == lang.RunCommand && RunArguments == lang.RunArguments && FileExtension == lang.FileExtension;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DisplayName, NeedCompile, CompileCommand, CompileArguments, RunCommand, RunArguments, FileExtension);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}