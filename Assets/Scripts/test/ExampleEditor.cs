// Render the provided asset texture into an Inspector thumbnail.
using UnityEngine;
using System.Collections;
using UnityEditor;

using System.IO;

public class Example : ScriptableObject
{
    public Texture2D PreviewIcon;
}


[CustomEditor(typeof(Example))]
public class ExampleEditor : UnityEditor.Editor
{
    public static void CreateAsset<Example>() where Example : ScriptableObject
    {
        Example asset = ScriptableObject.CreateInstance<Example>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(Example).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Examples/RenderStaticPreview example")]
    public static void CreateAsset()
    {
        CreateAsset<Example>();
    }

    public override void OnInspectorGUI()
    {
        Example e = (Example)target;

        EditorGUI.BeginChangeCheck();

        // Example has a single arg called PreviewIcon which is a Texture2D
        e.PreviewIcon = (Texture2D)
                EditorGUILayout.ObjectField(
                    "Thumbnail",                    // string
                    e.PreviewIcon,                  // Texture2D
                    typeof(Texture2D),              // Texture2D object, of course
                    false                           // allowSceneObjects
                );

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(e);
            AssetDatabase.SaveAssets();
            Repaint();
        }
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Example example = (Example)target;

        if (example == null || example.PreviewIcon == null)
            return null;

        // example.PreviewIcon must be a supported format: ARGB32, RGBA32, RGB24,
        // Alpha8 or one of float formats
        Texture2D tex = new Texture2D(width, height);
        EditorUtility.CopySerialized(example.PreviewIcon, tex);

        return tex;
    }
}