using System;
using System.IO;

namespace ReplaceNoTranslation
{
    public static class Setting
    {
        public static readonly string DEFAULT_FIND_NAME = "NoTranslation";
        private static readonly string FILE_NAME = "AppSetting.txt";

        private static ConfigData configData;

        public static void InitConfig()
        {
            Logger.Log(LogEnum.StartReadConfig);

            ReadSetting();
        }

        private static void ReadSetting()
        {
            string path = (AppDomain.CurrentDomain.BaseDirectory + FILE_NAME).Replace("\\", "/");
            if (File.Exists(path))
            {
                string[] content = File.ReadAllLines(path);
                configData = new ConfigData();
                for (int i = 0; i < content.Length; i++)
                {
                    if (content[i].StartsWith("//") || string.IsNullOrEmpty(content[i]))
                    {
                        continue;
                    }
                    string[] setStrs = content[i].Split(".");
                    configData.Set(int.Parse(setStrs[0]), setStrs[1]);
                }
                Logger.Log(LogEnum.EndReadConfig);
            }
            else
            {
                Logger.LogError("AppSetting.txt文件不存在");
            }
        }

        public static string GetFileDirectorPath()
        {
            return configData.fileDirPath.Replace(@"\", "/");
        }

        public static FileEnum GetReplaceType()
        {
            return configData.replaceType;
        }
        
    }

    public class ConfigData
    {
        public FileEnum replaceType;
        public string fileDirPath;

        public void Set(int index, string content)
        {
            try
            {
                string[] param = content.Split("=");
                param[1] = param[1].Trim();
                if (index == 1)
                {
                    replaceType = (FileEnum)Enum.Parse(typeof(FileEnum), param[1].ToLower());
                }
                else if (index == 2)
                {
                    fileDirPath = param[1];
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

    }

    public enum FileEnum
    {
        txt,
        excel,
        csv
    }

}
