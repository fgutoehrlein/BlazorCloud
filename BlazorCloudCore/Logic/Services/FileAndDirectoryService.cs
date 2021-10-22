using BlazorCloudCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCloudCore.Logic.Services
{
    public class FileAndDirectoryService
    {
        /// <summary>
        /// Returns the directories which are within the given directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>DirectoryBase</returns>
        public async Task<List<DirectoryBase>> GetListOfDirectoriesFromPathAsync(DirectoryBase directory)
        {
            var fullDirectoryPath = Path.Combine(directory.DirectoryPath, directory.DirectoryName);
            //Fetch all Folders in the Directory.
            string[] DirectoryNames  = await Task.Run(() =>
            {
                string[] directoryPaths = Directory.GetDirectories(fullDirectoryPath);
                return directoryPaths;
            });

            //Copy Directory names to Model collection
            List<DirectoryBase> directories = new List<DirectoryBase>();
            directories = await Task.Run(() =>
            {
                List<DirectoryBase> directories = new List<DirectoryBase>();
                foreach (string filePath in DirectoryNames)
                {
                    directories.Add(new DirectoryBase
                    {
                        DirectoryName = Path.GetFileName(filePath),
                        DirectoryPath = fullDirectoryPath
                    });
                }
                return directories;
            });
            return directories;
        }
        public async Task<List<DirectoryBase>> GetListOfDirectoriesFromPathAsync(string fullDirectoryPath)
        {
            //Fetch all Folders in the Directory.
            string[] DirectoryNames = await Task.Run(() =>
            {
                string[] directoryPaths = Directory.GetDirectories(fullDirectoryPath);
                return directoryPaths;
            });

            //Copy Directory names to Model collection
            List<DirectoryBase> directories = new List<DirectoryBase>();
            directories = await Task.Run(() =>
            {
                List<DirectoryBase> directories = new List<DirectoryBase>();
                foreach (string filePath in DirectoryNames)
                {
                    directories.Add(new DirectoryBase
                    {
                        DirectoryName = Path.GetFileName(filePath),
                        DirectoryPath = fullDirectoryPath
                    });
                }
                return directories;
            });
            return directories;
        }

        public async Task<List<FileBase>> GetListOfFilesFromPathAsync(FileBase file)
        {
            var fullDirectoryPath = Path.Combine(file.FilePath, file.FileName);
            //Fetch all Folders in the Directory.
            string[] DirectoryNames = await Task.Run(() =>
            {
                string[] directoryPaths = Directory.GetFiles(fullDirectoryPath);
                return directoryPaths;
            });

            //Copy Directory names to Model collection
            List<FileBase> files = new List<FileBase>();
            files = await Task.Run(() =>
            {
                List<FileBase> directories = new List<FileBase>();
                foreach (string filePath in DirectoryNames)
                {
                    directories.Add(new FileBase
                    {
                        FileName = Path.GetFileName(filePath),
                        FilePath = fullDirectoryPath
                    });
                }
                return directories;
            });
            return files;
        }
        public async Task<List<FileBase>> GetListOfFilesFromPathAsync(string fullDirectoryPath)
        {
            //Fetch all Folders in the Directory.
            string[] DirectoryNames = await Task.Run(() =>
            {
                string[] directoryPaths = Directory.GetFiles(fullDirectoryPath);
                return directoryPaths;
            });

            //Copy Directory names to Model collection
            List<FileBase> files = new List<FileBase>();
            files = await Task.Run(() =>
            {
                List<FileBase> directories = new List<FileBase>();
                foreach (string filePath in DirectoryNames)
                {
                    directories.Add(new FileBase
                    {
                        FileName = Path.GetFileName(filePath),
                        FilePath = fullDirectoryPath
                    });
                }
                return directories;
            });
            return files;
        }
        /// <summary>
        /// This Method gets the file and dictionary names from the given path
        /// </summary>
        /// <param name="fullDirectoryPath"></param>
        /// <returns></returns>
        public async Task<(List<FileBase> Files, List<DirectoryBase> Directories)> GetFilesAndDictionaryInGivenDirectoryAsync(string fullDirectoryPath)
        {
            //Fetch all items in the Folder (Directory).
            (string[] fileNames, string[] DirectoryNames) directoryItemNames = await Task.Run(() =>
            {
                string[] filePaths = Directory.GetFiles(fullDirectoryPath);
                string[] directoryPaths = Directory.GetDirectories(fullDirectoryPath);
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
                        FilePath = fullDirectoryPath
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
                        DirectoryPath = fullDirectoryPath
                    });
                }
                return directories;
            });

            return (files, directories);
        }
        public async Task<(List<FileBase> Files, List<DirectoryBase> Directories)> GetFilesAndDictionaryInGivenDirectoryAsync(DirectoryBase directory)
        {
            var fullDirectoryPath = Path.Combine(directory.DirectoryPath,directory.DirectoryName);
            //Fetch all items in the Folder (Directory).
            (string[] fileNames, string[] DirectoryNames) directoryItemNames = await Task.Run(() =>
            {
                string[] filePaths = Directory.GetFiles(fullDirectoryPath);
                string[] directoryPaths = Directory.GetDirectories(fullDirectoryPath);
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
                        FilePath = fullDirectoryPath
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
                        DirectoryPath = fullDirectoryPath
                    });
                }
                return directories;
            });

            return (files, directories);
        }
    }
}
