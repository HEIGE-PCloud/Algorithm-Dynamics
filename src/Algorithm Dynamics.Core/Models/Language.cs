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
        public Language(string displayName, string name, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments)
        {
            DisplayName = displayName;
            Name = name;
            NeedCompile = needCompile;
            CompileCommand = compileCommand;
            CompileArguments = compileArguments;
            RunCommand = runCommand;
            RunArguments = runArguments;
        }
        public Language(string displayName, string name, string runCommand, string runArguments)
        {
            DisplayName = displayName;
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
            "python",
            "{SourceCodeFilePath}"
        );
        public readonly static Language C = new(
            "C",
            "c",
            true,
            "gcc",
            "-x c {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language Cpp = new(
            "C++",
            "cpp",
            true,
            "g++",
            "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language Rust = new(
            "Rust",
            "rust",
            true,
            "rustc",
            "{SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language JavaScript = new(
            "JavaScript",
            "javascript",
            "node",
            "{SourceCodeFilePath}"
        );
        public readonly static Language Java = new(
            "Java",
            "java",
            "java",
            "{SourceCodeFilePath}"
        );
    }
}