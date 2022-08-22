using System;

namespace ReplaceNoTranslation
{
    public static class Logger
    {
        public static void Log(LogEnum logEnum)
        {
            Console.WriteLine(GetContent(logEnum));
        }

        public static void Log(string content)
        {
            Console.WriteLine(content);
        }

        public static void LogError(object message)
        {
            Console.WriteLine(message);
            Log(LogEnum.ErrorCatch);
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void LogFormat(LogEnum logEnum, params object[] args)
        {
            string content = GetContent(logEnum);
            Console.WriteLine(string.Format(content, args));
        }

        private static string GetContent(LogEnum logEnum)
        {
            string content = "";
            switch (logEnum)
            {
                case LogEnum.StartReadConfig:
                    content = "开始读取配置...";
                    break;
                case LogEnum.EndReadConfig:
                    content = "配置读取完成!";
                    break;
                case LogEnum.ErrorCatch:
                    content = "\n程序已出错!!!按任意键退出!";
                    break;
                case LogEnum.PressAnyKey:
                    content = "\n按任意键退出";
                    break;
                case LogEnum.HandleFile:
                    content = "正在处理文件:{0}";
                    break;
                case LogEnum.NoneNoTranFile:
                    content = Setting.GetFileDirectorPath() + "不存在" + Setting.DEFAULT_FIND_NAME + "文件";
                    break;
                case LogEnum.ReplaceEnum:
                    content = "替换文件类型:{0}";
                    break;
                case LogEnum.ReadingFile:
                    content = "正在处理文件:{0}";
                    break;
            }
            return content;
        }

    }

    public enum LogEnum
    {
        ErrorCatch,
        StartReadConfig,
        EndReadConfig,
        PressAnyKey,
        HandleFile,
        NoneNoTranFile,
        ReplaceEnum,
        ReadingFile,
    }
}
