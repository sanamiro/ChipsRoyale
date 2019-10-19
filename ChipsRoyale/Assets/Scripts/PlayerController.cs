﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    
    private Vector3 m_Velocity = Vector2.zero;

    private bool m_isJumping = false;

    private float m_speed = 2f;
    private float m_jumpHeight = 6f;

    private bool inVerre = false;

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

        m_isJumping = m_Velocity.y < -0.01f || m_Velocity.y > 0.01f ? true : false;

        if (Input.GetButtonDown("Jump") && !m_isJumping)
        {
            if (inVerre)
            {
                transform.position += transform.forward * 2f;
            }
            else
            {
                m_Velocity.y = m_jumpHeight;
            }
        }

        if (m_isJumping) Debug.Log("Jumping");
        Debug.Log(m_Velocity.y);

        // Look direction
        if (m_Velocity.x != 0 || m_Velocity.z != 0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Atan2(m_Velocity.x, m_Velocity.z) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }
    }

    private void FixedUpdate()
    {
        if (m_speed <= 0f) m_speed = 0.01f;

        rb.velocity = new Vector3(m_Velocity.x * m_speed, m_Velocity.y, m_Velocity.z * m_speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Flaque"))
        {
            m_speed--;
        }

        if (other.gameObject.tag.Equals("Verre"))
        {
            transform.position = other.transform.position + Vector3.up;
            rb.isKinematic = true;
            inVerre = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Flaque"))
        {
            m_speed++;
        }

        if (other.gameObject.tag.Equals("Verre"))
        {
            inVerre = false;
            rb.isKinematic = false;
        }
    }
}
