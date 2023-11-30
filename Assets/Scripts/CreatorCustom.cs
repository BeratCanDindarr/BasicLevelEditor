using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
namespace BasicLevelEditor.CustomLevelEditor{

    [CustomEditor(typeof(Creator))]

    public class CreatorCustom : Editor
    {
        //Base Script

        private Creator _creator;

        private bool isStartedLevelEditor = false;
        private bool isLoadedTexture = false;
        private bool _showPrefabButton = false;

        //Loaded 3D model texture
        private List<Texture2D> _textureArray;




    
        
        public override void OnInspectorGUI()
        {
                base.OnInspectorGUI();

            _creator = (Creator)target;
            isStartedLevelEditor = _creator.ReturnIsStartedBool();
            if (!isStartedLevelEditor)
            {
                if (GUILayout.Button("Start Level Editor"))
                {


                    _creator.Init();
                    _creator.Search();
                    isStartedLevelEditor = true;
                    _creator.ChangeStartedLevelEditor(isStartedLevelEditor);
                    _textureArray = _creator.loadTexture();
                    _creator.LoadPrefabs();


                }
                return;
            }

        
            
            GUIContent ChangeAbleLevelName = new GUIContent("LevelName");

            _creator.LevelIdx = EditorGUILayout.Popup(ChangeAbleLevelName, _creator.LevelIdx, _creator.LevelName.ToArray());
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Selected Level: " + _creator.SelectedLevel.ToString());
            if (GUILayout.Button("Select Level"))
            {
                _creator.SelectLevel();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Create Level"))
            {
                _creator.Create();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Level", GUILayout.Width(200)))
            {
                _creator.Save();
            }
            if (GUILayout.Button("Load Level", GUILayout.Width(200)))
            {
                _creator.Load();
            }
            if (GUILayout.Button("Clear Level Data", GUILayout.Width(200)))
            {
                _creator.ClearLevel();
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space(20);

            
            
        
            if (GUILayout.Button("Load Prefab "))
            {
                    
                _textureArray = _creator.loadTexture();
            }
                
            
            
            
            _showPrefabButton = EditorGUILayout.Foldout(_showPrefabButton, "Game Prefabs");
            if (_showPrefabButton)
            {
                    int arrayLenght = _textureArray.Count;
                    int a = arrayLenght / 3;
                    int b = arrayLenght % 3;
                    int p = 0;
                for (int i = 0; i < a; i++)
                {
                    GUILayout.BeginHorizontal();
                    for (int j = 0; j < 3; j++)
                    {
                        var _content = new GUIContent(" ", _textureArray[p]); 
                        if (GUILayout.Button(_content, GUILayout.Width(100)))
                        {
                            _creator.SelectedPrefab(p);
                        }
                        p++;
                    }
                    GUILayout.EndHorizontal();



                }
                        GUILayout.BeginHorizontal();
                    for (int j = 0;j < b; j++)
                    {
                        var _content = new GUIContent(" ", _textureArray[p]);
                        if (GUILayout.Button(_content, GUILayout.Width(100)))
                        {
                            _creator.SelectedPrefab(p);
                        }
                        p++;
                    }
                        GUILayout.EndHorizontal();

            }



            EditorGUILayout.Space(20);
            // if (GUILayout.Button("Search Level",GUILayout.Width(200)))
            // {
            //     _creator.Search();
            // }
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Scene Clear", GUILayout.Width(200)))
            {
                _creator.Clear();
            }
            
        
            if (GUILayout.Button("Delete Selected Obj", GUILayout.Width(200)))
            {
                _creator.DeleteSelectedObj();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Stop Level Editor"))
            {
                isStartedLevelEditor = false;
                _creator.ChangeStartedLevelEditor(isStartedLevelEditor);
            }

        }

        
    
    }
}
