using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public PlayerStuff playerControls;
    public GameObject thwimp;

    private float move;
    private CharacterController2D charCon;
    public Animator animator;
    private InputAction movement;
    private InputAction jumping;
    private InputAction crouching;
    private InputAction fluttering;
	private InputAction shortjumping;
	private InputAction slamming;
    private bool jump;
    private bool crouch;
    private bool flutter;
	private bool sjump;
	public bool slam;

    private void Awake()
    {
        playerControls = new PlayerStuff();
        thwimp.SetActive(false);
    }

    private void OnEnable()
    {
        movement = playerControls.Player.move;
        jumping = playerControls.Player.jump;
        crouching = playerControls.Player.crouch;
        fluttering = playerControls.Player.flutter;
		shortjumping = playerControls.Player.sjump;
		slamming = playerControls.Player.slam;
        movement.Enable();
        jumping.Enable();
        crouching.Enable();
        fluttering.Enable();
		shortjumping.Enable();
		slamming.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        jumping.Disable();
        crouching.Disable();
        fluttering.Disable();
		shortjumping.Disable();
		slamming.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController2D>();
        jump = false;
        crouch = false;
        flutter = false;
		sjump = false;
		slam = false;
    }

    // Update is called once per frame
    void Update()
    {
        move = movement.ReadValue<float>();
        jump = jumping.ReadValue<float>() > 0.1f;
        crouch = crouching.ReadValue<float>() > 0.1f;
        flutter = fluttering.ReadValue<float>() == 1f;
		sjump = shortjumping.ReadValue<float>() > 0.1f;
		slam = slamming.ReadValue<float>() > 0.1f;
		
    }

    private void FixedUpdate()
    {
        
        charCon.Move(move, crouch, sjump, jump, flutter, slam);
        jump = false;
        sjump = false;
        slam = false;
        if (charCon.m_wasCrouching)
        {
            animator.SetBool("Crouched", true);
            animator.SetFloat("Crouch Run", Mathf.Abs(move));
            
        }
        else
        {
            animator.SetBool("Crouched", false);
            animator.SetFloat("Idle Run",Mathf.Abs(move));
        }

    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("thwimp move")){
            thwimp.SetActive(true);

        }
    }

}
