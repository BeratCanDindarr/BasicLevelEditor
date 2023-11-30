using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using BasicLevelEditor.CustomLevelEditor.Data;
using BasicLevelEditor.CustomLevelEditor.LoadFromFileSystem;
using Unity.VisualScripting;



namespace BasicLevelEditor.CustomLevelEditor{


    #if UNITY_EDITOR
    [ExecuteInEditMode()]
    public class Creator : MonoBehaviour
    {

        //Main Camera
        private Camera _mainCam;

        #region  Prefabs Data

        //Loaded Prefabs container
        public GameObject[] ObjPrefabs;


        //Created Gameobjects in scene
        private List<GameObject> _createdObj;
        //Clicked Obj holder
        private GameObject _selectedObj;

        //Selected obj value for prefabs loading
        private int _selectedPrefabObjValue = 0;
        #endregion

        #region  Texture for GameObjects
        //Loaded Gameobjecst hold textures in container
        private List<Texture2D> _prefabTexture = new List<Texture2D>();
        #endregion


        //loaded Gameobjects hold dirs in container
        private List<string> _loadedPrefabsDirs ;


        #region  Level Data
        //Created Objects  hold pos and prefab in container
        private List<LevelObj> _newLevelObj;

        // Levels Data container
        private LevelData[] _levels;
        //Selected Level data Holder
        public LevelData SelectedLevel;

        //Selected Level ID
        public int LevelIdx = 0;
        //All levels name holder
        public List<String> LevelName;
        #endregion



        private   bool _isStartedLevelEditor = false;

        private bool isLoadedTexture = false;


        







        
        





        #region  Raycast Parameter
        private RaycastHit _hit;
        #endregion

        #region  Load File From Directory
        private LoadFileFromDirectory _loadDirectory;
        #endregion




    
        public void Init(){
            _mainCam = Camera.main;
            _loadDirectory = new LoadFileFromDirectory();
            _isStartedLevelEditor = false;
            _createdObj = new List<GameObject>();
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
                        float x = Mathf.Floor(_hit.point.x) + 0.5f;
                        float z = Mathf.Floor(_hit.point.z) + 0.5f;
                        Vector3 objPosition = new Vector3(x, _hit.point.y + (y / 2), z);
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
            level.LevelID = _levels.Length;
            SelectedLevel = level;
            LevelIdx = _levels.Length;
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

            // LevelData denemeeeee = new LevelData();
            // _levels= _loadDirectory.ResourcesLoadAll<LevelData>("Level");
            
            //_loadDirectory = new LoadFileFromDirectory();
            // string[] test = _loadDirectory.test<string>("Naber");
            // Debug.Log(test[0]);
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
            if (LevelName.Count != 0)
            {
                LevelName.Clear();
            }
            //LevelData[] newLevels = _loadDirectory.ResourcesLoadAll<LevelData>("Level");

            //_levels= _loadDirectory.ResourcesLoadAll("Level");//Resources.LoadAll<LevelData>("Level");
            
            LevelData[] newLevels = _loadDirectory.ResourcesLoadAll<LevelData>("Level");
            //_levels = new LevelData[newLevels.Length];
            _levels = newLevels.OrderBy(go => go.LevelID).ToArray();
            // SelectedLevel = null;
            // LevelIdx = 0;
            Debug.Log(_levels.Length);
             foreach (LevelData data in _levels)
             {
                 if(data == null){
                     Debug.Log("Data Obj is null");
                 }
                
                 LevelName.Add(data.name);
             }
            // for(int i = 0;i < newLevels.Length; i++){
            //     if(newLevels[i] == null){
            //         Debug.Log("Data Obj is null");
            //     }
            //     _levels[i] = newLevels[i];
            //     LevelName.Add(newLevels[i].name);
            // }
            
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
            if(_loadedPrefabsDirs  == null   ){
                _loadedPrefabsDirs = new List<string>();
            }
            foreach (string dir in dirs)
            {
                _loadedPrefabsDirs.Add(dir);
                _prefabTexture.Add(GetPrefabPreview(dir));
            }

            return _prefabTexture;
        }
        public bool LoadPrefabs()
        {
            
            ObjPrefabs = Resources.LoadAll<GameObject>("Prefabs");
            Debug.Log(ObjPrefabs is null);
            return ObjPrefabs != null;
        }


        public static Texture2D GetPrefabPreview(string path)
        {
            Debug.Log("Generate preview for " + path);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            var editor = UnityEditor.Editor.CreateEditor(prefab);
            Texture2D tex = editor.RenderStaticPreview(path, null, 200, 200);
            EditorWindow.DestroyImmediate(editor);
            
            if(tex == null){
                
                Debug.Log("Texture Not initialize");
            }
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
}