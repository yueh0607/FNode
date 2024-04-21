using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FNode.Editor
{

    /// <summary>
    /// Unity不支持同一类型多个编辑器窗口的创建，需要辅助类自行创建
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class CreateWindowUtility<T> where T : EditorWindow
    {
        public static Dictionary<string, T> Windows = new Dictionary<string, T>();


        /// <summary>
        /// 创建或者聚焦已有窗口
        /// </summary>
        /// <param name="unique"></param>
        /// <param name="utility"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T CreateWindow(string unique, string title, bool utility = false)
        {
            if (Windows.TryGetValue(unique, out T existedWindow) && existedWindow != null)
            {
                existedWindow.Show();
                existedWindow.Focus();
                return existedWindow;
            }
            else
            {
                Windows.Remove(unique);
                T editorWindow = ScriptableObject.CreateInstance<T>();
                Windows.Add(unique, editorWindow);
                if (title != null)
                {
                    editorWindow.titleContent = new GUIContent(title);
                }
                if (utility)
                {
                    editorWindow.ShowUtility();
                }
                else
                {
                    editorWindow.Show();
                }
                return editorWindow;
            }

            throw new System.InvalidOperationException("Unknown error");


        }
    }
}
