using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using static Player.ViewModels.PlayerViewModel;

namespace Player.ViewModels
{
    internal partial class PlayerViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Folder> folders;

        [ObservableProperty]
        public Folder selectedFolder;

        public string strFolder { get; set; }

        public PlayerViewModel()
        {
            strFolder = @"D:\Топ музон";
            Folders = GetSubfolders(strFolder);
            
        }

        public ObservableCollection<Folder> GetSubfolders(string strPath)
        {
            ObservableCollection<Folder> subfolders = new ObservableCollection<Folder>();
            string[] subdirs = Directory.GetDirectories(strPath, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in subdirs)
            {
                Folder thisnode = new Folder(dir);

                if (!dir.Contains("System Volume Information"))
                {
                    //var files = Directory.GetFiles(dir, @"*(\.mp3|\.wav|\.flac)$", SearchOption.AllDirectories);
                    if (Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        thisnode.Subfolders = new ObservableCollection<Folder>();
                        thisnode.Subfolders = GetSubfolders(dir);
                    }

                    var allowedExtensions = new[] { ".mp3", ".flac", ".wav" };
                    var files = Directory
                        .GetFiles(dir, "*", SearchOption.AllDirectories)
                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                        .ToArray();

                    if (files.Length > 0)
                    {
                        thisnode.Files = new ObservableCollection<FileInfo>();
                        foreach (string f in files)
                        {
                            FileInfo file = new FileInfo(f);
                            thisnode.Files.Add(file);
                        }
                    }
                }

                subfolders.Add(thisnode);
            }

            return subfolders;
        }

        public class Folder
        {
            public ObservableCollection<Folder>? Subfolders { get; set; }
            public ObservableCollection<FileInfo>? Files { get; set; }
            public string Name { get; }
            public string Path { get; }

            public Folder(string path)
            {
                Path = path;
                Name = System.IO.Path.GetFileName(path);
            }
        }

        public class File
        {
            public string Name { get; }
            public string Path { get; }
            public string Extension { get; }

            public File(string path)
            {
                Path = path;
                Name = System.IO.Path.GetFileName(path);
                Extension = System.IO.Path.GetExtension(path);
            }
        }
    }
}
