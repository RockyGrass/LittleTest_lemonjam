using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeUIBase : MonoBehaviour
{
    Camera _mainCamera;
    float uiPlaneDepth = 5;
    [SerializeField]
    protected Vector3 screenPos;

    float lastUIDepth;
    float sizeInView;
    protected void Awake()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogWarning("Buttom can not Get MainCamera");
            this.enabled = false;
        }

        //为了获取当前摄像机以及ui深度下，每单位世界坐标对应的屏幕比值，例如1单位x轴世界坐标，对应屏幕宽度的0.1
        Vector3 viewPoint = _mainCamera.ScreenToViewportPoint(new Vector3(0, 0, uiPlaneDepth));
        Vector3 wordPosRT = _mainCamera.ViewportToWorldPoint(new Vector3(_mainCamera.rect.max.x, _mainCamera.rect.max.y, viewPoint.z));
        Vector3 wordPosLB = _mainCamera.ViewportToWorldPoint(new Vector3(_mainCamera.rect.min.x, _mainCamera.rect.min.y, viewPoint.z));

        sizeInView = 1f / (wordPosRT.x - wordPosLB.x);
    }

    protected void Update()
    {
        FaceToCamera();
    }

    void FaceToCamera()
    {
        this.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward, _mainCamera.transform.up);
    }

    protected Vector3 GetUIPos(Vector3 screenPos)
    {
        return _mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, uiPlaneDepth));
    }

    protected Vector2 GetSizeInScreen(Vector3 localScale)
    {
        return new Vector2(localScale.x * sizeInView * Screen.width, 
                           localScale.y * sizeInView * Screen.width) * .5f;
    }
}
