using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpHeight;
    public float jumpTime;

    private float timeCount;
    private bool bJump = false;

    private void Start()
    {
        EventMgr.Resister(EventID.SpriteJump, OnJump);
    }

    private void OnDestroy()
    {
        EventMgr.UnResister(EventID.SpriteJump, OnJump);
    }

    void OnJump()
    {
        if (!bJump)
            StartCoroutine(StartJump());  
    }

    IEnumerator StartJump()
    {
        float origY = this.transform.position.y;
        bJump = true;
        while (timeCount < jumpTime)
        {
            timeCount += Time.deltaTime;
            float addY = EaseIn(0.0f, jumpHeight, Mathf.Clamp01(timeCount / jumpTime));
            this.transform.position = new Vector3(transform.position.x, origY + addY, transform.position.z);
            yield return null;
        }

        while (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            float addY = EaseIn(0.0f, jumpHeight, Mathf.Clamp01(timeCount / jumpTime));
            this.transform.position = new Vector3(transform.position.x, origY + addY, transform.position.z);
            yield return null;
            if (timeCount <= 0)
            {
                timeCount = 0;
                bJump = false;
                yield return null;
            }
        }
    }

    float EaseIn(float st, float ed, float per)
    {
        return Mathf.Lerp(st, ed, 1 - (1 - per) * (1 - per));
    }
}
