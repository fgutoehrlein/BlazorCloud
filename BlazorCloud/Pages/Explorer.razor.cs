using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using BlazorCloudCore.Models;

namespace BlazorCloud.Pages
{
    public partial class Explorer
    {
        private string WebRootPath;
        public List<FileBase> Files = new List<FileBase>();
        public List<DirectoryBase> Directories = new List<DirectoryBase>();

        protected override async Task OnInitializedAsync()
        {
            WebRootPath = Startup.WwwRootPath;
            var items = await GetFilesInWebRootAsync();
            Files = items.Files;
            Directories = items.Directories;
        }
        public async Task<(List<FileBase> Files, List<DirectoryBase> Directories)> GetFilesInWebRootAsync()
        {
            //Fetch all items in the Folder (Directory).
            (string[] fileNames,string[] DirectoryNames) directoryItemNames = await Task.Run(() =>
            {
                string[] filePaths = Directory.GetFiles(Path.Combine(this.WebRootPath, "Files/"));
                string[] directoryPaths = Directory.GetDirectories(Path.Combine(this.WebRootPath, "Files/"));
                return (filePaths,directoryPaths);
            });
            

            //Copy File names to Model collection.
            List<FileBase> files = new List<FileBase>();

            files = await Task.Run(() =>
            {
                List<FileBase> files = new List<FileBase>();
                foreach (string filePath in directoryItemNames.fileNames)
                {
                    files.Add(new FileBase { 
                        FileName = Path.GetFileName(filePath),
                        FilePath = WebRootPath
                    });
                }
                return files;
            });

            //Copy Directory names to Model collection
            List<DirectoryBase> directories = new List<DirectoryBase>();
            directories = await Task.Run(() =>
            {
                List<DirectoryBase> directories = new List<DirectoryBase>();
                foreach (string filePath in directoryItemNames.DirectoryNames)
                {
                    directories.Add(new DirectoryBase {
                        DirectoryName = Path.GetFileName(filePath),
                        DirectoryPath = WebRootPath
                    });
                }
                return directories;
            });

            return (files,directories);
        }
    }
}
