using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _speed = 12f;
    [SerializeField] float _sprint = 1.5f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 3f;
    public bool _canMove = true;

    [Header("Ground Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundDistance = .4f;
    [SerializeField] LayerMask _groundMask;

    CharacterController _controller;
    Vector3 _velocity;
    bool _isGrounded;
    Camera _mainCam;
    AudioSource _source;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mainCam = Camera.main;
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if grounded
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if(_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f;
        }

        // Move on X and Z axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
            move *= _sprint;
        if (!_canMove)
            move *= 0;
        
        _controller.Move(move * _speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && _isGrounded && _canMove)
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

        // Gravity
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
