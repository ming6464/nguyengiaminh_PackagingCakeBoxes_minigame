using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    public string NameLog;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Name : {NameLog} || Target : {other.transform.name}");
    }
}
