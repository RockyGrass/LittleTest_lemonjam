using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotateSpeed;

	void Update ()
    {
        Vector3 eularEngle = this.transform.eulerAngles + rotateSpeed * Time.deltaTime;
        this.transform.localRotation = Quaternion.Euler(eularEngle); 
	}
}
