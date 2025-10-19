using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Target_L1 : MonoBehaviour
{
    public float speed = 2f;
    public float score = 10f;

    public GameObject breakEffect;
  
    // 回転が完了したかどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Destroy(collision.gameObject);
        Debug.Log("Oncollision");
        GenerateEffect();

    }

    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect) as GameObject;
        effect.transform.position = gameObject.transform.position;
    }

    void OnBecameInvisible() 
    {
    Destroy(this.gameObject); // オブジェクトを破棄
    }
}

