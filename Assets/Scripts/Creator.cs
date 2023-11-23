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
    //Folder in prefabs
    public GameObject[] ObjPrefabs;

    private bool _isStartedLevelEditor = false;

    private int _selectedPrefabObjValue = 0;

    private Camera _mainCam;

    



    private List<LevelObj> _newLevelObj;
    private List<GameObject> _createdObj;

    private LevelData[] _levels;

    private GameObject _selectedObj;

    public LevelData SelectedLevel;

    
    private List<LevelObj> _levelObject;


    private List<Texture2D> _prefabTexture = new List<Texture2D>();


    private List<string> _loadedPrefabsDirs ;


    public int LevelIdx = 0;
    public List<String> LevelName;
    private RaycastHit _hit;




    private bool isLoadedTexture = false;

    private LoadFileFromDirectory _loadDirectory;

  
    public void Init(){
        _mainCam = Camera.main;
        _loadDirectory = new LoadFileFromDirectory();
    }
    private void OnGUI()
    {
        if (!_isStartedLevelEditor)
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
        if (Physics.Raycast(ray, out _hit, 100))
        {
            if (_hit.collider.CompareTag("Ground"))
            {
                if (SelectedLevel != null)
                {
                    MeshRenderer renderer = ObjPrefabs[_selectedPrefabObjValue].GetComponent<MeshRenderer>();
                    float y = renderer.bounds.size.y;
                    Vector3 objPosition = new Vector3(_hit.point.x, _hit.point.y + (y / 2), _hit.point.z);
                    ItemCreate(ObjPrefabs[_selectedPrefabObjValue], objPosition);
                    AddLevelObj(_selectedPrefabObjValue,objPosition);

                }
                else
                {
                    Debug.Log("Bir Level Olu�turun Veya Se�in");
                }

            }
            else
            {
                _selectedObj = _hit.collider.gameObject;
            }
        }
    }

    public void Create()
    {
        Debug.Log("Create");
        LevelData level = ScriptableObject.CreateInstance<LevelData>();
        _loadDirectory.CreateScriptableObjectInDirectory("LevelName",level);
        SelectedLevel = level;
        LevelIdx = _levels.Length-1;
        Search();
        
        
    }

    
    public void Clear()
    {
        for (int i = 0; i< _createdObj.Count; i++) {
            
            DestroyImmediate(_createdObj[i]);
            
        }
        _newLevelObj.Clear();
        _createdObj.Clear();
        Debug.Log("Clear");
        
    }


    public void Save()
    {
        SelectedLevel.ClearObjects();
        for (int i = 0; i < _createdObj.Count; i++)
        {
            SelectedLevel.AddNewObj(_newLevelObj[i]);
        }
        
        Debug.Log("Save");
    }

    private void AddLevelObj(int prefabValue, Vector3 pos)
    {
        if (_newLevelObj == null)
        {
            _newLevelObj = new List<LevelObj>();
        }
        LevelObj newLevel = new LevelObj();
        
        newLevel.Obj = _loadDirectory.LoadFromGameObject(_loadedPrefabsDirs[prefabValue]);//AssetDatabase.LoadAssetAtPath < GameObject > (_loadedPrefabsDirs[prefabValue]);
        newLevel.pos = pos;
        _newLevelObj.Add(newLevel);
    }
   

    public void Search()
    {
        if (LevelName.Count > 0)
        {
            LevelName.Clear();
        }
        _levels = Resources.LoadAll<LevelData>("Level");
        Debug.Log(_levels.Length);
        foreach (LevelData data in _levels)
        {
            LevelName.Add(data.name);
        }
        
    }

    public void Load()
    {
        for (int i = 0; i<SelectedLevel.objs.Count;i++)
        {
            ItemCreate(SelectedLevel.objs[i].Obj, SelectedLevel.objs[i].pos);
            _newLevelObj.Add(SelectedLevel.objs[i]);
        }
        Debug.Log("Load");
    }
    private void ItemCreate(GameObject prefab,Vector3 pos)
    {
        GameObject obj = Instantiate(prefab,pos,prefab.transform.rotation);
        
        _createdObj.Add(obj);

    }
    public void SelectLevel()
    {
        
        foreach (LevelData data in _levels)
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
        if (_selectedObj != null)
        {
            _createdObj.Remove(_selectedObj);
            for (int i = 0; i< _newLevelObj.Count;i++)
            {
                if (_newLevelObj[i].pos == _selectedObj.transform.position)
                {
                    _newLevelObj.RemoveAt(i);
                }
            }
            DestroyImmediate(_selectedObj);
            Debug.Log("_selectedObj Silindi");
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
        _isStartedLevelEditor = isActive;
    }
    public bool ReturnIsStartedBool()
    {
        return _isStartedLevelEditor;
    }
    public bool ReturnIsLoadedTextureBool()
    {
        return isLoadedTexture;
    }
    public List<Texture2D> loadTexture()
    {
        isLoadedTexture = true;
        //GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/");
        //string[] paths = new string[prefabs.Length];

        if (_prefabTexture != null)
        {
            _prefabTexture.Clear();
        }
        var dirs = Directory.EnumerateFiles("Assets/Resources/Prefabs/", "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".prefab"));
        if (dirs == null)
        {
            isLoadedTexture = false;
            Debug.LogError("folder is empty please add the prefabs this folder Resources/Prefabs");
        }
        foreach (string dir in dirs)
        {
            _prefabTexture.Add(GetPrefabPreview(dir));
            _loadedPrefabsDirs.Add(dir);
        }

        return _prefabTexture;
    }
    public bool LoadPrefabs()
    {
        
        ObjPrefabs = Resources.LoadAll<GameObject>("Prefabs");
        Debug.Log(ObjPrefabs is null);
        return ObjPrefabs != null;
    }


    private Texture2D GetPrefabPreview(string path)
    {
        Debug.Log("Generate preview for " + path);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        
        var editor = UnityEditor.Editor.CreateEditor(prefab);
        Texture2D tex = editor.RenderStaticPreview(path, null, 200, 200);
        EditorWindow.DestroyImmediate(editor);
        return tex;
    }
    public void SelectedPrefab(int a)
    {
        if (ObjPrefabs.Length == 0)
        {
            LoadPrefabs();
        }
        _selectedPrefabObjValue = a;
    }
}
#endif
