using System.Collections;
using System.Collections.Generic;
using BasicLevelEditor.CustomLevelEditor;
using BasicLevelEditor.CustomLevelEditor.Data;
using UnityEngine;

public class DrawInterakt : BaseInterakt
{
   public DrawInterakt(Creator creator,Camera camera) :base(creator,camera){}

   private bool _isDragging = false;

    public override void MouseDown()
    {
        
        CreateObj();
    }
     public override void MouseDrag()
    {
        CreateObj();
        _isDragging = true;
    }
    public override void MouseUp()
    {
        // if(_isDragging){
        //     var LevelObjects = _creator.CreatedObjects;
        //     List<int> ayni = new List<int>();
        //     List<LevelObj> newLevelObjects = new List<LevelObj>();
        //     for(int i = 0; i<LevelObjects.Count; i++){
        //         for(int j = LevelObjects.Count-1; j>i; j-- ){
                   
        //             if(LevelObjects[i].transform.position  == LevelObjects[j].transform.position){
        //                _creator.TestDeleteSelectedObj();
        //             }
        //         }
        //     }

           
        //     _creator.CreatedObjects = LevelObjects;

            

        // }
    }

    private void CreateObj(){
        base.RayThrowing();
        if(CheckTouches("Ground")){

            if(_creator.SelectedLevel != null){
                    Vector3 objPosition = ReturnObjPos();
                    _creator.ItemCreate(_creator.ObjPrefabs[_creator.SelectedPrefabObjValue], objPosition);
                    _creator.AddLevelObj(_creator.SelectedPrefabObjValue,objPosition);
                }
                else{
                    Debug.Log("Bir Level Olu�turun Veya Se�in");
                }
        }
    }
}
