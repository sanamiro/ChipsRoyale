using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    public GameObject HandController;

    private Rigidbody rb;
    
    private Vector3 m_Velocity = Vector2.zero;

    private bool m_isJumping = false;
    private bool m_isInHand = false;

    private int m_spamCounter = 0;

    private float m_speed = 2f;
    private float m_jumpHeight = 6f;

    private GameObject m_verre;
    private bool m_inTheVerre = false;

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

        m_isJumping = m_Velocity.y < -0.01f || m_Velocity.y > 0.01f ? true : false;

        if (Input.GetButtonDown("Jump") && !m_isJumping)
        {
            if (m_inTheVerre)
            {
                m_verre.GetComponent<VerreController>().RemoveChips();

                EjectFromTheVerre(transform.forward * 2f);
            }
            else
            {
                m_Velocity.y = m_jumpHeight;
            }
        }

        if (Input.GetButtonUp("A1"))
            Instantiate(HandController, transform.parent);

        // Look direction
        if (m_Velocity.x != 0 || m_Velocity.z != 0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Atan2(m_Velocity.x, m_Velocity.z) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }

        if (m_isInHand)
            UpdateWhileCatched();
    }

    private void FixedUpdate()
    {
        if (m_speed < 0.2f) m_speed = 0.2f;

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
            other.gameObject.GetComponent<VerreController>().GetChips(gameObject);
            
            transform.position = other.transform.position + Vector3.up;
            rb.isKinematic = true;
            m_inTheVerre = true;
            m_verre = other.gameObject;

            //Vector3 dir = (transform.position - other.transform.position).normalized * 50f;
            //dir = new Vector3(dir.x, 2f, dir.z);
            //rb.AddForce(dir, ForceMode.Impulse);
        }

        if (other.gameObject.tag.Equals("Hand"))
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

    public void EjectFromTheVerre(Vector3 direction)
    {
        transform.position += direction;
        rb.isKinematic = false;
        m_inTheVerre = false;
        m_verre = null;
    }
}
