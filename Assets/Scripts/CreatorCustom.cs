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
                //base.OnInspectorGUI();

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
                    _creator.SetIntegrationMode(INTEGRATION_MODE.NULL);


                }
                return;
            }


            GUILayout.BeginHorizontal();
            if(GUILayout.Button("SELECT",GUILayout.Width(50),GUILayout.Height(50))){
                _creator.SetIntegrationMode(INTEGRATION_MODE.SELECT);
            }
            if(GUILayout.Button("ROTATE",GUILayout.Width(50),GUILayout.Height(50))){
                _creator.SetIntegrationMode(INTEGRATION_MODE.ROTATE);
            }
            if(GUILayout.Button("DRAW",GUILayout.Width(50),GUILayout.Height(50))){
                _creator.SetIntegrationMode(INTEGRATION_MODE.DRAW);
            }
            if(GUILayout.Button("DELETE",GUILayout.Width(50),GUILayout.Height(50))){
                _creator.SetIntegrationMode(INTEGRATION_MODE.DELETE);
            }

            
            GUILayout.EndHorizontal();
            GUILayout.Label("Selected Mode: " + _creator._selectedMode.ToString());
            EditorGUILayout.Space(10);

            //Level Data
            GUIContent ChangeAbleLevelName = new GUIContent("LevelName");

            _creator.LevelIdx = EditorGUILayout.Popup(ChangeAbleLevelName, _creator.LevelIdx, _creator.LevelName.ToArray());
            EditorGUILayout.BeginHorizontal();
            
            if(_creator.SelectedLevel != null){
                EditorGUILayout.LabelField("Selected Level: " + _creator.SelectedLevel.name);

            }else{
                EditorGUILayout.LabelField("Selected Level:  NULL ");
            }
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
            if (GUILayout.Button("Save Level"))
            {
                _creator.Save();
            }
            if (GUILayout.Button("Load Level"))
            {
                _creator.Load();
            }
            if (GUILayout.Button("Clear Level Data"))
            {
                _creator.ClearLevel();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Scene Clear"))
            {
                _creator.Clear();
            }

            EditorGUILayout.Space(50);
            

            //Selected Mode
            if(_creator._selectedMode == INTEGRATION_MODE.SELECT){
                EditorGUILayout.ObjectField(_creator.SelectedObject,typeof(GameObject),true);
            }

            //Rotate Mode
            if(_creator._selectedMode == INTEGRATION_MODE.ROTATE){

            }
            //Draw Mode
            if(_creator._selectedMode == INTEGRATION_MODE.DRAW){

                   


                if (GUILayout.Button("Load Prefab "))
                {
                        
                    _textureArray = _creator.loadTexture();
                }


                //select Draw Items
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





            }
            //Delete Mode
            if(_creator._selectedMode == INTEGRATION_MODE.DELETE){

            }

            


            EditorGUILayout.Space(20);

            // if (GUILayout.Button("Search Level",GUILayout.Width(200)))
            // {
            //     _creator.Search();
            // }
            
            
            
        
           
            
            if (GUILayout.Button("Stop Level Editor"))
            {
                isStartedLevelEditor = false;
                _creator.ChangeStartedLevelEditor(isStartedLevelEditor);
            }

        }
        
        
    
    }
}
