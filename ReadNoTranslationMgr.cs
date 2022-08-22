using System.Collections.Generic;
using System.IO;

namespace ReplaceNoTranslation
{
    public class ReadNoTranslationMgr
    {
        private static ReadNoTranslationMgr _instance;
        public static ReadNoTranslationMgr GetInstance()
        {
            if (_instance == null)
                _instance = new ReadNoTranslationMgr();
            return _instance;
        }

        private List<string> _fileNames = new List<string>();
        private Dictionary<string, Dictionary<string, string>> _noTranslationDic = new Dictionary<string, Dictionary<string, string>>();

        private ReadNoTranslationMgr()
        {
            string path = Setting.GetFileDirectorPath() + "/" + Setting.DEFAULT_FIND_NAME;
            if (!File.Exists(path + ".txt") && !File.Exists(path + ".csv"))
            {
                Logger.LogError("不存在" + path);
                return;
            }
            path = path + (File.Exists(path + ".txt") ? ".txt" : ".csv");
            string[] allLines = File.ReadAllLines(path);
            string[] keyValue;
            for (int i = 0; i < allLines.Length; i++)
            {
                if (i == 0) continue;
                allLines[i] = allLines[i].Replace("\"", "");
                keyValue = allLines[i].Split(",");
                if (!string.IsNullOrEmpty(keyValue[0]))
                {
                    string[] fileName = keyValue[0].Split('_', 2, System.StringSplitOptions.RemoveEmptyEntries);
                    if (!_fileNames.Contains(fileName[0]))
                    {
                        _fileNames.Add(fileName[0]);
                    }
                    if (!_noTranslationDic.ContainsKey(fileName[0]))
                    {
                        _noTranslationDic.Add(fileName[0], new Dictionary<string, string>());
                    }
                    _noTranslationDic[fileName[0]].Add(fileName[1], keyValue[1]);
                }
            }
        }

        public Dictionary<string,string> GetAllTranslation(string fileName)
        {
            if (_noTranslationDic.ContainsKey(fileName))
            {
                return _noTranslationDic[fileName];
            }
            return null;
        }

        public List<string> GetAllFileName()
        {
            return _fileNames;
        }

    }
}
