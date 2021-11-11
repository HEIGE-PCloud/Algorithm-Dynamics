namespace Algorithm_Dynamics.Core.Models
{
    public class Language
    {
        public string Name { get; set; }
        public bool NeedCompile { get; set; }
        public string CompileCommand { get; set; }
        public string CompileArguments { get; set; }
        public string RunCommand { get; set; }
        public string RunArguments { get; set; }
        public Language(string name, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments)
        {
            Name = name;
            NeedCompile = needCompile;
            CompileCommand = compileCommand;
            CompileArguments = compileArguments;
            RunCommand = runCommand;
            RunArguments = runArguments;
        }
        public Language(string name, string runCommand, string runArguments)
        {
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
            "{SourceCodeFilePath}"
        );
        public readonly static Language Cpp = new(
            "C++",
            true,
            "g++",
            "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
    }
}