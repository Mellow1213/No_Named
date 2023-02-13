using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 10.0f;

    [Tooltip("Move speed of the character in m/s")]
    public float SprintSpeed = 15.0f;

    // Player
    private CharacterController _controller;
    private Animator _animator;

    private bool _hasAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _hasAnimator = TryGetComponent(out _animator);

    }

    // Update is called once per frame
    void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);
        
        Move();
    }

    // Player Move
    private void Move()
    {
        
    }
}