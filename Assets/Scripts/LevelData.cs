using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicLevelEditor.CustomLevelEditor.Data{

    //Level Data

    [CreateAssetMenu(menuName ="Create/Create Level",fileName ="Level")]
    public class LevelData : ScriptableObject
    {

        public int LevelID;
        //Container that holds the position and type of created game objects
        public List<LevelObj> objs;
        
        //Code that assigns the created object to the object
        public void AddNewObj(LevelObj levelObj)
        {
            if (objs== null)
            {
                CreateList();
            }
            objs.Add(levelObj);
        }

        //Delete all objects
        public void ClearObjects()
        {
            if (objs != null)
            {
                objs.Clear();
            }
        }

        //when list is empty, this code running  
        private void CreateList()
        {
            objs = new List<LevelObj>();
        }
    }

        //Data that holds the position and type of created game objects
        [Serializable]
        public class LevelObj
        {
            public GameObject Obj;
            public Vector3 pos; 
        }   
}