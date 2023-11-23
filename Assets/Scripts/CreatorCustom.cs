using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Creator))]
public class CreatorCustom : Editor
{
    Creator creator;
    Editor gameObjectEditor;
    private PreviewRenderUtility previewRenderUtility;
    private UnityEditor.MeshPreview preview;

    private bool isStartedLevelEditor = false;

    private bool _showPrefabButton = false;

    List<Texture2D> textureArray;




    private void OnEnable()
    {
        creator = (Creator)target;
        
    }
    
    public override void OnInspectorGUI()
    {
        isStartedLevelEditor = creator.ReturnIsStartedBool();
        if (!isStartedLevelEditor)
        {
            if (GUILayout.Button("Start Level Editor"))
            {


                isStartedLevelEditor = true;
                creator.ChangeStartedLevelEditor(isStartedLevelEditor);



                textureArray = creator.loadTexture();
            }
            return;
        }
        base.OnInspectorGUI();




        //Level Select Panel
        GUIContent ChangeAbleLevelName = new GUIContent("LevelName");
        creator.LevelIdx = EditorGUILayout.Popup(ChangeAbleLevelName, creator.LevelIdx, creator.LevelName.ToArray());
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(creator.SelectedLevel.ToString());
        if (GUILayout.Button("Select Level"))
        {
            creator.SelectLevel();
        }
        EditorGUILayout.EndHorizontal();



        _showPrefabButton = EditorGUILayout.Foldout(_showPrefabButton, "Game Prefabs");
        if (_showPrefabButton)
        {
                int arrayLenght = textureArray.Count;
                int a = arrayLenght / 3;
                int b = arrayLenght % 3;
                int p = 0;
            for (int i = 0; i < a; i++)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < 3; j++)
                {
                    var _content = new GUIContent("buton ", textureArray[p]); // file name in the resources folder without the (.png) extension
                    if (GUILayout.Button(_content, GUILayout.Width(100)))
                    {
                        creator.testii(p);
                    }
                    p++;
                }
                GUILayout.EndHorizontal();



            }
                    GUILayout.BeginHorizontal();
                for (int j = 0;j < b; j++)
                {
                    var _content = new GUIContent("buton ", textureArray[p]);
                    if (GUILayout.Button(_content, GUILayout.Width(100)))
                    {
                        creator.testii(p);
                    }
                    p++;
                }
                    GUILayout.EndHorizontal();

        }


        //int size = EditorGUILayout.IntField("object size", creator.test.Length);
        //if (creator.test != null && size != creator.test.Length)
        //    creator.test = new GameObject[size];
        //for (int i = 0; i < size; i++)
        //{
        //    creator.test[i] = EditorGUILayout.ObjectField("Object " + i.ToString(), creator.test[i], typeof(GameObject), false, GUILayout.MinWidth(50), GUILayout.MinHeight(50)) as GameObject;
        //}
        
        

        if (GUILayout.Button("Load Texture ", GUILayout.Width(200)))
        {
            creator.loadTexture();
        }
        if (GUILayout.Button("Search Level",GUILayout.Width(200)))
        {
            creator.Search();
        }
        if (GUILayout.Button("Create Level", GUILayout.Width(200)))
        {
            creator.Create();
        }
        if (GUILayout.Button("Scene Clear", GUILayout.Width(200)))
        {
            creator.Clear();
        }
        if (GUILayout.Button("Save Level", GUILayout.Width(200)))
        {
            creator.Save();
        }
        if (GUILayout.Button("Load Level", GUILayout.Width(200)))
        {
            creator.Load();
        }
        if (GUILayout.Button("Delete Selected Obj", GUILayout.Width(200)))
        {
            creator.DeleteSelectedObj();
        }
        if (GUILayout.Button("Clear Level Obj", GUILayout.Width(200)))
        {
            creator.ClearLevel();
        }
        if (GUILayout.Button("Stop Level Editor"))
        {
            isStartedLevelEditor = false;
            creator.ChangeStartedLevelEditor(isStartedLevelEditor);
        }

    }

    
   
}
