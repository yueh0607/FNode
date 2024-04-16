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
    }
}
