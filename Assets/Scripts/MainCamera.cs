using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Inst;

    private void Awake()
    {
        if (MainCamera.Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }
}
