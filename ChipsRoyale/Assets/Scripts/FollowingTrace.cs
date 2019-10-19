using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTrace : MonoBehaviour
{
    // Prefabs
    public GameObject flaque;

    // Components
    private Rigidbody rb;

    // Lists
    private List<GameObject> listFlaque = new List<GameObject>();

    // Variables
    // Timer before new Flaque set to 0.5 second
    private const float timerNewFlaque = 0.5f;
    private float timer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        timer = timerNewFlaque;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        // If the Chips is not moving
        if (rb.velocity.x == 0 & rb.velocity.z == 0)
        {
            // Reset timer
            timer = timerNewFlaque;
        }

        // If the Chips is jumping
        if (rb.velocity.y > 0.5f || rb.velocity.y < -0.5f)
        {
            // Reset timer
            timer = timerNewFlaque;
        }

        if (timer <= 0f)
        {
            TryNewFlaque();

            // Reset timer
            timer = timerNewFlaque;
        }
    }

    private void TryNewFlaque()
    {
        Vector2 chipsPos = new Vector2(transform.position.x, transform.position.z);
        bool isThereAFlaque = false;

        foreach (GameObject go in listFlaque)
        {
            Vector2 flaquePos = new Vector2(go.transform.position.x, go.transform.position.z);
            if (Vector2.Distance(chipsPos, flaquePos) < 0.5f)
            {
                isThereAFlaque = true;
                break;
            }
        }

        if (!isThereAFlaque)
        {
            GameObject newFlaque = Instantiate(flaque, transform.position, Quaternion.identity);
            newFlaque.transform.position = new Vector3(chipsPos.x, 0f, chipsPos.y);
            newFlaque.GetComponent<FlaqueController>().GetParentChips(gameObject);
            listFlaque.Add(newFlaque);
        }
    }

    public void RemoveFlaque(GameObject go)
    {
        listFlaque.Remove(go);
    }
}
