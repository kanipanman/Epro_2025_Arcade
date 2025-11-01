using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_L2 : MonoBehaviour
{
    public float move;
    public float counterCount;
    public int scoreValue = 20;
    private int counter = 0;
    private bool shouldWaitUntilReturn = false;
    public GameObject breakEffect;
    private BoxCollider thisCollider;
    private GameObject scoreText1P;
    private GameObject scoreText2P;
    private GameObject scoreText3P;
    private GameObject scoreText4P;

    void Start()
    {
        thisCollider = GetComponent<BoxCollider>();
        thisCollider.enabled = true;
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
                    shouldWaitUntilReturn = false;
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

        if (collision.gameObject.CompareTag("Bullet_1P"))
        {
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager1P>().score1P += scoreValue;
            Debug.Log("Hit: 1P");
            GenerateEffect();
        }
        else if (collision.gameObject.CompareTag("Bullet_2P"))
        {
            Destroy(collision.gameObject);
            scoreText2P.GetComponent<scoreManager2P>().score2P += scoreValue;
            Debug.Log("Hit: 2P");
            GenerateEffect();
        }
        else if (collision.gameObject.CompareTag("Bullet_3P"))
        {
            Destroy(collision.gameObject);
            scoreText3P.GetComponent<scoreManager3P>().score3P += scoreValue;
            Debug.Log("Hit: 3P");
            GenerateEffect();
        }
        else if (collision.gameObject.CompareTag("Bullet_4P"))
        {
            Destroy(collision.gameObject);
            scoreText4P.GetComponent<scoreManager4P>().score4P += scoreValue;
            Debug.Log("Hit: 4P");
            GenerateEffect();
        }

        // 共通処理：見た目を消す＆再出現
        gameObject.GetComponent<Renderer>().enabled = false;
        shouldWaitUntilReturn = true;
        StartCoroutine(WaitAndSwitchVisible());
    }

    IEnumerator WaitAndSwitchVisible()
    {
        yield return new WaitUntil(() => shouldWaitUntilReturn == false);
        gameObject.GetComponent<Renderer>().enabled = true;
        thisCollider.enabled = true;
    }

    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect);
        effect.transform.position = transform.position;
    }
}