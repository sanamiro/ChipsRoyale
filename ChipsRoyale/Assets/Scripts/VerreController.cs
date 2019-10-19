using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerreController : MonoBehaviour
{
    // Prefabs
    public GameObject chips;

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
