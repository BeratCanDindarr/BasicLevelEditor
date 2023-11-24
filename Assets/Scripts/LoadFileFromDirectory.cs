using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using System.IO;
using BasicLevelEditor.CustomLevelEditor.Data;

namespace BasicLevelEditor.CustomLevelEditor.LoadFromFileSystem
{
    public class LoadFileFromDirectory : MonoBehaviour
    {
        const string resourcesDir = "Assets/Resources/";
        const string levelDir = "Assets/Resources/Level";
        const string prefabDir = "Assets/Resources/Prefabs";
        LevelData[] data;

        public void CreateScriptableObjectInDirectory(string fileName, LevelData levelData){
            if(CheckFileExists(levelDir)){

                string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Resources/Level/{fileName}.asset");
                AssetDatabase.CreateAsset(levelData, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        // public LevelData[] ResourcesLoadAll(string path){
            
        //     if(CheckFileExists(levelDir)){
        //         LevelData[] levelData = Resources.LoadAll<LevelData>(path);
                
        //         return levelData;
        //     }
        //     return null;

        // }

        public GameObject LoadFromGameObject(string path){
            GameObject gameObject = null;
            if(CheckFileExists(prefabDir)){
                gameObject =  AssetDatabase.LoadAssetAtPath < GameObject > (path);
            }
            return gameObject;
        }

        public T[] ResourcesLoadAll<T>(string path){
            
            if(CheckFileExists(resourcesDir+path)){
                data = Resources.LoadAll<LevelData>(path);
            }
            T[] result = (T[])Convert.ChangeType(data,typeof(T[]));
            return result;
            
        }

        public T[] test<T>(string testii){
            Debug.Log(testii);
            string[] deneme = new string[2];
            deneme[0] = testii;
            deneme[1] = "deneme";
            T[] result = (T[])Convert.ChangeType( deneme, typeof(T[]));
            Debug.Log(result);
            return result;
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
}
