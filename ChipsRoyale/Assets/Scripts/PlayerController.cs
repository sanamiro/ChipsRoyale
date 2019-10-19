using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    
    private Vector3 m_Velocity = Vector2.zero;

    private bool m_isJumping = false;
    private bool m_isInHand = false;

    private int m_spamCounter = 0;

    private float m_speed = 2f;
    private float m_jumpHeight = 6f;

    private GameObject m_enemyHand;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

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
        m_Velocity = rb.velocity;

        m_Velocity.x = Input.GetAxisRaw("Horizontal");
        m_Velocity.z = Input.GetAxisRaw("Vertical");

        m_isJumping = m_Velocity.y != 0 ? true : false;

        if (Input.GetButtonDown("Jump") && !m_isJumping)
        {
            m_Velocity.y = m_jumpHeight;
        }

        if (m_Velocity.x != 0 || m_Velocity.z != 0)
        {
            float lookingAt = Mathf.Atan2(m_Velocity.x, m_Velocity.z);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, lookingAt * Mathf.Rad2Deg, transform.eulerAngles.z);
        }

        if (m_isInHand)
            UpdateWhileCatched();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(m_Velocity.x * m_speed, m_Velocity.y, m_Velocity.z * m_speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Flaque"))
        {
            m_speed--;
        }
        else if (other.gameObject.tag.Equals("Hand"))
        {
            m_enemyHand = other.gameObject;
            m_isInHand = true;
            m_spamCounter = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Flaque"))
        {
            m_speed++;
        }
    }

    private void UpdateWhileCatched()
    {
        m_Velocity = new Vector3(0, 0, 0);

        transform.position = m_enemyHand.transform.position;

        if (Input.GetButtonUp("B1"))
            m_spamCounter++;

        if (m_spamCounter > 10 && m_isInHand)
            m_isInHand = false;
    }
}
