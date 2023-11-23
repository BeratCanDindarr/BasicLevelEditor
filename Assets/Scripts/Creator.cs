using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Diagnostics;
using System.IO;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using Unity.VisualScripting;

#if UNITY_EDITOR
[ExecuteInEditMode()]
public class Creator : MonoBehaviour
{
    private bool isStartedLevelEditor = false;

    public Texture2D test2D;

    public GameObject SquareGameObject;
    public GameObject[] ObjPrefabs;
    private int selectedPrefabObjValue = 0;


    public GameObject[] test;
    private Camera _mainCam;

    public Mesh mesh;

    public List<LevelObj> newLevelObj;

    public List<GameObject> CreatedObj;

    public LevelData[] levels;

    public GameObject SelectedObj;

    [SerializeField]public LevelData SelectedLevel;

    [SerializeField]
    private List<LevelObj> _levelObject;


    List<Texture2D> PrefabTexture = new List<Texture2D>();


    List<string> LoadedPrefabsDirs;


    public int LevelIdx = 0;
    public List<String> LevelName;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;
        Debug.Log(SquareGameObject.transform.localScale.y);
    }
    private void OnGUI()
    {
        if (!isStartedLevelEditor)
        {
            return;
        }
        if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
        else if (Event.current.type == EventType.MouseDown)
        {
            DoMouseDown();
        }
    }

    private void DoMouseDown()
    {
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                if (SelectedLevel != null)
                {
                    MeshRenderer renderer = ObjPrefabs[selectedPrefabObjValue].GetComponent<MeshRenderer>();
                    float y = renderer.bounds.size.y;
                    Vector3 objPosition = new Vector3(hit.point.x, hit.point.y + (y / 2), hit.point.z);
                    ItemCreate(ObjPrefabs[selectedPrefabObjValue], objPosition);
                    AddLevelObj(selectedPrefabObjValue,objPosition);

                }
                else
                {
                    Debug.Log("Bir Level Oluþturun Veya Seçin");
                }

            }
            else
            {
                SelectedObj = hit.collider.gameObject;
            }
        }
    }

    public void Create()
    {
        Debug.Log("Create");
        LevelData level = ScriptableObject.CreateInstance<LevelData>();
        string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Level/LevelName.asset");
        AssetDatabase.CreateAsset(level, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        SelectedLevel = level;
        Search();
        for (int i = 0; i < LevelName.Count; i++)
        {
            if (LevelName[i] == level.name)
            {
                LevelIdx = i;
            }
        }
        
    }

    
    public void Clear()
    {
        for (int i = 0; i< CreatedObj.Count; i++) {
            
            DestroyImmediate(CreatedObj[i]);
            
        }
        newLevelObj.Clear();
        CreatedObj.Clear();
        Debug.Log("Clear");
        
    }


    public void Save()
    {
        
        for (int i = 0; i < CreatedObj.Count; i++)
        {
            //LevelObj newLevel = new LevelObj();
            //newLevel.Obj = CreatedObj[i];
            //newLevel.pos = CreatedObj[i].transform.position;
            SelectedLevel.AddNewObj(newLevelObj[i]);
        }
        
        Debug.Log("Save");
    }

    private void AddLevelObj(int prefabValue, Vector3 pos)
    {
        if (newLevelObj == null)
        {
            newLevelObj = new List<LevelObj>();
        }
        LevelObj newLevel = new LevelObj();
        
        newLevel.Obj = AssetDatabase.LoadAssetAtPath < GameObject > (LoadedPrefabsDirs[prefabValue]);
        newLevel.pos = pos;
        newLevelObj.Add(newLevel);
    }
    public void Search()
    {
        



        if (LevelName.Count > 0)
        {
            LevelName.Clear();
        }
        if (LevelName.Count == 0)
        {
            LevelName.Add(null);
        }
        levels = Resources.LoadAll<LevelData>("Level/");
        foreach (LevelData data in levels)
        {
            LevelName.Add(data.name);
        }
    }

    public void Load()
    {
        for (int i = 0; i<SelectedLevel.objs.Count;i++)
        {
            ItemCreate(SelectedLevel.objs[i].Obj, SelectedLevel.objs[i].pos);
        }
        Debug.Log("Load");
    }
    private void ItemCreate(GameObject prefab,Vector3 pos)
    {
        GameObject obj = Instantiate(prefab,pos,prefab.transform.rotation);
        CreatedObj.Add(obj);

    }
    public void SelectLevel()
    {
        
        foreach (LevelData data in levels)
        {
            if (data.name == LevelName[LevelIdx])
            {
                Debug.Log("Selected Level" + data.name);
                SelectedLevel = data;

            }
        }
        //Debug.Log("Select level");
    }
    public void DeleteSelectedObj()
    {
        if (SelectedObj != null)
        {
            CreatedObj.Remove(SelectedObj);
            for (int i = 0; i< newLevelObj.Count;i++)
            {
                if (newLevelObj[i].pos == SelectedObj.transform.position)
                {
                    newLevelObj.RemoveAt(i);
                }
            }
            DestroyImmediate(SelectedObj);
            Debug.Log("SelectedObj Silindi");
        }
        else
        {
            Debug.Log("Secili obje yok");
        }
    }
    public void ClearLevel()
    {
        SelectedLevel.ClearObjects();
        Clear();
    }

    public void ChangeStartedLevelEditor(bool isActive)
    {
        isStartedLevelEditor = isActive;
    }
    public bool ReturnIsStartedBool()
    {
        return isStartedLevelEditor;
    }
    public List<Texture2D> loadTexture()
    {
        //GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/");
        //string[] paths = new string[prefabs.Length];

        if (PrefabTexture != null)
        {
            PrefabTexture.Clear();
        }
        var dirs = Directory.EnumerateFiles("Assets/Resources/Prefabs/", "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".prefab"));

        foreach (string dir in dirs)
        {
            PrefabTexture.Add(GetPrefabPreview(dir));
            LoadedPrefabsDirs.Add(dir);
        }

        return PrefabTexture;
    }
    public void LoadPrefabs()
    {
        ObjPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        Debug.Log("Load Prefabs");
    }


    static Texture2D GetPrefabPreview(string path)
    {
        Debug.Log("Generate preview for " + path);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        
        var editor = UnityEditor.Editor.CreateEditor(prefab);
        Texture2D tex = editor.RenderStaticPreview(path, null, 200, 200);
        EditorWindow.DestroyImmediate(editor);
        return tex;
    }
    public void testii(int a)
    {
        if (ObjPrefabs.Length == 0)
        {
            LoadPrefabs();
        }

        SquareGameObject = ObjPrefabs[a];
        selectedPrefabObjValue = a;
        Debug.Log(a);
    }
}
#endif
