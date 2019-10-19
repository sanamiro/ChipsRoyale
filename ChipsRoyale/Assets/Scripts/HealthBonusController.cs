using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonusController : MonoBehaviour
{
    float angle = 0f;
    void Update()
    {
        angle++;

        if (angle >= 360)
        {
            angle = 0;
        }

        transform.eulerAngles = new Vector3(0f, angle, 0f);
    }
}
