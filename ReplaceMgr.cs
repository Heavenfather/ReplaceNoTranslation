using IronXL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReplaceNoTranslation
{
    public class ReplaceMgr
    {
        private static ReplaceMgr _instance;
        public static ReplaceMgr GetInstance()
        {
            if (_instance == null)
                _instance = new ReplaceMgr();
            return _instance;
        }

        public void Replace()
        {
            FileEnum fe = Setting.GetReplaceType();
            Logger.LogFormat(LogEnum.ReplaceEnum, fe.ToString());
            switch (fe)
            {
                case FileEnum.txt:
                    HandleTxt();
                    break;
                case FileEnum.csv:
                    HandleCsv();
                    break;
                case FileEnum.excel:
                    HandleExcel();
                    break;
            }
        }

        private void HandleTxt()
        {
            FileInfo[] files = GetFileInfos(new string[1] { "*.txt"});
            for (int i = 0; i < files.Length; i++)
            {
                if (IsContainFile(files[i]))
                {
                    string[] allLines = File.ReadAllLines(files[i].FullName);
                    int index = 0;
                    string name = files[i].Name.Split(".")[0];
                    Dictionary<string, string> allTran = ReadNoTranslationMgr.GetInstance().GetAllTranslation(name);                    
                    if (allTran == null)
                        continue;
                    
                    while (allLines.Length > index)
                    {
                        string key = name + "_" + index;
                        if (allTran.ContainsKey(key))
                        {
                            allLines[index] = allTran[key];
                        }
                        index++;
                    }
                    File.Delete(files[i].FullName);
                    File.WriteAllLines(files[i].FullName, allLines, Encoding.UTF8);
                }
            }
        }

        private void HandleCsv()
        {
            FileInfo[] files = GetFileInfos(new string[1] { "*.csv" });
            for (int i = 0; i < files.Length; i++)
            {
                if (IsContainFile(files[i]))
                {
                    ReplaceCsvCell(files[i]);
                }
            }
        }

        private void ReplaceCsvCell(FileInfo file)
        {
            string[] patterns = new string[1] { "*.csv"};
            FileInfo[] files = GetFileInfos(patterns);
            for (int i = 0; i < files.Length; i++)
            {
                WorkBook book = WorkBook.LoadCSV(files[i].FullName);
                ReplaceExcelCell(files[i].FullName, book);
            }
        }

        private void HandleExcel()
        {
            string[] patterns = new string[2] { "*.xlsx", "*.xls" };
            FileInfo[] files = GetFileInfos(patterns);
            for (int i = 0; i < files.Length; i++)
            {
                WorkBook book = WorkBook.LoadExcel(files[i].FullName);
                ReplaceExcelCell(files[i].FullName, book);
            }
        }

        private void ReplaceExcelCell(string path, WorkBook book)
        {
            WorkSheet sheet = book.DefaultWorkSheet;
            if (IsContainName(sheet.Name))
            {
                Logger.LogFormat(LogEnum.ReadingFile, sheet.Name);
                Dictionary<string, string> allTran = ReadNoTranslationMgr.GetInstance().GetAllTranslation(sheet.Name);
                foreach (var item in allTran)
                {
                    string[] keys = item.Key.Split("_");
                    int row = int.Parse(keys[0]);
                    int col = int.Parse(keys[1]);
                    sheet.SetCellValue(row, col, item.Value);
                }
                book.Save();
            }
            book.Close();
        }

        private FileInfo[] GetFileInfos(string[] patterns)
        {
            List<FileInfo> infos = new List<FileInfo>();
            DirectoryInfo dir = new DirectoryInfo(Setting.GetFileDirectorPath());
            for (int i = 0; i < patterns.Length; i++)
            {
                FileInfo[] files = dir.GetFiles(patterns[i], SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    infos.Add(files[j]);
                }
            }
            return infos.ToArray();
        }

        private bool IsContainFile(FileInfo file)
        {
            string name = file.Name.Split(".")[0];
            if (IsContainName(name))
            {
                Logger.LogFormat(LogEnum.ReadingFile, file.Name);
                return true;
            }
            return false;
        }

        private bool IsContainName(string name)
        {
            List<string> fileNames = ReadNoTranslationMgr.GetInstance().GetAllFileName();
            return fileNames.Contains(name);
        }

    }
}
