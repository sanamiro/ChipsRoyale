using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Enum
    private enum Surface
    {
        Table,
        Sauce,
        Trail,
        Liquide
    }

    private Surface surface;

    // Prefabs
    public GameObject healthBonus;
    public Mesh chips3hp;
    public Mesh chips2hp;
    public Mesh chips1hp;

    private GameObject m_enemyHand;
    private GameObject m_verre;

    // Components
    private Rigidbody rb;

    private PlayAudio audioComp;

    // Variables
    public int joystick;

    private Vector3 m_Velocity = Vector2.zero;
    private MainController m_mainController;

    private bool m_isJumping = false;
    private bool m_isInHand = false;
    private bool m_inTheVerre = false;
    private bool m_isSauced = false;

    private int m_spamCounter = 0;
    private int m_healthPoints = 3;

    private float m_speed = 2f;
    private float m_jumpHeight = 6f;

    private const float m_timerSauced = 5f;
    private float m_timerSauce = 0f;

    // coef < 1 = moins de chance de se faire chopper
    // coef > 1 = plus de chance de se faire chopper
    [HideInInspector]
    public float coefHand = 1f;

    void Awake()
    {
        m_timerSauce = m_timerSauced;

        surface = Surface.Table;

        rb = GetComponent<Rigidbody>();
        audioComp = Camera.main.GetComponent<PlayAudio>();

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
        if (m_isSauced)
        {
            m_timerSauce -= Time.deltaTime;

            if (m_timerSauce <= 0f)
            {
                m_isSauced = false;
                m_timerSauce = m_timerSauced;

                coefHand = 1f;
                Debug.Log("SAUCE: false");
            }
        }

        if (surface == Surface.Trail) m_speed = 1f;
        else m_speed = 2f;
        
        rb.velocity = new Vector3(m_Velocity.x * m_speed, m_Velocity.y, m_Velocity.z * m_speed);
    
        if ((rb.velocity.x != 0 || rb.velocity.z != 0) && !m_isJumping && !m_inTheVerre)
        {
            if (!GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Play();
            }
            if(audioComp.son != PlayAudio.Son.WalkTable && audioComp.son != PlayAudio.Son.WalkSauce && audioComp.son != PlayAudio.Son.WalkTrail && audioComp.son != PlayAudio.Son.WalkLiquide && audioComp.son != PlayAudio.Son.Hurt) {
                switch (surface)
                {
                    case Surface.Table:
                        audioComp.PlaySound(PlayAudio.Son.WalkTable);
                        break;

                    case Surface.Sauce:
                        audioComp.PlaySound(PlayAudio.Son.WalkSauce);
                        break;

                    case Surface.Trail:
                        audioComp.PlaySound(PlayAudio.Son.WalkTrail);
                        break;

                    case Surface.Liquide:
                        audioComp.PlaySound(PlayAudio.Son.WalkLiquide);
                        break;
                }
            }
        }
        else if (GetComponent<ParticleSystem>().isPlaying)
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("BolSauce"))
        {
            surface = Surface.Sauce;

            m_isSauced = true;
            coefHand++;
            Debug.Log("SAUCE: true | " + coefHand);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("BolSauce"))
        {
            surface = Surface.Table;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Flaque"))
        {
            surface = Surface.Trail;
        }

        if (other.gameObject.tag.Equals("Liquide"))
        {
            surface = Surface.Liquide;
            m_healthPoints--;
            Debug.Log("DAMAGE | HP: " + m_healthPoints);

            audioComp.PlaySound(PlayAudio.Son.Hurt);

            CheckMesh();
        }

        if (other.gameObject.tag.Equals("HealthBonus"))
        {
            m_healthPoints++;
            Debug.Log("HEALTH | HP: " + m_healthPoints);

            audioComp.PlaySound(PlayAudio.Son.BonusHP);

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

            audioComp.PlaySound(PlayAudio.Son.ZoneSafe);
        }

        if (other.gameObject.tag.Equals("Hand"))
        {
            m_enemyHand = other.gameObject;
            m_isInHand = true;
            m_spamCounter = 0;
            other.gameObject.GetComponent<HandController>().HandIndic.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Flaque"))
        {
            surface = Surface.Table;
        }

        if (other.gameObject.tag.Equals("Liquide"))
        {
            surface = Surface.Table;
        }
    }

    private void UpdateWhileCatched()
    {
        m_Velocity = new Vector3(0, 0, 0);

        transform.position = new Vector3(m_enemyHand.transform.position.x, m_enemyHand.transform.position.y - 1.32f, m_enemyHand.transform.position.z);
        transform.localRotation = new Quaternion(0, -200, 0, 0);
        
        if (Input.GetButtonUp("B" + joystick))
            m_spamCounter++;

        if (m_spamCounter > 3 && m_isInHand)
        {
            m_isInHand = false;
            m_healthPoints--;
            Debug.Log("ESCAPED HAND | HP: " + m_healthPoints);

            audioComp.PlaySound(PlayAudio.Son.Hurt);

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

        audioComp.PlaySound(PlayAudio.Son.Death);

        Destroy(gameObject);
    }

    private void CheckMesh()
    {
        switch (m_healthPoints)
        {
            case 1:
                transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = chips1hp;
                break;

            case 2:
                transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = chips2hp;
                break;

            case 3:
            case 4:
            case 5:
            case 6:
                transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = chips3hp;
                break;
        }
    }

    private IEnumerator MakeControllerVibrate(float vibrationTime)
    {
        PlayerIndex joystickIndex = PlayerIndex.One;

        switch (joystick)
        {
            case 1:
                joystickIndex = PlayerIndex.Two;
                break;

            case 2:
                joystickIndex = PlayerIndex.Three;
                break;

            case 3:
                joystickIndex = PlayerIndex.Four;
                break;

            case 4:
                joystickIndex = PlayerIndex.One;
                break;

            default:
                break;
        }
        GamePad.SetVibration(joystickIndex, 1, 1);

        yield return new WaitForSeconds(vibrationTime);
        
        GamePad.SetVibration(joystickIndex, 0, 0);
    }
}
