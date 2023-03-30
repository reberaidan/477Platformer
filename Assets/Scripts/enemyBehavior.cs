using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    private Rigidbody2D m_Rigidbody2D;
    public float move = 0.1f;
    private float m_MovementSmoothing = 0.2f;
    private Vector3 m_Velocity = Vector3.zero;
    // Start is called before the first frame update

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Rigidbody2D.velocity.x == 0)
        {
            m_Rigidbody2D.velocity = new Vector2(move,m_Rigidbody2D.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("wall"))
        {
            move *= -1;
        }
        else if (collision.transform.CompareTag("enemy"))
        {
            move *= -1;
        }
        else if (collision.transform.CompareTag("lava"))
        {
            Destroy(gameObject);
        }
    }

    
}
