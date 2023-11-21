using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Level Data

[CreateAssetMenu(menuName ="Create/Create Level",fileName ="Level")]
public class LevelData : ScriptableObject
{


    
    public List<LevelObj> objs;


    public void AddNewObj(LevelObj levelObj)
    {
        if (objs== null)
        {
            CreateList();
        }
        objs.Add(levelObj);
    }
    public void ClearObjects()
    {
        if (objs != null)
        {
            objs.Clear();
        }
    }
    private void CreateList()
    {
        objs = new List<LevelObj>();
    }
}


    [Serializable]
    public class LevelObj
    {
        public GameObject Obj;
        public Vector3 pos; 
    }