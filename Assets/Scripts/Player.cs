using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public PlayerStuff playerControls;

    private float move;
    private CharacterController2D charCon;
    private InputAction movement;
    private InputAction jumping;
    private InputAction crouching;
    private InputAction fluttering;
    private bool jump;
    private bool crouch;
    private bool flutter;

    private void Awake()
    {
        playerControls = new PlayerStuff();
    }

    private void OnEnable()
    {
        movement = playerControls.Player.move;
        jumping = playerControls.Player.jump;
        crouching = playerControls.Player.crouch;
        fluttering = playerControls.Player.flutter;
        movement.Enable();
        jumping.Enable();
        crouching.Enable();
        fluttering.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        jumping.Disable();
        crouching.Disable();
        fluttering.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        crouch = false;
        flutter = false;
    }

    // Update is called once per frame
    void Update()
    {
        move = movement.ReadValue<float>();
        jump = jumping.ReadValue<float>() > 0.1f;
        crouch = crouching.ReadValue<float>() > 0.1f;
        
    }

    private void FixedUpdate()
    {
        charCon.Move(move, crouch, jump,flutter);
        
    }
}
