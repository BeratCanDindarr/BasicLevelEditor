using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.PackageManager.UI;
using System.IO;

public class LoadFileFromDirectory : MonoBehaviour
{
    const string resourcesDir = "Assets/Resources/";
    const string levelDir = "Assets/Resources/Level";
    const string prefabDir = "Assets/Resources/Prefabs";

    public void CreateScriptableObjectInDirectory(string fileName, LevelData levelData){
        if(CheckFileExists(levelDir)){

            string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Level/{fileName}.asset");
            AssetDatabase.CreateAsset(levelData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    public LevelData[] ResourcesLoadAll(string path){
        
        if(CheckFileExists(levelDir)){
            LevelData[] levelData = Resources.LoadAll<LevelData>(path);
            
            return levelData;
        }
        return null;

    }

    public GameObject LoadFromGameObject(string path){
        GameObject gameObject = null;
        if(CheckFileExists(prefabDir)){
            gameObject =  AssetDatabase.LoadAssetAtPath < GameObject > (path);
        }
        return gameObject;
    }

    private bool CheckFileExists(string filePath){
        bool success = true;
        if(!Directory.Exists(filePath)){
            success = false;
            Debug.LogError("Create File " + filePath);
        }
        
        return success;
    }
}
