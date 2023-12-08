using System.Collections;
using System.Collections.Generic;
using BasicLevelEditor.CustomLevelEditor;
using UnityEngine;

public class DeleteInterakt : BaseInterakt
{
    public DeleteInterakt(Creator creator, Camera camera) : base(creator,camera){}

    public override void MouseDown()
    {
        DeleteObject();
    }
    public override void MouseDrag()
    {
        DeleteObject();
    }

    private void DeleteObject(){
        base.RayThrowing();
        if(!CheckTouches("Ground")){
            _creator.SelectedObject = _hit.collider?.gameObject;
            _creator.DeleteSelectedObj();
        }
    }
}
