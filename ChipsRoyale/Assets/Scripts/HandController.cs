using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private bool m_isPickingDown = false;
    private bool m_isPickingUp = false;

    public GameObject ShadowCreater;

    void Start()
    {
        StartCoroutine(WaitBeforeAttack());
    }


    void Update()
    {
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
            ShadowCreater.transform.localScale = new Vector3(ShadowCreater.transform.localScale.x + 0.025f, ShadowCreater.transform.localScale.y, ShadowCreater.transform.localScale.z + 0.025f);
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
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.05f, transform.localPosition.z);
            ShadowCreater.transform.localScale = new Vector3(ShadowCreater.transform.localScale.x - 0.0075f, ShadowCreater.transform.localScale.y, ShadowCreater.transform.localScale.z - 0.0075f);
        }
        else if (m_isPickingUp && transform.localPosition.y >= 10.0f)
        {
            m_isPickingUp = false;
            Destroy(transform.parent.gameObject,0.1f);
        }
    }

    private IEnumerator WaitBeforeAttack()
    {
        yield return new WaitForSeconds(1.0f);
        m_isPickingDown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Verre"))
        {
            m_isPickingDown = false;
            m_isPickingUp = true;
        }
    }
}
