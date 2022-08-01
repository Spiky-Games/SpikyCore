using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SpikyCoreInitializer
{
    public class InitialProjectStructure
    {
         
        [MenuItem("Spiky Tools/Create Initial Folders")]
        public static void SetupInitialFolders()
        {
            string gameFolder = "_" + Application.productName;
            FolderUtilities.CreateGroupOfFolder(gameFolder, new []{"Art","Scripts","Materials","Scenes","Prefabs"});
        }
        
        [MenuItem("Spiky Tools/URP/Setup Post Processing")]
        public static void SetupPostProcessing()
        {
            List<VolumeProfile> volumeProfiles = FindAssetsByType<VolumeProfile>();
            volumeProfiles.ForEach(volumeProfile =>
            {
                if (!volumeProfile.TryGet(out ColorAdjustments colorAdjustments))
                {
                    colorAdjustments = VolumeProfileFactory.CreateVolumeComponent<ColorAdjustments>(volumeProfile);
                }
                colorAdjustments.contrast.overrideState = true;
                colorAdjustments.saturation.overrideState = true;
                
                Debug.Log($"{AssetDatabase.GetAssetPath(volumeProfile)} Has Changed.");
                EditorUtility.SetDirty(volumeProfile);
                
                AssetDatabase.SaveAssetIfDirty(volumeProfile);
                Selection.activeObject = volumeProfile;
            });
        }

        private static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                if (assetPath.Contains("Packages/"))
                {
                    Debug.Log($"{assetPath} has been ignored because is inside the unity package Folder.");
                    continue;
                }
                T asset = AssetDatabase.LoadAssetAtPath<T>( assetPath );
                if( asset != null )
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}