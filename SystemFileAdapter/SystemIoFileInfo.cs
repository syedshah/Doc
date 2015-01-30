// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemIoFileInfo.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the SystemIoFileInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;

namespace SystemFileAdapter
{
    using FileSystemInterfaces;

    public class SystemIoFileInfo : IFileInfo
    {
        private readonly FileInfo _file;

        public SystemIoFileInfo(String fileName)
        {
            _file = new FileInfo(fileName);
        }

        public Boolean Exists
        {
            get
            {
                return _file.Exists;
            }
        }

        public string Extension
        {
            get
            {
                return _file.Extension;
            }
        }

        public Stream Create()
        {
            if (_file.Exists)
            {
                throw new IOException("File already exists");
            }
            return _file.Create();
        }

        public void Delete()
        {
            _file.Delete();
        }

        public Stream Open(FileMode fileMode)
        {
            return _file.Open(fileMode);
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess)
        {
            return _file.Open(fileMode, fileAccess);
        }
    }
}
