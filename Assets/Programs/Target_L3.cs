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
    public UnityEngine.Vector3 targetPosition
    = new UnityEngine.Vector3(5.0f, 2.0f, -3.0f);
    //的の移動目標座標を格納
    public float speed = 2.0f;
    public int scoreValue = 100;
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

        transform.position = UnityEngine.Vector3.Lerp
        (transform.position, targetPosition, speed * Time.deltaTime);
        //targetPositionに滑らかに移動
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet_1P") || collision.gameObject.CompareTag("Bullet_2P") 
        || collision.gameObject.CompareTag("Bullet_3P") || collision.gameObject.CompareTag("Bullet_4P"))
        {
            breakCount--;
            Debug.Log("breakCount");
            if (breakCount == 0)
            {
                Destroy(gameObject);
                Destroy(collision.gameObject);
                GenerateEffect();
                //全体に加点
                scoreText1P.GetComponent<scoreManager1P>().score1P = scoreText1P.GetComponent<scoreManager1P>().score1P + scoreValue;
                scoreText2P.GetComponent<scoreManager2P>().score2P = scoreText2P.GetComponent<scoreManager2P>().score2P + scoreValue;
                scoreText3P.GetComponent<scoreManager3P>().score3P = scoreText3P.GetComponent<scoreManager3P>().score3P + scoreValue;
                scoreText4P.GetComponent<scoreManager4P>().score4P = scoreText4P.GetComponent<scoreManager4P>().score4P + scoreValue;
                
            }
        }
    }

    void GenerateEffect()
    {
        GameObject effect = Instantiate(breakEffect) as GameObject;
        effect.transform.position = gameObject.transform.position;
    }
}
