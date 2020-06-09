using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool active = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "agent")
        {
            //goal reached
        }
    }
}
