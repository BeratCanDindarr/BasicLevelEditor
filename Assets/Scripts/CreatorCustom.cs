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





    

    private void OnEnable()
    {
        creator = (Creator)target;
        isStartedLevelEditor = creator.ReturnIsStartedBool();
    }
    
    public override void OnInspectorGUI()
    {
        if (!isStartedLevelEditor)
        {
            if (GUILayout.Button("Start Level Editor"))
            {
                isStartedLevelEditor = true;
                creator.ChangeStartedLevelEditor(isStartedLevelEditor);
            }
            return;
        }
        base.OnInspectorGUI();


        //EditorGUI.BeginChangeCheck();

        ////creator.SquareGameObject = (GameObject)EditorGUILayout.ObjectField(creator.SquareGameObject, typeof(GameObject), true);


        //if (EditorGUI.EndChangeCheck())
        //{
        //    if (gameObjectEditor != null) DestroyImmediate(this);
        //}

        //GUIStyle bgColor = new GUIStyle();

        ////bgColor.normal.background = ;

        //if (creator.SquareGameObject != null)
        //{
        //    if (gameObjectEditor == null)

        //        gameObjectEditor = Editor.CreateEditor(creator.SquareGameObject);
        //    gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(200, 200), bgColor);
        //}

        //preview.mesh = creator.SquareGameObject.GetComponent<MeshFilter>().sharedMesh;
        //var rect = GUILayoutUtility.GetRect(1, 200);
        //preview.OnPreviewGUI(rect, "TextField");

        //preview.OnPreviewSettings();

        //var property = serializedObject.FindProperty("test");
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(property,true);
        //serializedObject.ApplyModifiedProperties();
        for (int i = 0; i < 3; i++)
        {
            var _content = new GUIContent("Click me", creator.loadTexture()); // file name in the resources folder without the (.png) extension

            if (GUILayout.Button(_content))
            {
                creator.testii(i);
            }
            

        }



        int size = EditorGUILayout.IntField("object size", creator.test.Length);
        if (creator.test != null && size != creator.test.Length)
            creator.test = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            creator.test[i] = EditorGUILayout.ObjectField("Object " + i.ToString(), creator.test[i], typeof(GameObject), false,GUILayout.MinWidth(50),GUILayout.MinHeight(50)) as GameObject;
        }

        GUIContent ChangeAbleLevelName = new GUIContent("LevelName");
        creator.LevelIdx = EditorGUILayout.Popup(ChangeAbleLevelName,creator.LevelIdx,creator.LevelName.ToArray());
        if (GUILayout.Button("Test Level", GUILayout.Width(200)))
        {
            creator.loadTexture();
        }
        if (GUILayout.Button("Select Level", GUILayout.Width(200)))
        {
            creator.SelectLevel();
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
