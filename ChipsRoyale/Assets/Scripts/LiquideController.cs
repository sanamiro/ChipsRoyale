using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquideController : MonoBehaviour
{
    private float coef = 15f;

    private bool growing = true;

    void Awake()
    {
        coef += Random.Range(-10f, 10f);

        transform.localScale = Vector3.one * 0.01f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (growing)
        {
            transform.localScale += Vector3.one * (Time.deltaTime * 4 / coef);
        }
        else
        {
            transform.localScale -= Vector3.one * (Time.deltaTime / (2 * coef));
        }

        if (transform.localScale.x >= 1f && growing)
        {
            growing = false;
        }

        if (transform.localScale.x <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
