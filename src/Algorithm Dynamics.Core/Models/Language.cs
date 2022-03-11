using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Models
{
    public class Language
    {
        private int _id;
        private string _name;
        private string _displayName;
        private bool _needCompile;
        private string _compileCommand;
        private string _compileArguments;
        private string _runCommand;
        private string _runArguments;
        private string _fileExtension;
        public int Id { get => _id; private set => _id = value; }
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

        public void Delete()
        {
            // Delete all submissions related to this lang
            Submission.All.FindAll(submission => submission.Language.Id == Id).ForEach(submission => submission.Delete());
            // Then self destory
            DataAccess.DeleteLanguage(Id);
        }
        private void UpdateDatabase()
        {
            DataAccess.EditLanguage(_id, _name, _displayName, _needCompile, _compileCommand, _compileArguments, _runCommand, _runArguments, _fileExtension);
        }

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
    public static class LanguageConfig
    {
        public readonly static Language Python = new(
            "Python",
            "python",
            ".py",
            "python",
            "{SourceCodeFilePath}"
        );
        public readonly static Language C = new(
            "C",
            "c",
            ".c",
            true,
            "gcc",
            "-x c {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language Cpp = new(
            "C++",
            "cpp",
            ".cpp",
            true,
            "g++",
            "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language Rust = new(
            "Rust",
            "rust",
            ".rs",
            true,
            "rustc",
            "{SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language JavaScript = new(
            "JavaScript",
            "javascript",
            ".js",
            "node",
            "{SourceCodeFilePath}"
        );
        public readonly static Language Java = new(
            "Java",
            "java",
            ".java",
            "java",
            "{SourceCodeFilePath}"
        );
        public readonly static Language Go = new(
            "Go",
            "go",
            ".go",
            "go",
            "run {SourceCodeFilePath}"
        );
    }
}