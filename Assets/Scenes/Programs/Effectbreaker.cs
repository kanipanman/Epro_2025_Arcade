using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectbreaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Invoke("BreakEffect",2.0f);
    }

    // Update is called once per frame
    void BreakEffect()
    {
        Destroy(gameObject);
    }
  
}
