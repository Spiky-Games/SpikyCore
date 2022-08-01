using System.IO;
using UnityEngine;

namespace SpikyCoreInitializer
{
    public static class FolderUtilities
    {
        /// <summary>
        /// Creates a folder inside the project. ie: ProjectDirectory/Assets/name
        /// </summary>
        /// <param name="name">Creates a folder using this name</param>
        public static void CreateFolder(string name)
        {
            string root = Application.dataPath;
            string folderPath = Path.Combine(root, name);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        
        /// <summary>
        /// Creates folders inside the project. ie: ProjectDirectory/Assets/root/foldersName
        /// </summary>
        /// <param name="name">Creates a folder using this name</param>
        public static void CreateGroupOfFolder(string root, string[] foldersName)
        {
            CreateFolder(root);
           
            foreach (string folderName in foldersName)
            {
                CreateFolder(Path.Combine(root, folderName));
            }
        }
    }
}