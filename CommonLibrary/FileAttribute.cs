using System;
using System.IO;

namespace CommonLibrary
{

    public class FileAttribute
    {
        public FileAttribute()
        {
            SetDefaultValue();
        }

        public FileAttribute(bool isDirectory, FileInfo fileInfo)
        {
            SetFileAttributes(isDirectory, fileInfo);
        }

        #region public properties
        public bool IsDirectory { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public double Size { get; set; }
        public double SizeByUnit { get; set; }
        public string SizeUnit { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public string OldName { get; set; }
        #endregion

        #region private methods
        private void SetDefaultValue()
        {
            IsDirectory = false;
            Path = string.Empty;
            Name = string.Empty;
            Extension = string.Empty;
            CreatedDateTime = DateTime.MinValue;
            LastModifiedDateTime = DateTime.MinValue;
            OldName = string.Empty;
            Size = 0;
            SizeByUnit = 0;
            SizeUnit = string.Empty;
        }

        private void SetFileAttributes(bool isDirectory, FileInfo fileInfo)
        {
            IsDirectory = isDirectory;
            Path = fileInfo.Directory.FullName;
            Name = fileInfo.Name;
            Extension = fileInfo.Extension;
            CreatedDateTime = fileInfo.CreationTimeUtc;
            LastModifiedDateTime = fileInfo.LastWriteTimeUtc;
            OldName = string.Empty;
            if (!IsDirectory)
            {
                SetFileSize(fileInfo);
            }
        }

        private void SetFileSize(FileInfo fileInfo)
        {
            Size = SizeByUnit = fileInfo.Length;
            if (SizeByUnit < 1024)
            {
                SizeUnit = "B";
            }
            else if (SizeByUnit < 1024 * 1024)
            {
                SizeByUnit = Math.Round(SizeByUnit / 1024, 2);
                SizeUnit = "KB";
            }
            else if (SizeByUnit < 1024 * 1024 * 1024)
            {
                SizeByUnit = Math.Round(SizeByUnit / (1024 * 1024), 2);
                SizeUnit = "MB";
            }
            else
            {
                throw new Exception(String.Format("\"{0}\" File to much big. please contact your administrator.", Name));
            }
        }
        #endregion
    }
}
