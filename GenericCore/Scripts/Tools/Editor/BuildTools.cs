using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;
using Codice.Client.BaseCommands;

namespace SpikyCoreInitializer
{
    public class BuildTools
    {
        private static string OUTPUT_PREF_KEY => "BuildOutputPath_"+ Application.productName;
        private static string BUILD_PATH_PREF_KEY => "BuildPath_"+ Application.productName;
        
        public static string BuildPath
        {
            get => PlayerPrefs.GetString(BUILD_PATH_PREF_KEY);
            set => PlayerPrefs.SetString(BUILD_PATH_PREF_KEY, value);
        }  

        [MenuItem("Spiky Tools/Build/With Debug")]
        public static void BuildGame ()
        {
            string outputPath = GetOrCreateBuildPath();

            var options = new BuildPlayerOptions();
            BuildPlayerOptions playerSettings = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(options);
            
            AddDefineSymbols(debugSymbols);
            
            BuildPipeline.BuildPlayer(playerSettings);
            BuildPath = playerSettings.locationPathName;
            RemoveDefineSymbols(debugSymbols);
            
            string finalPath = outputPath + $"/{Application.productName}.apk";
            File.Delete(finalPath);
            File.Copy(BuildPath, finalPath);
            
            AssetDatabase.SaveAssets();
        }

        private static string GetOrCreateBuildPath()
        {
            string outputPath = EditorPrefs.GetString(OUTPUT_PREF_KEY, "");
            if (string.IsNullOrEmpty(outputPath))
            {
                outputPath = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
                EditorPrefs.SetString(OUTPUT_PREF_KEY, outputPath);
            }

            return outputPath;
        }

        [MenuItem("Spiky Tools/Build/Whats My Build Path?")]
        public static void PrintOutputPath()
        {
            string outputPath = EditorPrefs.GetString(OUTPUT_PREF_KEY, "");
            Debug.Log(outputPath);
        }
        
        [MenuItem("Spiky Tools/Build/DebugMenu/Add")]
        public static void AddDebugMenu ()
        {
            AddDefineSymbols(debugSymbols);
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("Spiky Tools/Build/DebugMenu/Remove")]
        public static void RemoveDebugMenu ()
        {
            RemoveDefineSymbols(debugSymbols);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Spiky Tools/Build/Clear Output Path")]
        public static void ClearOutputPath()
        {
            EditorPrefs.DeleteKey(OUTPUT_PREF_KEY);
        }
        
        /// <summary>
        /// Symbols that will be added to the editor
        /// </summary>
        public static readonly string [] debugSymbols = {"USE_DEBUG"};
 
        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        private static void AddDefineSymbols(string[] symbolsToAdd)
        {
            string alreadyDefined = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = alreadyDefined.Split ( ';' ).ToList ();
            
            allDefines.AddRange ( symbolsToAdd.Except ( allDefines ) );

            string finalSymbols = string.Join(";", allDefines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, finalSymbols);
        }
        
        private static void RemoveDefineSymbols(string[] symbolsToRemove)
        {
            string alreadyDefined = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefined = alreadyDefined.Split ( ';' ).ToList ();
            
            symbolsToRemove.ToList().ForEach(symbol=> allDefined.Remove(symbol));

            string finalSymbols = string.Join(";", allDefined.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, finalSymbols);
        }
    }
}