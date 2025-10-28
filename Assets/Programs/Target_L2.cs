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
    private bool shouldWaitUntilReturn = false;
    public GameObject breakEffect;
    private GameObject scoreText;
    private BoxCollider thisCollider;
    private GameObject scoreText1P;
    private GameObject scoreText2P;
    private GameObject scoreText3P;
    private GameObject scoreText4P;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider = GetComponent<BoxCollider>();
        thisCollider.enabled = true;    //当たり判定の有効化
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
                if (shouldWaitUntilReturn)
                {
                    shouldWaitUntilReturn = false; // 待機解除のトリガーを引く
                }
            
                counter = 0;
                move *= -1;
                float waitTime = Random.Range(0.3f, 2.0f);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        thisCollider.enabled = false;
        //target_L1とほぼ同様   
        if (collision.gameObject.CompareTag("Bullet_1P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager1P>().score1P = scoreText1P.GetComponent<scoreManager1P>().score1P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            shouldWaitUntilReturn = true; 
            StartCoroutine(WaitAndSwitchVisible());
        }
        else if (collision.gameObject.CompareTag("Bullet_2P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager2P>().score2P = scoreText2P.GetComponent<scoreManager2P>().score2P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            shouldWaitUntilReturn = true;
            StartCoroutine(WaitAndSwitchVisible());
        }
        else if (collision.gameObject.CompareTag("Bullet_3P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager3P>().score3P = scoreText3P.GetComponent<scoreManager3P>().score3P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            shouldWaitUntilReturn = true;
            StartCoroutine(WaitAndSwitchVisible());
        }
        else if (collision.gameObject.CompareTag("Bullet_4P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager4P>().score4P = scoreText4P.GetComponent<scoreManager4P>().score4P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
            gameObject.GetComponent<Renderer>().enabled = false;
            shouldWaitUntilReturn = true;
            StartCoroutine(WaitAndSwitchVisible());
        }
    }
    
    IEnumerator WaitAndSwitchVisible()
    {
        // counter が counterCount に達するまで待機する
        // つまり、次の移動の折り返し地点まで待つ
        yield return new WaitUntil(() => shouldWaitUntilReturn == false);

        // 待機が完了したら、レンダラーを再表示し、当たり判定を有効化
        gameObject.GetComponent<Renderer>().enabled = true;
        thisCollider.enabled = true;
        
    }

    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect) as GameObject;
        effect.transform.position = gameObject.transform.position;
    }
}  

