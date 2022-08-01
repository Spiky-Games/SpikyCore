using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SpikyCoreInitializer
{
    public class BuildTools
    {
        private static string OUTPUT_PREF_KEY => "BuildOutputPath_"+ Application.productName;

        [MenuItem("Spiky Tools/Build/With Debug")]
        public static void BuildGame ()
        {
            // Get Output Build Path.
            string outputPath = EditorPrefs.GetString(OUTPUT_PREF_KEY, "");
            if (string.IsNullOrEmpty(outputPath))
            {
                outputPath = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
                EditorPrefs.SetString(OUTPUT_PREF_KEY, outputPath);
            }
            
            string[] levels = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
            
            AddDefineSymbols(debugSymbols);
            
            // Build player.
            BuildPipeline.BuildPlayer(levels, outputPath + $"/{Application.productName}.apk",BuildTarget.Android, BuildOptions.None);
            
            
            RemoveDefineSymbols(debugSymbols);
            
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Spiky Tools/Build/Whats My Build Path?")]
        public static void PrintOutputPath()
        {
            string outputPath = EditorPrefs.GetString(OUTPUT_PREF_KEY, "");
            Debug.Log(outputPath);
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