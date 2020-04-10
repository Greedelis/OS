using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OS
{
    public class SoftDisk
    {
        private const string root = "root";
        private string _currentFolder = "root";

        public List<string> GetAllFilesInCurrentFolder()
        {
            var files = Directory.GetFileSystemEntries(_currentFolder, "", SearchOption.TopDirectoryOnly).ToList();
            return files;
        }

        public List<string> GetAllDirectories()
        {
            var directories = Directory.GetDirectories(_currentFolder).ToList();
            /*for(var i = 0; i < directories.Count; i++)
            {
                directories[i] = Path.GetDirectoryName(directories[i]);
            }*/
            return directories;
        }

        public List<FileInfo> GetAllFileInfo()
        {
            var fileInfoList = new List<FileInfo>();
            foreach (var file in GetAllFilesInCurrentFolder())
            {
                var fileInfo = new FileInfo(file);
                if(fileInfo.Exists)
                    fileInfoList.Add(fileInfo);
            }

            return fileInfoList;
        }

        public bool ChangeDir(string folder)
        {
            if (folder == ".." && _currentFolder != root)
            {
                var temp = _currentFolder.Split('\\').Last();
                _currentFolder = Path.GetRelativePath(Path.Combine(root, ".."), Path.Combine( _currentFolder, "..")); //very ugly, bet tingiu galvot db kaip kitaip :D
                return true;
            }
            var newDir = $"{_currentFolder}\\{folder}";
            if (!Directory.Exists(newDir)) return false;
            _currentFolder = newDir;
            return true;

        }

        public List<string> OpenFile(string file)
        {
            return File.ReadAllLines(Path.Combine($"{_currentFolder}", file)).ToList();
        }
    }
}