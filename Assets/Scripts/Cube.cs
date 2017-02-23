using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour,IDragable
{
    Material _mat;
    private void Awake()
    {
        try
        {
            _mat = this.GetComponent<MeshRenderer>().material;
        }
        catch { Debug.LogErrorFormat("Can not get Material on ", this.gameObject.name); }
    }

    IEnumerator DragCube()
    {
        ChangeAlpha(.3f);
        while (Input.GetMouseButton(0))
        {
            Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);
            transform.position = new Vector3(curPosition.x, transform.position.y, curPosition.z);

            yield return new WaitForFixedUpdate();
        }
        ChangeAlpha(1.0f);
    }

    void ChangeAlpha(float alpha)
    {
        try
        {
            Color color = _mat.GetColor("_Color");
            color.a = alpha;
            _mat.SetColor("_Color", color);
        }
        catch { Debug.LogErrorFormat("Shader without '_Color',but still try to get it! On Object ", this.gameObject.name); }
    }

    public void Drag()
    {
        StartCoroutine(DragCube());
    }
}
