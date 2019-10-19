using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Prefabs
    public GameObject healthBonus;
    public Mesh chips3hp;
    public Mesh chips2hp;
    public Mesh chips1hp;

    private GameObject m_enemyHand;
    private GameObject m_verre;

    // Components
    private Rigidbody rb;

    // Variables
    public int joystick;

    private Vector3 m_Velocity = Vector2.zero;
    private MainController m_mainController;

    private bool m_isJumping = false;
    private bool m_isInHand = false;
    private bool m_inTheVerre = false;

    private int m_spamCounter = 0;
    private int m_healthPoints = 3;

    private float m_speed = 2f;
    private float m_jumpHeight = 6f;

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

        m_Velocity.x = Input.GetAxisRaw("LeftAnalogX" + joystick);
        m_Velocity.z = Input.GetAxisRaw("LeftAnalogY" + joystick);

        m_isJumping = m_Velocity.y < -0.01f || m_Velocity.y > 0.01f ? true : false;

        if (Input.GetButtonDown("A" + joystick) && !m_isJumping)
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

        // Look direction
        if (m_Velocity.x != 0 || m_Velocity.z != 0)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Atan2(m_Velocity.x, m_Velocity.z) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }

        if (m_isInHand)
        {
            UpdateWhileCatched();
        }

        // Check if dead
        if (m_healthPoints <= 0)
        {
            Debug.Log("DEATH: No health left");
            DestroyChips();
        }
        if (transform.position.y < -1f)
        {
            Debug.Log("DEATH: Fell out of the table");
            DestroyChips();
        }
        if (transform.position.y > 7.5f)
        {
            Debug.Log("DEATH: Got rekt by the hand");
            DestroyChips();
        }
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
            Debug.Log("SLOW: Current speed is " + m_speed);
        }

        if (other.gameObject.tag.Equals("Liquide"))
        {
            m_healthPoints--;
            Debug.Log("DAMAGE: Current health is " + m_healthPoints);

            CheckMesh();
        }

        if (other.gameObject.tag.Equals("HealthBonus"))
        {
            m_healthPoints++;
            Debug.Log("HEALTH: Current health is " + m_healthPoints);

            Destroy(other.gameObject);

            CheckMesh();
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
        
        if (Input.GetButtonUp("B" + joystick))
            m_spamCounter++;

        if (m_spamCounter > 10 && m_isInHand)
        {
            m_isInHand = false;
            m_healthPoints--;
            Debug.Log("ESCAPED HAND: Current health is " + m_healthPoints);

            CheckMesh();
        }
    }

    public void EjectFromTheVerre(Vector3 direction)
    {
        transform.position += direction;
        rb.isKinematic = false;
        m_inTheVerre = false;
        m_verre = null;
    }

    private void DestroyChips()
    {
        float x = Random.Range(-5f, 5f);
        float z = Random.Range(-5f, 5f);

        Instantiate(healthBonus, new Vector3(x, 0f, z), Quaternion.identity);

        if (m_mainController == null)
            m_mainController = FindObjectOfType<MainController>();
        if (m_mainController != null)
            m_mainController.PlayerDied(joystick);

        Destroy(gameObject);
    }

    private void CheckMesh()
    {
        switch (m_healthPoints)
        {
            case 1:
                GetComponent<MeshFilter>().sharedMesh = chips1hp;
                break;

            case 2:
                GetComponent<MeshFilter>().sharedMesh = chips2hp;
                break;

            case 3:
            case 4:
            case 5:
            case 6:
                GetComponent<MeshFilter>().sharedMesh = chips3hp;
                break;
        }
    }
}
