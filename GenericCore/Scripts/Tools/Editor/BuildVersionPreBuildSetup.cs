using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildVersionPreBuildSetup : IPreprocessBuildWithReport
{
    public int callbackOrder { get; }
    public void OnPreprocessBuild(BuildReport report)
    {
        string alreadyDefined = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = alreadyDefined.Split ( ';' ).ToList ();
        string versionSymbol = allDefines.FirstOrDefault(s => s.Contains("VERSION"));
        if (!string.IsNullOrEmpty(versionSymbol))
        {
            var splittedArguments = versionSymbol.Split("_");
            var bundleVersion = splittedArguments[1];
            var bundleVersionCode = splittedArguments[2];
            PlayerSettings.bundleVersion = bundleVersion;
            PlayerSettings.Android.bundleVersionCode = int.Parse(bundleVersionCode);
            PlayerSettings.iOS.buildNumber = bundleVersionCode;
            Debug.Log("Version Changed " + splittedArguments);
        }
        
    }
}
