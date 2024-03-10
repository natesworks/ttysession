using System.Diagnostics;
using System.Text;
using CrystalSharp;

namespace TtySessionMan
{
    public class TtySessionMan
    {
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "config" || !ConfigMan.ConfigExists())
        {
            EditConfig();
        }
        else
        {
            ConsoleColor bg = Console.BackgroundColor;
            ConsoleColor fg = Console.ForegroundColor;

            Menu sessionSelector = new();
            sessionSelector.SetPrompt("Select session:\n");
            sessionSelector.Colors(bg,fg);
            sessionSelector.Colors(fg,bg);

            dynamic config = ConfigMan.ReadConfig();
            foreach (var session in config.Sessions)
            {
                string name = session.Name;
                string executable = session.Executable;
                sessionSelector.AddOption(name, () => Exec(executable));
            }
            sessionSelector.AddOption("Back to terminal", () => Environment.Exit(0));
            sessionSelector.Run();
        }
    }

        public static void Exec(string command)
        {
            string[] parts = command.Split(' ', 2);
            string fileName = parts[0];
            string arguments = parts.Length > 1 ? parts[1] : "";
            if(File.Exists(fileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    string error = GetStandardError(process);
                    Console.WriteLine(error);
                }
            }
            Console.WriteLine("Error: File doesn't exist!");
        }

        public static void EditConfig()
        {
            ConsoleColor bg = Console.BackgroundColor;
            ConsoleColor fg = Console.ForegroundColor;

            Menu editorMenu = new();
            editorMenu.SetPrompt("Edit config:\n");
            editorMenu.Colors(bg,fg);
            editorMenu.Colors(fg,bg);

            editorMenu.AddOption("Add session", () => AddSession());
                        editorMenu.AddOption("Remove Session", () => RemoveSession());  
            editorMenu.AddOption("Exit", () => Environment.Exit(0));
            editorMenu.Run();
        }

        public static void AddSession()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Executable: ");
            string executable = Console.ReadLine();

            SessionConfig config = ConfigMan.LoadConfig() ?? new SessionConfig();

            config.Sessions ??= new List<Session>();
            config.Sessions.Add(new Session { Name = name, Executable = executable });

            ConfigMan.SaveConfig(config);

            EditConfig();
        }

        public static void RemoveSession()
        {
            SessionConfig config = ConfigMan.LoadConfig();
            Console.Write("Name:");
            string name = Console.ReadLine();

            if (config != null && config.Sessions != null)
            {
                config.Sessions.RemoveAll(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                ConfigMan.SaveConfig(config);
            }
            else
            {
                Console.WriteLine($"Session '{name}' not found.");
                Console.ReadLine();
            }
        }

        private static string GetStandardError(Process process)
        {
            var error = new StringBuilder();
            while (process.StandardError.Peek() > -1)
                error.Append((char)process.StandardError.Read());
            return error.ToString();
        }
    }
}