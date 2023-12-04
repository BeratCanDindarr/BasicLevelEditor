using System.Collections;
using System.Collections.Generic;
using BasicLevelEditor.CustomLevelEditor;
using UnityEngine;

public abstract class BaseInterakt : IInteraktable
{

    protected Creator _creator;
    protected Camera _mainCamera;

   public BaseInterakt( Creator creator,Camera camera){
    this._creator = creator;
    this._mainCamera = camera;
   }
    protected RaycastHit _hit;
    protected Ray _ray;



   public virtual void RayThrowing(){
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = Screen.height - mousePos.y;
        _ray = _mainCamera.ScreenPointToRay(mousePos);
        Physics.Raycast(_ray, out _hit, 100);
   }

   public virtual void MouseDown(){
    RayThrowing();
    if(!CheckTouches("Ground")){
      _creator.SelectedObject = _hit.collider.gameObject;
    }
    else{
      _creator.SelectedObject = null;
    }
   }
   public abstract void MouseDrag();
   public virtual void MouseUp(){
    
   }

   public bool CheckTouches(string touchesObjTag){
    bool success = true;
    if(!_hit.collider.CompareTag(touchesObjTag)){
      success = false;

    }

    return success;
   }
   protected Vector3 ReturnObjPos(){
      MeshRenderer renderer = _creator.ObjPrefabs[_creator.SelectedPrefabObjValue].GetComponent<MeshRenderer>();
      float y = renderer.bounds.size.y;
      Vector3 objLocalSize = _creator.ObjPrefabs[_creator.SelectedPrefabObjValue].transform.localScale;
      float x = Mathf.Floor(_hit.point.x);
      float z = Mathf.Floor(_hit.point.z);
      if(objLocalSize.x % 2 == 1 ){
        x+= 0.5f;
      }
      if(objLocalSize.z % 2 == 1){
        Debug.Log("Girdi");
        z+=0.5f;
      }
      
      return new Vector3(x, _hit.point.y + (y / 2), z);
   }

}
