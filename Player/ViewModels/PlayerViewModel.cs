using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using static Player.ViewModels.PlayerViewModel;
using static System.Net.WebRequestMethods;

namespace Player.ViewModels
{
    internal partial class PlayerViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Folder> folders;

        [ObservableProperty]
        public Folder selectedFolder;

        public PlayerViewModel()
        {
            Folders = GetSubfolders(@"D:");
        }

        public ObservableCollection<Folder> GetSubfolders(string path)
        {
            ObservableCollection<Folder> subfolders = new ObservableCollection<Folder>();
            string[] subfoldersNames = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in subfoldersNames)
            {
                Folder currentFolder = new Folder(dir);

                int count = GetCountOfDirectories(dir);
                if (count >= 0)
                {
                    if (count > 0)
                    {
                        currentFolder.Subfolders = GetSubfolders(dir);
                    }
                    currentFolder.Files = GetFiles(dir, new[] { ".mp3", ".flac", ".wav" });
                }

                subfolders.Add(currentFolder);
            }

            return subfolders;
        }
        private ObservableCollection<FileInfo> GetFiles(string path, string[]? extensions = null)
        {
            extensions ??= new[] { ".mp3", ".flac", ".wav" };
            var files = Directory
                .GetFiles(path, "*", SearchOption.AllDirectories)
                .Where(file => extensions.Any(file.ToLower().EndsWith))
                .ToArray();

            var filesInfo = new ObservableCollection<FileInfo>();

            foreach (string f in files)
            {
                FileInfo file = new FileInfo(f);
                filesInfo.Add(file);
            }

            return filesInfo;
        }

        /// <summary>
        /// Get count of subdirectories if directory at the given <paramref name="path"/> is accessible. Otherwise returns -1.
        /// </summary>
        /// <param name="path"> Path to the directory. </param>
        /// <returns>
        /// -1 if access is denied.
        /// Any other non negative number if directory is accessible.
        /// </returns>
        private int GetCountOfDirectories(string path)
        {
            int count;
            try
            {
                count = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).Length;
            }
            catch
            {
                return -1;
            }

            return count;
        }     
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
}
