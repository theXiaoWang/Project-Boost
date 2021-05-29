using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperEventTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Oscillator>().enabled = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
