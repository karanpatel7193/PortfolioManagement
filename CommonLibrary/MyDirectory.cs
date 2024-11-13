using System;
using System.Collections.Generic;
using System.IO;

namespace CommonLibrary
{
    public class MyDirectory
    {
        #region Public Properties
        public string Path { get; set; }
        public string Name { get; set; }

        private string FullPath
        {
            get
            {
                return Path.TrimEnd('/').TrimEnd('\\') + "\\" + Name;
            }
        }
        #endregion

        public MyDirectory()
        {
        }

        public bool Exists()
        {
            return Directory.Exists(FullPath);
        }

        public void Create()
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);
            else
                throw new Exception(String.Format("\"{0}\" Directory is already exist.", FullPath));
        }

        public void Delete(bool Recursive = false)
        {
            if (!Directory.Exists(FullPath))
                throw new Exception(String.Format("\"{0}\" Directory do not exist.", FullPath));
            else
                Directory.Delete(FullPath, Recursive);
        }

        public void Rename(string NewName)
        {
            if (!Directory.Exists(FullPath))
                throw new Exception(String.Format("\"{0}\" Directory do not exist.", FullPath));

            string Destination = Path.TrimEnd('/') + "\\" + NewName;
            Directory.CreateDirectory(Destination);

            CopySubDirectories(FullPath, Destination);

            Delete(true);
        }

        private void CopySubDirectories(string Source, string Destination)
        {
            CopyFiles(Source, Destination);

            foreach (string name in Directory.GetDirectories(Source))
            {
                string FolderName = (new FileInfo(name)).Name;
                string NewSource = Source.TrimEnd('\\') + "\\" + FolderName;
                string NewDestination = Destination.TrimEnd('\\') + "\\" + FolderName;
                Directory.CreateDirectory(NewDestination);
                CopySubDirectories(NewSource, NewDestination);
            }
        }

        private void CopyFiles(string Source, string Destination)
        {
            foreach (string name in Directory.GetFiles(Source))
            {
                string fileName = (new FileInfo(name)).Name;
                string NewSource = Source.TrimEnd('\\') + "\\" + fileName;
                string NewDestination = Destination.TrimEnd('\\') + "\\" + fileName;
                File.Copy(NewSource, NewDestination);
            }
        }

        public List<FileAttribute> GetLists()
        {
            List<FileAttribute> Files = new List<FileAttribute>();
            foreach (string name in Directory.GetFiles(Path))
            {
                Files.Add(new FileAttribute(false, new FileInfo(name)));
            }
            foreach (string name in Directory.GetDirectories(Path))
            {
                Files.Add(new FileAttribute(true, new FileInfo(name)));
            }
            return Files;
        }


        public void RenameFolder(string FullPath, string Path, string NewName)
        {
            if (!Directory.Exists(FullPath))
                throw new Exception(String.Format("\"{0}\" Directory do not exist.", FullPath));

            string Destination = Path + "/" + NewName;
            Directory.CreateDirectory(Destination);

            CopySubFolders(FullPath, Destination);


            if (!Directory.Exists(FullPath))
                throw new Exception(String.Format("\"{0}\" Directory do not exist.", FullPath));
            else
                Directory.Delete(FullPath, true);
        }

        private void CopySubFolders(string Source, string Destination)
        {
            CopyFolderFiles(Source, Destination);

            foreach (string name in Directory.GetDirectories(Source))
            {
                string FolderName = (new FileInfo(name)).Name;
                string NewSource = Source + "/" + FolderName;
                string NewDestination = Destination + "/" + FolderName;
                Directory.CreateDirectory(NewDestination);
                CopySubDirectories(NewSource, NewDestination);
            }
        }

        private void CopyFolderFiles(string Source, string Destination)
        {
            foreach (string name in Directory.GetFiles(Source))
            {
                string fileName = (new FileInfo(name)).Name;
                string NewSource = Source + "/" + fileName;
                string NewDestination = Destination + "/" + fileName;
                System.IO.File.Copy(NewSource, NewDestination);
            }
        }

    }
}
