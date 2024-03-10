using Newtonsoft.Json;
using CrystalSharp;
 
 namespace TtySessionMan
 {
    public class ConfigMan
    {
        static string configPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config/sessions.json";
            public static dynamic ReadConfig()
            {
                string jsonText = File.ReadAllText(configPath);

                dynamic config = JsonConvert.DeserializeObject(jsonText);
                
                return config;
            }

            public static SessionConfig LoadConfig()
            {
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<SessionConfig>(json);
            }
            return null;
        }

        public static void SaveConfig(SessionConfig config)
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }

        public static bool ConfigExists()
        {
            if(File.Exists(configPath))
            {
                return true;
            }
            return false;
        }
    }
    public class SessionConfig
    {
        public List<Session> Sessions { get; set; }
    }

    public class Session
    {
        public string Name { get; set; }
        public string Executable { get; set; }
    }
}