using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Events;

public class Jumbo : MonoBehaviour
{
    private BoxCollider boxcol;

    private void Awake()
    {
        boxcol = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        
        CoreGameSignals.Instance.onReset += OnReset;
    }

    void Start()
    {
        boxcol.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boxcol.enabled = false;
        }
    }

    private void OnReset()
    {
        boxcol.enabled = true;
    }
    private void OnDisable()
    {
        CoreGameSignals.Instance.onReset -= OnReset;
    }
}
