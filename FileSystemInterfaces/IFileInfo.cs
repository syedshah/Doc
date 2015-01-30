// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileInfo.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   Defines the IFileInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FileSystemInterfaces
{
    using System;
    using System.IO;

    public interface IFileInfo
    {
        Boolean Exists { get; }

        string Extension { get; }

        Stream Create();

        void Delete();

        Stream Open(FileMode fileMode);

        Stream Open(FileMode fileMode, FileAccess fileAccess);
    }
}
