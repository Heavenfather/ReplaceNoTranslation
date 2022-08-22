using System;

namespace ReplaceNoTranslation
{
    class Program
    {
        static void Main(string[] args)
        {
            Setting.InitConfig();

            ReplaceMgr.GetInstance().Replace();

            Logger.Log(LogEnum.PressAnyKey);
            Console.ReadKey();
        }
    }
}
