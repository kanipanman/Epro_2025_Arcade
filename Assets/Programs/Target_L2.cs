using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Target_L2 : MonoBehaviour
{
    public float move;
    public float counterCount;
    public int scoreValue = 20;
    private int counter = 0;
    public GameObject breakEffect;
    private GameObject scoreText;
    private GameObject scoreText1P;
    private GameObject scoreText2P;
    private GameObject scoreText3P;
    private GameObject scoreText4P;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TargetStart");
        scoreText1P = GameObject.Find("Score_1P");
        scoreText2P = GameObject.Find("Score_2P");
        scoreText3P = GameObject.Find("Score_3P");
        scoreText4P = GameObject.Find("Score_4P");
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
        //target_L1とほぼ同様   
        if (collision.gameObject.CompareTag("Bullet_1P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager1P>().score1P = scoreText1P.GetComponent<scoreManager1P>().score1P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            Invoke("switchVisible", 5.0f);
        }
        else if (collision.gameObject.CompareTag("Bullet_2P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager2P>().score2P = scoreText2P.GetComponent<scoreManager2P>().score2P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            Invoke("switchVisible", 5.0f);
        }
        else if (collision.gameObject.CompareTag("Bullet_3P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager3P>().score3P = scoreText3P.GetComponent<scoreManager3P>().score3P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            Invoke("switchVisible", 5.0f);
        }
        else if (collision.gameObject.CompareTag("Bullet_4P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager4P>().score4P = scoreText4P.GetComponent<scoreManager4P>().score4P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            Invoke("switchVisible", 5.0f);
        }
    }
    void switchVisible()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        //このオブジェクトを表示する
    }

    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect) as GameObject;
        effect.transform.position = gameObject.transform.position;
    }
}  

