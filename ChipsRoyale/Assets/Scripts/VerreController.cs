using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerreController : MonoBehaviour
{
    // Prefabs
    public GameObject chips;

    private const float m_timerBeforeOut = 3f;
    private float m_timer = 0f;

    private void Start()
    {
        m_timer = m_timerBeforeOut;
    }

    private void Update()
    {
        if (chips != null)
        {
            m_timer -= Time.deltaTime;

            if (m_timer <= 0f)
            {
                m_timer = m_timerBeforeOut;

                chips.GetComponent<PlayerController>().EjectFromTheVerre(chips.transform.forward * 2f);
                RemoveChips();
            }
        }
        else m_timer = m_timerBeforeOut;
    }

    public void GetChips(GameObject go)
    {
        if (chips == null)
        {
            chips = go;
        }
        else
        {
            chips.GetComponent<PlayerController>().EjectFromTheVerre(go.transform.forward * 2f);
            chips = go;
        }
    }

    public void RemoveChips()
    {
        chips = null;
    }
}
