using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Movement
    public Rigidbody RigidBody;
    public BoxCollider Collider;
    
    private Vector3 m_Velocity = Vector2.zero;

    private bool m_isJumping = false;

    void Start()
    {
        if (Input.GetJoystickNames().Length != 0)
        {
            if (Input.GetJoystickNames().GetValue(0).ToString() != "")
            {
                Debug.Log("Joystick detected");
                //hasJoystick = true;
                //GetComponent<MouseManager>().hasJoystick = true;
                //GameManager.hasJoystick = true;
            }
        }
    }

    void Update()
    {
        m_Velocity.x = Input.GetAxisRaw("Horizontal");
        m_Velocity.z = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonUp("Jump") && !m_isJumping)
        {
            m_Velocity.y = 1;
            m_isJumping = true;
        }
        if (m_isJumping)
        {
            m_Velocity.y = m_Velocity.y - 0.1f;
        }

        m_Velocity.Normalize();
    }

    private void FixedUpdate()
    {
        Move(m_Velocity * 2.5f);
    }


    private void Move(Vector3 targetSpeed)
    {
        RigidBody.velocity = new Vector3(targetSpeed.x, targetSpeed.y, targetSpeed.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_isJumping)
        {
            m_Velocity.y = 0;
            m_isJumping = false;
        }
    }
}
