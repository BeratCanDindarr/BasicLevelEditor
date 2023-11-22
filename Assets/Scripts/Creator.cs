using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Diagnostics;
using System.IO;
using System;

#if UNITY_EDITOR
[ExecuteInEditMode()]
public class Creator : MonoBehaviour
{
    private bool isStartedLevelEditor = false;



    public GameObject SquareGameObject;
    public GameObject[] test;
    private Camera _mainCam;

    public Mesh mesh;
    

    public List<GameObject> CreatedObj;

    public LevelData[] levels;

    public GameObject SelectedObj;

    [SerializeField]public LevelData SelectedLevel;

    [SerializeField]
    private List<LevelObj> _levelObject;

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
                    Vector3 objPosition = new Vector3(hit.point.x, hit.point.y + (SquareGameObject.transform.localScale.y / 2), hit.point.z);
                    ItemCreate(SquareGameObject,objPosition);

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
        //string path = "Assets/Level/LevelName.asset";
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
        CreatedObj.Clear();
        Debug.Log("Clear");
        
    }


    public void Save()
    {
        for (int i = 0; i < CreatedObj.Count; i++)
        {
            LevelObj newLevel = new LevelObj();
            newLevel.Obj = SquareGameObject;
            newLevel.pos = CreatedObj[i].transform.position;
            SelectedLevel.AddNewObj(newLevel);
        }
        
        Debug.Log("Save");
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
            ItemCreate(SquareGameObject, SelectedLevel.objs[i].pos);
        }
        Debug.Log("Load");
    }
    private void ItemCreate(GameObject prefab,Vector3 pos)
    {
        GameObject obj = Instantiate(prefab,pos,transform.rotation);
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
        Debug.Log("Select level");
    }
    public void DeleteSelectedObj()
    {
        if (SelectedObj != null)
        {
            CreatedObj.Remove(SelectedObj);
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
}
#endif
