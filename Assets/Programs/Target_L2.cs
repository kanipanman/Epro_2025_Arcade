using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Target_L2 : MonoBehaviour
{
    public float speed = 0.05f;
    public float score = 20f;

    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TargetStart");
    }

    IEnumerator TargetStart()
    {
        while (true)
        {
            Vector3 p = new Vector3(0, speed, 0);
            transform.Translate(p);
            yield return new WaitForSeconds(0.01f);
            counter++;

            if (counter == 100)
            {
                counter = 0;
                speed *= -1;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
