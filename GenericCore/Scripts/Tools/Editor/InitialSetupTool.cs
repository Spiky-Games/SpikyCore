using UnityEditor;
using UnityEngine;

public class InitialSetupTool : EditorWindow
{
    private const string COMPANY_NAME = "spiky";

    [MenuItem("Spiky Tools/Initial Setup")]
    private static void StartInitialSetupTool()
    {
        PlayerSettings.companyName = COMPANY_NAME;
        PlayerSettings.accelerometerFrequency = 0;
        PlayerSettings.applicationIdentifier = "";
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.allowedAutorotateToLandscapeRight = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
        PlayerSettings.gpuSkinning = true;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;

        PlayerSettings.SplashScreenLogo[] logos = new PlayerSettings.SplashScreenLogo[1];
        Sprite companyLogo = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/SpikyCore/GenericCore/Art/Spiky_Games_Logo.png", typeof(Sprite));
        logos[0] = PlayerSettings.SplashScreenLogo.Create(2, companyLogo);
        PlayerSettings.SplashScreen.logos = logos;
    }
}
