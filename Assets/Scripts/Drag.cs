using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    //需要对应Unity设置里的layer
    enum DragPriority
    {
        SceneObj = 8,
        FakeUI = 9,
    }

    void FixedUpdate()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Camera mainCamera = Camera.main;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                                  mainCamera.ScreenPointToRay(Input.mousePosition).direction, 100,
                                  Physics.DefaultRaycastLayers);

        int minDragableLayer = (int)DragPriority.SceneObj;
        int dragIndex = -1, minLayer = int.MaxValue;
        for (int i = 0; i < hits.Length; ++i)
        {
            GameObject obj = hits[i].collider.gameObject;
            //在允许拖拽的layer中，选择最小的那个
            if (obj.layer >= minDragableLayer && obj.layer < minLayer)
            {
                minLayer = obj.layer;
                dragIndex = i;
            }
        }
        if (dragIndex > -1)
        {
            GameObject dragObj = hits[dragIndex].collider.gameObject;
            IDragable dragable = dragObj.GetComponent<IDragable>();
            if (dragable != null)
                dragable.Drag();
        }
    }
}
