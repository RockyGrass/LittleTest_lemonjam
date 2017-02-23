using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FatherSon : MonoBehaviour {

    private void Start()
    {
        Son b = new Son();
        Father a = b;
        a.G();
        b.G();
        a = new Son2();
        a.G();
        Father f = new Father();
        f.G();
    }

    class Father
    {
        public  virtual void G()
        {
            Debug.Log("A.G");
        }
    }

    class Son : Father
    {
        public override void G()
        {
            Debug.Log("B.G");
        }
    }

    class Son2 : Father
    {
        public override void G()
        {
            Debug.Log("C.G");
        }
    }
}
