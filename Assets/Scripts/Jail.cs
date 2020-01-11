using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jail : MonoBehaviour
{
    public static Jail Inst;

    void Awake()
    {
        if (Jail.Inst == null)
            Inst = this;
    }
}
