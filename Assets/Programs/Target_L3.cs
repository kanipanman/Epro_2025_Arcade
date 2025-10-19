using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Target_L3 : MonoBehaviour
{

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = UnityEngine.Random.Range(1.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        float wave = Mathf.Sin(Time.time * speed);
        transform.position = new UnityEngine.Vector3(4.6f + -speed * Time.time, 2.0f + wave, -49f);
    }

    void OnBecameInvisible() 
    {
    Destroy(this.gameObject); // オブジェクトを破棄
    }

}
