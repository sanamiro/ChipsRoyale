using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaqueController : MonoBehaviour
{
    // Prefabs
    private GameObject parentChips;

    // Components
    private Collider col;

    // Variables
    // Timer before Flaque disappears set to 10 seconds
    private const float timerRemoveFlaque = 10f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        timer = timerRemoveFlaque;

        col = GetComponent<Collider>();

        // Disable collider at spawn
        col.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        // Enable the collider 1 second after spawning
        if (timer <= 9f && !col.enabled)
        {
            col.enabled = true;
        }

        if (timer <= 0f)
        {
            parentChips.GetComponent<FollowingTrace>().RemoveFlaque(gameObject);
        }
    }

    public void GetParentChips(GameObject go)
    {
        parentChips = go;
    }
}
