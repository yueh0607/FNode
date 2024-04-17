using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FNode.Editor
{
    internal static class FileAccessUtility
    {
        /// <summary>
        /// 检测文件是否被读占用
        /// </summary>

        public static bool IsFileReadInUse(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// 检测文件是否被写占用
        /// </summary>
        public static bool IsFileWriteInUse(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// 检测文件是否被读写占用
        /// </summary>
        public static bool IsFileReadWriteInUse(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// 确保文件或目录必定存在，不会发送文件/目录不存在的异常
        /// </summary>
        public static void FileAndDirectoryMustExist(string fileOrDir)
        {
            if(Path.HasExtension(fileOrDir))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileOrDir));
                if(File.Exists(fileOrDir))
                    File.Create(fileOrDir).Dispose();
            }
            else
                Directory.CreateDirectory(Path.GetDirectoryName(fileOrDir));
        }
    }
}
