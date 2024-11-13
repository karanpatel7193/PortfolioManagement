using System;
using System.IO;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public enum ContentTypes { String = 1, Base64String = 2, Bytes = 3, Stream = 4 }

    public class MyFile
    {
        #region Public Properties
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        public object Content { get; set; }
        public ContentTypes ContentType { get; set; }
        public int Size { get; set; }

        private string FullPath
        {
            get
            {
                return Path.TrimEnd('/') + "/" + Name;
            }
        }
        #endregion

        public MyFile()
        {
            ContentType = ContentTypes.Base64String;
        }

        public string Create(bool IsAddDateTimeInName = false)
        {
            string FullPathNew = FullPath;
            string DateTimeString = string.Empty;
            if (IsAddDateTimeInName)
            {
                DateTimeString = DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF");
                FullPathNew = FullPath.Replace(".", DateTimeString + ".");
            }

            if (File.Exists(FullPathNew))
                throw new Exception(String.Format("\"{0}\" File is already exist.", Name));

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            switch (ContentType)
            {
                case ContentTypes.String:
                    File.WriteAllText(FullPathNew, MyConvert.ToString(Content));
                    break;
                case ContentTypes.Base64String:
                    string[] content = MyConvert.ToString(Content).Split(',');
                    Byte[] bytes = Convert.FromBase64String(content[content.Length - 1]);
                    File.WriteAllBytes(FullPathNew, bytes);
                    break;
                case ContentTypes.Bytes:
                    File.WriteAllBytes(FullPathNew, (byte[])Content);
                    break;
                case ContentTypes.Stream:
                    FileStream fileStream = new FileStream(FullPathNew, FileMode.Create, FileAccess.Write);
                    ((Stream)Content).CopyTo(fileStream);
                    fileStream.Dispose();
                    break;
            }

            return Name.Replace(".", DateTimeString + ".");
        }

        public void Modify()
        {
            Delete();

            Create(false);
        }

        public string Recreate(string OldFileName = "", bool IsAddDateTimeInName = false)
        {
            Delete(OldFileName);

            return Create(IsAddDateTimeInName);
        }

        public void Delete()
        {
            Delete(Name);
        }

        private void Delete(string fileName)
        {
            if (!File.Exists(Path.TrimEnd('/') + "/" + fileName))
                throw new Exception(String.Format("\"{0}\" File do not exist.", fileName));

            File.Delete(Path.TrimEnd('/') + "/" + fileName);
        }

        public string Rename(string newName, bool IsAddDateTimeInName = false)
        {
            if (!File.Exists(FullPath))
                throw new Exception(String.Format("\"{0}\" File do not exist.", Name));

            string FullPathNew = Path.TrimEnd('/') + "/" + newName;
            string DateTimeString = string.Empty;
            if (IsAddDateTimeInName)
            {
                DateTimeString = DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF");
                FullPathNew = FullPath.Replace(".", DateTimeString + ".");
            }

            if (File.Exists(FullPathNew))
                throw new Exception(String.Format("\"{0}\" File is already exist.", newName));

            Content = File.ReadAllBytes(FullPath);
            Delete();
            File.WriteAllBytes(FullPathNew, (byte[])Content);

            return newName.Replace(".", DateTimeString + ".");
        }

        public string GetExtension(string fileName)
        {
            string[] Names = fileName.Split('.');
            return Names[Names.Length - 1];
        }

        public bool Exists()
        {
            return (File.Exists(FullPath));
        }

        #region Asynch Methods 
        public async Task<string> CreateAsync(bool IsAddDateTimeInName = false)
        {
            string FullPathNew = FullPath;
            string DateTimeString = string.Empty;
            if (IsAddDateTimeInName)
            {
                DateTimeString = DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF");
                FullPathNew = FullPath.Replace(".", DateTimeString + ".");
            }

            if (File.Exists(FullPathNew))
                throw new Exception(String.Format("\"{0}\" File is already exist.", Name));

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            switch (ContentType)
            {
                case ContentTypes.String:
                    await File.WriteAllTextAsync(FullPathNew, MyConvert.ToString(Content));
                    break;
                case ContentTypes.Base64String:
                    string[] content = MyConvert.ToString(Content).Split(',');
                    Byte[] bytes = Convert.FromBase64String(content[content.Length - 1]);
                    await File.WriteAllBytesAsync(FullPathNew, bytes);
                    break;
                case ContentTypes.Bytes:
                    await File.WriteAllBytesAsync(FullPathNew, (byte[])Content);
                    break;
                case ContentTypes.Stream:
                    FileStream fileStream = new FileStream(FullPathNew, FileMode.Create, FileAccess.Write);
                    await ((Stream)Content).CopyToAsync(fileStream);
                    await fileStream.DisposeAsync();
                    break;
            }

            return Name.Replace(".", DateTimeString + ".");
        }

        public async Task ModifyAsync()
        {
            Delete();

            await CreateAsync(false);
        }

        public async Task<string> RecreateAsync(string OldFileName = "", bool IsAddDateTimeInName = false)
        {
            Delete(OldFileName);

            return await CreateAsync(IsAddDateTimeInName);
        }

        public async Task<string> RenameAsync(string newName, bool IsAddDateTimeInName = false)
        {
            if (!File.Exists(FullPath))
                throw new Exception(String.Format("\"{0}\" File do not exist.", Name));

            string FullPathNew = Path.TrimEnd('/') + "/" + newName;
            string DateTimeString = string.Empty;
            if (IsAddDateTimeInName)
            {
                DateTimeString = DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF");
                FullPathNew = FullPath.Replace(".", DateTimeString + ".");
            }

            if (File.Exists(FullPathNew))
                throw new Exception(String.Format("\"{0}\" File is already exist.", newName));

            Content = await File.ReadAllBytesAsync(FullPath);
            Delete();
            await File.WriteAllBytesAsync(FullPathNew, (byte[])Content);

            return newName.Replace(".", DateTimeString + ".");
        }

        #endregion
    }
}
