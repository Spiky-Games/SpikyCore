using System;
using System.Collections.Generic;
using System.IO;
using Shapes2D;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace SpikyCoreInitializer
{
    public class InitialProjectStructure
    {
        
        private static string GameProjectFolder => "_" + Application.productName;
        private static string ScenesProjectFolder => "Assets/_" + Application.productName + "/Scenes/";
        
        [MenuItem("Spiky Tools/Create Initial Folders")]
        public static void SetupInitialFolders()
        {
            FolderUtilities.CreateGroupOfFolder(GameProjectFolder, new []{"Art","Scripts","Materials","Scenes","Prefabs"});
            CreateBaseScenes();
            SetupLevelLoader();
        }

        private static void CreateBaseScenes()
        {
            //Creating Base Scenes
            AssetDatabase.CopyAsset("Assets/SpikyCore/GenericCore/Scenes/Init.unity", ScenesProjectFolder + "Init.unity");

            if(File.Exists(Application.dataPath + "/Scenes/SampleScene.unity"))
            {
                AssetDatabase.MoveAsset("Assets/Scenes/SampleScene.unity", ScenesProjectFolder + "Gameplay.unity");
            }else {
                Scene gameplayScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                EditorSceneManager.SaveScene(gameplayScene, ScenesProjectFolder + "Gameplay.unity");
                EditorSceneManager.CloseScene(gameplayScene, true);
            }
            
            FolderUtilities.CreateGroupOfFolder(GameProjectFolder, new []{"Art","Scripts","Materials","Scenes","Prefabs"});
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(ScenesProjectFolder + "Init.unity", true));
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(ScenesProjectFolder + "Gameplay.unity", true));
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            SceneAutoLoader.SetMasterScene(ScenesProjectFolder + "Init.unity");
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        

        private static void SetupLevelLoader()
        {
            //Creating Basic LevelLoader Scriptable
            string LevelDefDestination =  "Assets/" + GameProjectFolder + $"/{Application.productName}_LevelDefinitionScriptable.asset";
            AssetDatabase.CopyAsset("Assets/SpikyCore/GenericCore/DefaultLevelDefinitionScriptable.asset", LevelDefDestination);
            
            string LevelLoaderDestination = "Assets/" + GameProjectFolder + $"/{Application.productName}_LevelLoaderScriptable.asset";
            AssetDatabase.CopyAsset("Assets/SpikyCore/GenericCore/LevelLoaderScriptable.asset", LevelLoaderDestination);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            DefaultLevelLoaderScriptable levelLoader = AssetDatabase.LoadAssetAtPath<DefaultLevelLoaderScriptable>(LevelLoaderDestination);
            
            LevelDefinitionScriptable levelDef = AssetDatabase.LoadAssetAtPath<LevelDefinitionScriptable>(LevelDefDestination);
            levelDef.Clear();
            levelDef.AddLevel(new LevelDefinition(){sceneToLoad = "Gameplay"});
            
            levelLoader.SetLevelsDefinition(levelDef);
            EditorUtility.SetDirty(levelLoader);
            EditorUtility.SetDirty(levelDef);
            
            EditorSceneManager.OpenScene(ScenesProjectFolder + "Init.unity");
            
            DefaultLevelManager levelManager = GameObject.FindObjectOfType<DefaultLevelManager>();
            levelManager.SetLevelLoader(levelLoader);
            EditorUtility.SetDirty(levelManager);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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