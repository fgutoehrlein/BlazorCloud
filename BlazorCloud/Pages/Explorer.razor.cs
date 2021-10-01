using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using BlazorCloudCore.Models;
using MudBlazor;
using Microsoft.AspNetCore.Components;
using BlazorCloud.Shared;
using BlazorCloud.Data;
using Microsoft.AspNetCore.Components.Forms;
using BlazorCloudCore.Logic.Services;

namespace BlazorCloud.Pages
{
    public partial class Explorer
    {
        public ModalDialog Modal { get; set; }
        IList<IBrowserFile> files = new List<IBrowserFile>();
        public List<FileBase> Files = new List<FileBase>();
        public List<DirectoryBase> Directories = new List<DirectoryBase>();
        [Inject]
        public IJSRuntime JS { get; set; }
        public UserSessionService _userSessionService;
        public Explorer(UserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
        }

        protected override async Task OnInitializedAsync()
        {
            var items = await GetFilesAndDictionaryInWebRootAsync();
            Files = items.Files;
            Directories = items.Directories;
        }

        /// <summary>
        /// This Method gets the file and dictionary names from the given path
        /// </summary>
        /// <returns></returns>
        public async Task<(List<FileBase> Files, List<DirectoryBase> Directories)> GetFilesAndDictionaryInWebRootAsync()
        {
            var fileDirectory = PathManager.Instance.GetFullFilePath();
            //Fetch all items in the Folder (Directory).
            (string[] fileNames, string[] DirectoryNames) directoryItemNames = await Task.Run(() =>
             {
                 string[] filePaths = Directory.GetFiles(fileDirectory);
                 string[] directoryPaths = Directory.GetDirectories(fileDirectory);
                 return (filePaths, directoryPaths);
             });


            //Copy File names to Model collection.
            List<FileBase> files = new List<FileBase>();

            files = await Task.Run(() =>
            {
                List<FileBase> files = new List<FileBase>();
                foreach (string filePath in directoryItemNames.fileNames)
                {
                    files.Add(new FileBase
                    {
                        FileName = Path.GetFileName(filePath),
                        FilePath = fileDirectory
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
                    directories.Add(new DirectoryBase
                    {
                        DirectoryName = Path.GetFileName(filePath),
                        DirectoryPath = fileDirectory
                    });
                }
                return directories;
            });

            return (files, directories);
        }

        /// <summary>
        /// Generates the required parameters for the modal to manage and view and opens it up.
        /// </summary>
        /// <param name="file"></param>
        public void OpenFileModal(FileBase file)
        {
            var fileSuffix = file.FileName.Split(".").LastOrDefault();

            //Create EventCallback (function pointer) for the download function;
            var download = EventCallback.Factory.Create(this, (Action)(async () => await Download(file)));

            //This Dictionary is used to store the name of the Button and its Functionality and pass that down to the Button
            var Buttons = new Dictionary<string, EventCallback>();
            Buttons.Add("Download", download);

            var parameters = new DialogParameters();
            if (SuffixContainer.Instance.GetSuffixList().Contains(fileSuffix))
            {
                var text = File.ReadAllText(Path.Combine(file.FilePath, file.FileName));
                using (var stringFormatter = new BlazorCloudCore.Logic.String.StringFormater())
                {
                    var htmlText = stringFormatter.ConvertFormattedStringToHtml(text);
                    parameters.Add("ContentText", htmlText);
                }
            }
            else
            {
                parameters.Add("ContentText", "File: " + file.FileName);
            }

            parameters.Add("Buttons", Buttons);

            DialogService.Show<ModalDialog>(file.FileName, parameters);
        }

        public void OpenFileUploadModal()
        {
            DialogService.Show<ModalDialog>();
        }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
            }
            foreach (var file in e.GetMultipleFiles())
            {
                if (!System.IO.File.Exists(Path.Combine(PathManager.Instance.GetFullFilePath(), file.Name)))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(Path.Combine(PathManager.Instance.GetFullFilePath(), file.Name)))
                    {
                        await file.OpenReadStream().CopyToAsync(fs);
                    }
                }
                else
                {
                    Console.WriteLine("File \"{0}\" already exists.", file.Name);

                    return;
                }
            }
            var items = await GetFilesAndDictionaryInWebRootAsync();
            Files = items.Files;
            Directories = items.Directories;
        }

        /// <summary>
        /// Invokes Javascript to download the given file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task Download(FileBase file)
        {
            byte[] data = System.IO.File.ReadAllBytes(Path.Combine(file.FilePath, file.FileName));
            await JS.InvokeAsync<object>("saveAsFile", file.FileName, Convert.ToBase64String(data));
        }
    }
}
