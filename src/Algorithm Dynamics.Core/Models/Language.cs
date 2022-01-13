namespace Algorithm_Dynamics.Core.Models
{
    public class Language
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public bool NeedCompile { get; set; }
        public string CompileCommand { get; set; }
        public string CompileArguments { get; set; }
        public string RunCommand { get; set; }
        public string RunArguments { get; set; }
        public string FileExtension { get; set; }
        public Language(string displayName, string name, string fileExtension, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments)
        {
            DisplayName = displayName;
            FileExtension = fileExtension;
            Name = name;
            NeedCompile = needCompile;
            CompileCommand = compileCommand;
            CompileArguments = compileArguments;
            RunCommand = runCommand;
            RunArguments = runArguments;
        }
        public Language(string displayName, string name, string fileExtension, string runCommand, string runArguments)
        {
            DisplayName = displayName;
            FileExtension = fileExtension;
            Name = name;
            NeedCompile = false;
            RunCommand = runCommand;
            RunArguments = runArguments;
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