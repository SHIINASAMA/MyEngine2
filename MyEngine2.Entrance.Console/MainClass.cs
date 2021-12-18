using MyEngine2.Common.Service;
using static System.Console;

namespace MyEngine2.Entrance.Console
{
    public class MainClass
    {
        private static ServiceProfile? Profile;

        public static void Main(string[] argv)
        {
            LoadProfile();
            ServiceMain service = new(Profile ?? new());
            service.StartAccpet();
            ReadLine();
            service.StopAccept();
        }

        private static void LoadProfile()
        {
            // 临时配置文件
            Profile = new();
        }
    }
}