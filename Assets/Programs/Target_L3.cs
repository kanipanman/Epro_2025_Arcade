using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Target_L3 : MonoBehaviour
{
    public float breakCount;
    public GameObject breakEffect;
    public UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(5.0f, 2.0f, -3.0f);
    public float speed = 2.0f;

    public float scoreValue;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {  
        transform.position = UnityEngine.Vector3.Lerp
        (transform.position, targetPosition, speed * Time.deltaTime);
    }
    

    void OnCollisionEnter(Collision collision)
    {
        breakCount--;
        if (breakCount == 0)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            GenerateEffect();
        }
    }
    
    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect) as GameObject;
        effect.transform.position = gameObject.transform.position;
    }


}
