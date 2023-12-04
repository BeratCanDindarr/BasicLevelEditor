using System.Collections;
using System.Collections.Generic;
using BasicLevelEditor.CustomLevelEditor;
using UnityEngine;

public class SelectInterakt : BaseInterakt
{
    public SelectInterakt(Creator creator,Camera camera) : base(creator,camera){}

    private bool _isDragging = false;
    private GameObject _selectedObject = null;

    public override void MouseDown()
    {
        base.RayThrowing();
        if(!CheckTouches("Ground")){
            _selectedObject = _hit.collider.gameObject;
            _creator.SelectedObject = _selectedObject;

            _isDragging = true;
        }
    }
    public override void MouseDrag()
    {
        base.RayThrowing();
        if(CheckTouches("Ground") && _isDragging){
            _creator.TestDeleteSelectedObj();
            Vector3 objPos = base.ReturnObjPos();
            _selectedObject.transform.position = objPos;
            _creator.AddLevelObj(_creator.SelectedPrefabObjValue,objPos);
        }
    }
      public override void MouseUp(){
        _isDragging = false;
        
    }
}
