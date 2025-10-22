using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Target_L2 : MonoBehaviour
{
    public float move;
    public float counterCount;
    public float scoreValue = 20f;
    
    int counter = 0;
    public GameObject breakEffect;
    private GameObject scoreText;
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TargetStart");
        scoreText = GameObject.Find("Score_1P");
    }

    IEnumerator TargetStart()
    {
        while (true)
        {
            Vector3 p = new Vector3(0, move, 0);
            transform.Translate(p);
            yield return new WaitForSeconds(0.01f);
            counter++;

            if (counter == counterCount)
            {
                counter = 0;
                move *= -1;
                float waitTime = Random.Range(0.3f, 2.0f);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        Destroy(collision.gameObject);
        Debug.Log("Oncollision");
        GenerateEffect();
       
    }

    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect) as GameObject;
        effect.transform.position = gameObject.transform.position;
    }
}  

