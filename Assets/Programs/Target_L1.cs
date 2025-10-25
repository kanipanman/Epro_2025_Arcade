using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Target_L1 : MonoBehaviour
{
    public float speed = 2f;
    public int scoreValue = 10;
    public GameObject breakEffect;
    private GameObject scoreText1P;
    private GameObject scoreText2P;
    private GameObject scoreText3P;
    private GameObject scoreText4P;
   
  

    // Start is called before the first frame update
    void Start()
    {
        scoreText1P = GameObject.Find("Score_1P");
        scoreText2P = GameObject.Find("Score_2P");
        scoreText3P = GameObject.Find("Score_3P");
        scoreText4P = GameObject.Find("Score_4P");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        //死ぬほど見づらいですが容赦願います
        //弾が当たった後のスコア処理、オブジェクトの破壊とエフェクト出現
        if (collision.gameObject.CompareTag("Bullet_1P")) //1Pの弾が当たった場合
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            scoreText1P.GetComponent<scoreManager1P>().score1P = scoreText1P.GetComponent<scoreManager1P>().score1P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
        }
        else if (collision.gameObject.CompareTag("Bullet_2P")) //2Pの弾が当たった場合
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            scoreText2P.GetComponent<scoreManager2P>().score2P = scoreText2P.GetComponent<scoreManager2P>().score2P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
        }
        else if (collision.gameObject.CompareTag("Bullet_3P")) //3Pの弾が当たった場合
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            scoreText3P.GetComponent<scoreManager3P>().score3P = scoreText3P.GetComponent<scoreManager3P>().score3P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
        }
        else if (collision.gameObject.CompareTag("Bullet_4P")) //4Pの弾が当たった場合
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            scoreText4P.GetComponent<scoreManager4P>().score4P = scoreText4P.GetComponent<scoreManager4P>().score4P + scoreValue;
            Debug.Log("Oncollision");
            GenerateEffect();
        }

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

