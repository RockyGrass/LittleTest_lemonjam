using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : FakeUIBase, IDragable
{
    [SerializeField][Range(.1f,.9f)]
    float smoothMoveSpeed;
    [SerializeField][Range(0,20)]
    float slowDownSpeed;

    Vector3 _moveDir;
    Vector3 _lastScreenPos;
    bool bDragedLastFrame = false;
    Vector2 _size;

    #region UIBound_Define
    struct UIBound
    {
        public Vector2 leftBottom, rightTop;
        public void UpdateSize(Vector2 screenSize, Vector2 uiSize)
        {
            leftBottom.x = uiSize.x;
            leftBottom.y = uiSize.y;
            rightTop.x = screenSize.x - uiSize.x;
            rightTop.y = screenSize.y - uiSize.y;
        }
        public Vector3 LimitInBound(Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, leftBottom.x, rightTop.x);
            pos.y = Mathf.Clamp(pos.y, leftBottom.y, rightTop.y);

            return pos;
        }

        public Vector3 ReflectInBound(Vector3 pos)
        {
            if (pos.x < leftBottom.x)
                pos.x = leftBottom.x * 2 - pos.x;
            if (pos.x > rightTop.x)
                pos.x = rightTop.x * 2 - pos.x;
            if (pos.y < leftBottom.y)
                pos.y = leftBottom.y * 2 - pos.y;
            if (pos.y > rightTop.y)
                pos.y = rightTop.y * 2 - pos.y;

            return pos;
        }

        public Vector3 ReflectMoveDir(Vector3 screenPos, Vector3 moveDir)
        {
            //这里使用无损反弹，将原本的速度直接反向
            if (screenPos.x < leftBottom.x || screenPos.x > rightTop.x)
                moveDir.x = -moveDir.x;
            if (screenPos.y < leftBottom.y || screenPos.y > rightTop.y)
                moveDir.y = -moveDir.y;
            return moveDir;
        }
    }
    #endregion
    UIBound _bound;

    private void Start()
    {
        Vector2 sizeInScreen = GetSizeInScreen(this.transform.localScale);
        screenPos = new Vector3(Screen.width - sizeInScreen.x, Screen.height - sizeInScreen.y, 0);
        this.transform.position = GetUIPos(screenPos);
        _lastScreenPos = screenPos;
    }

    new void Update()
    {
        _bound.UpdateSize(new Vector2(Screen.width, Screen.height), GetSizeInScreen(this.transform.localScale));
        base.Update();
        SmoothMove();
    }

    void SmoothMove()
    {
        if (screenPos != _lastScreenPos&& !Input.GetMouseButton(0) && bDragedLastFrame)
        {
            _moveDir = screenPos - _lastScreenPos;
            bDragedLastFrame = false;
        }

        Vector2 addSpeed = -_moveDir.normalized * slowDownSpeed;
        _moveDir = ClampMoveDirToZero(_moveDir, addSpeed);

        if (_moveDir.x != 0 && _moveDir.y != 0)
        {
            screenPos += _moveDir * smoothMoveSpeed;

            //如果坐标超出屏幕边界，就对当前的移动方向进行反向
            _moveDir = _bound.ReflectMoveDir(screenPos, _moveDir);

            //反向完毕后再将坐标限制在屏幕内(反弹处理)
            screenPos = _bound.ReflectInBound(screenPos);
        }

        this.transform.position = GetUIPos(screenPos);
    }

    Vector3 ClampMoveDirToZero(Vector3 moveDir, Vector2 addSpeed)
    {
        if (moveDir.x > 0)
            moveDir.x = Mathf.Max(0, moveDir.x + addSpeed.x);
        if (moveDir.x < 0)
            moveDir.x = Mathf.Min(0, moveDir.x + addSpeed.x);
        if (moveDir.y > 0)
            moveDir.y = Mathf.Max(0, moveDir.y + addSpeed.y);
        if (moveDir.y < 0)
            moveDir.y = Mathf.Min(0, moveDir.y + addSpeed.y);

        return moveDir;
    }

    IEnumerator DragButton()
    {
        while (Input.GetMouseButton(0))
        {
            _lastScreenPos = screenPos;
            bDragedLastFrame = true;

            screenPos =_bound.LimitInBound(Input.mousePosition);

            yield return new WaitForFixedUpdate();
        }
    }

    public void Drag()
    {
        if (Input.GetMouseButtonDown(0))
            EventMgr.Call(EventID.SpriteJump);

        StartCoroutine(DragButton());
    }
}
