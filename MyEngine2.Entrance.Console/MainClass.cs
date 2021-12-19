using MyEngine2.Common.Service;
using System.Xml.Serialization;
using static System.Console;

namespace MyEngine2.Entrance.Console
{
    public static class MainClass
    {
        private static ServiceProfile? Profile;

        public static void Main(string[] argv)
        {
            LoadProfileFromFile("Config.xml");
            if (Profile == null)
            {
                Environment.Exit(-1);
            }

            ServiceMain service = new(Profile);
            service.StartAccpet();
            ReadLine();
            service.StopAccept();
        }

        private static void LoadProfileFromFile(string path)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Open);
                XmlSerializer serializer = new(typeof(ServiceProfile));
                Profile = (ServiceProfile?)serializer.Deserialize(fileStream);
                WriteLine("[OK] Configuration File");
            }
            catch (Exception e)
            {
                WriteLine(string.Format("[NO] Configuration File\n{0}", e.Message));
                Profile = null;
            }
        }
    }
}