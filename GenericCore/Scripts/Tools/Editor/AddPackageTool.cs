using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace SpikyCoreInitializer
{
    public class AddPackageTool
    {
        private const string SPRITE__2D_PACKAGE = "com.unity.2d.sprite";
        private const string RECORDER_PACKAGE = "com.unity.recorder";
        
        [MenuItem("Spiky Tools/Packages/Add 2D Sprite Package")]
        private static void Add2DSPritePackage()
        {
            CheckIfExistAndAddPackage(SPRITE__2D_PACKAGE);
        }
        
        [MenuItem("Spiky Tools/Packages/Add Recorder")]
        private static void AddRecorderPackage()
        {
            CheckIfExistAndAddPackage(RECORDER_PACKAGE);
        }

        private static async void CheckIfExistAndAddPackage(string package)
        {
            PackageExistResult checkIfExistPackage = await CheckIfList(package);

            if (checkIfExistPackage == PackageExistResult.Error)
            {
                return;
            }
            
            await AddPackageByName(package);
            Debug.Log("Finished.");
        }

        private static async Task AddPackageByName(string package)
        {
            AddRequest addRequests = Client.Add(package);
            while (!addRequests.IsCompleted)
            {
                await Task.Yield();
            }

            if (addRequests.Status == StatusCode.Success)
            {
                Debug.Log("Added: " + addRequests.Result.packageId + "... Installing...");
            }
            
            if (addRequests.Status >= StatusCode.Failure)
            {
                Debug.LogError(addRequests.Error.message);
            }
        }

        private enum PackageExistResult
        {
            No,
            Error,
        }

        private static async Task<PackageExistResult> CheckIfList(string packageName)
        {
            ListRequest getListRequest = Client.List();
            while (!getListRequest.IsCompleted)
            {
                await Task.Yield();
            }
            
            if (getListRequest.Status == StatusCode.Success)
            {
                bool exist = getListRequest.Result.Any(pck => pck.name == packageName);
                if (exist)
                {
                    Debug.LogError("Package Already Exists!");
                    return PackageExistResult.Error;
                }
                Debug.Log("Package is not present in project, Downloading...!");
                return PackageExistResult.No;
            }
            
            Debug.LogError("The List could not be downloaded!");
            return PackageExistResult.Error;
        }
    }
}