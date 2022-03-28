namespace Algorithm_Dynamics.Core.Models
{
    /// <summary>
    /// A helper class with some default language configs
    /// </summary>
    public static class LanguageConfig
    {
        public readonly static Language Python = new(
            "Python",
            "python",
            ".py",
            "python.exe",
            "{SourceCodeFilePath}"
        );
        public readonly static Language C = new(
            "C",
            "c",
            ".c",
            true,
            "gcc.exe",
            "-x c {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language Cpp = new(
            "C++",
            "cpp",
            ".cpp",
            true,
            "g++.exe",
            "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language Rust = new(
            "Rust",
            "rust",
            ".rs",
            true,
            "rustc.exe",
            "{SourceCodeFilePath} -o {ExecutableFilePath}",
            "{ExecutableFilePath}",
            ""
        );
        public readonly static Language JavaScript = new(
            "JavaScript",
            "javascript",
            ".js",
            "node.exe",
            "{SourceCodeFilePath}"
        );
        public readonly static Language Java = new(
            "Java",
            "java",
            ".java",
            "java.exe",
            "{SourceCodeFilePath}"
        );
        public readonly static Language Go = new(
            "Go",
            "go",
            ".go",
            "go.exe",
            "run {SourceCodeFilePath}"
        );
    }
}