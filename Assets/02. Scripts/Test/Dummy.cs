using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown((KeyCode.A)))
        {
            Hit();
        }
    }

    public void Hit()
    {
        _animator.SetTrigger("Attacked");
    }
}
