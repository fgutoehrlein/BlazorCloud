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
using Microsoft.AspNetCore.Identity;
using BlazorCloud.Areas.Authorization;

namespace BlazorCloud.Pages
{
    public partial class Explorer
    {
        IList<IBrowserFile> files = new List<IBrowserFile>();
        [Inject]
        public FileAndDirectoryService _fileAndDirectoryService { get; set; }
        [Inject]
        public UserSessionService _userSessionService { get; set; }

        public void OpenFileUploadModal()
        {
            DialogService.Show<ModalDialog>();
        }
        /// <summary>
        /// Handles the upload of files and writes them to the server file system
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            var fullPath = PathManager.Instance.GetFullFilePath();
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
            }
            foreach (var file in e.GetMultipleFiles())
            {
                if (!System.IO.File.Exists(Path.Combine(fullPath, file.Name)))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(Path.Combine(fullPath, file.Name)))
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
        }
    }
}
