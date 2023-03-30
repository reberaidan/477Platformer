using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_ShortJumpForce = 100f;              // Amount of force added when the player jumps.
	[SerializeField] private float m_EnemyJump = 100f;
    [SerializeField] private float m_JumpForce = 400f;
	[SerializeField] private float m_SlamForce = -100f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;      // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;             // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;              // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;             // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;              // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;  // A collider that will be disabled when crouching
    public GameObject Goomba;   

    public float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;            // Whether or not the player is grounded.
    public const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private CapsuleCollider2D m_collider;
    private BoxCollider2D c_collider;
    private AudioSource p_audio;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    public bool m_wasCrouching = false;
    private float accelerationSpeed;

    public void SetMovementSmoothing(float value)
    {
        m_MovementSmoothing = value;
    }

    public bool IsPlayerOnGround()
    {
        
        return m_Grounded;
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
        c_collider = m_CeilingCheck.GetComponent<BoxCollider2D>();
        p_audio = GetComponent<AudioSource>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    public void Move(float move, bool crouch, bool sjump, bool jump, bool flutter, bool slam)
    {
        
        if (m_wasCrouching)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            m_CeilingCheck.transform.position = new Vector3(m_CeilingCheck.transform.position.x,m_CeilingCheck.transform.position.y+0.5f,m_CeilingCheck.transform.position.z);

            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                
                crouch = true;
                m_wasCrouching = true;

            }
            m_CeilingCheck.transform.position = new Vector3(m_CeilingCheck.transform.position.x,m_CeilingCheck.transform.position.y-0.5f,m_CeilingCheck.transform.position.z);

        }
        // If crouching, check to see if the character can stand up

        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching

            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                
                crouch = true;
                m_wasCrouching = true;

            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            p_audio.mute = false;
            m_AirControl = true;
            if(flutter){
                move *= m_CrouchSpeed;
            }
            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }
                if(crouch || m_wasCrouching){
                    c_collider.offset = new Vector2(0, -1f);
                    m_collider.size = new Vector2(1, 1);
                    m_collider.offset = new Vector2(0, -0.4f);
                    m_Rigidbody2D.gravityScale = 2.6f;

                }
                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching) {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
                c_collider.offset = new Vector2(0, 0);
                m_collider.offset = new Vector2(0, 0);
                m_collider.size = new Vector2(1, 2);
                m_Rigidbody2D.gravityScale = 1.3f;
            }
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f*accelerationSpeed, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && sjump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_ShortJumpForce));
        }
		if (m_Grounded && jump)
		{
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
        if (!m_Grounded && flutter)
        {
            m_Rigidbody2D.AddForce(new Vector2(move, 6f));
        }
		if (!m_Grounded && slam)
		{
            m_Rigidbody2D.AddForce(new Vector2(-m_Rigidbody2D.velocity.x*4,0));
			m_Rigidbody2D.AddForce(new Vector2(0f, m_SlamForce));
            m_AirControl = false;
		}
        if (!m_Grounded || move == 0)
        {
            p_audio.mute = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("headcollider")){
            m_Rigidbody2D.AddForce(new Vector2(0, m_EnemyJump));
        }
        if (collision.transform.CompareTag("slipperyground"))
        {
            accelerationSpeed = 0.5f;
        }
        else
        {
            accelerationSpeed = 1f;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
