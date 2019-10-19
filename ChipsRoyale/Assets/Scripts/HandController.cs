using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private bool m_isPickingDown = true;
    private bool m_isPickingUp = false;

    public GameObject ShadowCreater;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetButtonUp("A1"))
        {
            Debug.Log("testos");
            m_isPickingDown = true;
        }

        if (m_isPickingDown)
            PickDownChips();

        if (m_isPickingUp)
            PickUpChips();
    }

    public void PickDownChips()
    {
        if (m_isPickingDown && transform.localPosition.y > 0.5f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.25f, transform.localPosition.z);

        }
        else if (m_isPickingDown && transform.localPosition.y <= 0.5f)
        {
            m_isPickingDown = false;
            m_isPickingUp = true;
        }
        
    }

    public void PickUpChips()
    {
        if (m_isPickingUp && transform.localPosition.y < 10.0f)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.05f, transform.localPosition.z);
        else if (m_isPickingUp && transform.localPosition.y >= 10.0f)
        {
            m_isPickingUp = false;
        }
    }

    
}
