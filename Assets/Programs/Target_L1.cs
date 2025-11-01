using System.Collections;
using System.Collections.Generic;
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
private BoxCollider thisCollider;

void Start()
{
    scoreText1P = GameObject.Find("Score_1P");
    scoreText2P = GameObject.Find("Score_2P");
    scoreText3P = GameObject.Find("Score_3P");
    scoreText4P = GameObject.Find("Score_4P");
    thisCollider = GetComponent<BoxCollider>();
}

void Update()
{
    transform.position += Vector3.left * speed * Time.deltaTime;
}

void OnCollisionEnter(Collision collision)
{
    int playerNum = 0;
    GameObject scoreText = null;
    Component scoreManager = null;

    if (collision.gameObject.CompareTag("Bullet_1P"))
    {
        playerNum = 1;
        scoreText = scoreText1P;
        scoreManager = scoreText.GetComponent<scoreManager1P>();
    }
    else if (collision.gameObject.CompareTag("Bullet_2P"))
    {
        playerNum = 2;
        scoreText = scoreText2P;
        scoreManager = scoreText.GetComponent<scoreManager2P>();
    }
    else if (collision.gameObject.CompareTag("Bullet_3P"))
    {
        playerNum = 3;
        scoreText = scoreText3P;
        scoreManager = scoreText.GetComponent<scoreManager3P>();
    }
    else if (collision.gameObject.CompareTag("Bullet_4P"))
    {
        playerNum = 4;
        scoreText = scoreText4P;
        scoreManager = scoreText.GetComponent<scoreManager4P>();
    }
    else
    {
        return;
    }

    Destroy(gameObject);
    Destroy(collision.gameObject);
    GenerateEffect();

    // ArduinoManagerから現在のバフ/デバフ状態を取得
    ArduinoManager arduinoManager = FindObjectOfType<ArduinoManager>();
    if (arduinoManager == null)
    {
        Debug.LogError("[Target_L1] ArduinoManager not found!");
        return;
    }

    // プレイヤーの現在エリアを取得
    Area playerArea = UdpReceiver.GetPlayerArea(playerNum);
    Debug.Log($"[Target_L1] Player{playerNum} area={playerArea}, Buff={arduinoManager.buffArea}, Debuff={arduinoManager.debuffArea}");

    // 倍率計算
    float multiplier = 1f;
    if (playerArea == arduinoManager.buffArea)
    {
        multiplier = 2f;
    }
    else if (playerArea == arduinoManager.debuffArea)
    {
        multiplier = 0.5f;
    }

    int finalScore = Mathf.RoundToInt(scoreValue * multiplier);
    Debug.Log($"[Target_L1] Player{playerNum} scored {finalScore} (x{multiplier})");

    // スコア反映
    switch (playerNum)
    {
        case 1:
            ((scoreManager1P)scoreManager).score1P += finalScore;
            break;
        case 2:
            ((scoreManager2P)scoreManager).score2P += finalScore;
            break;
        case 3:
            ((scoreManager3P)scoreManager).score3P += finalScore;
            break;
        case 4:
            ((scoreManager4P)scoreManager).score4P += finalScore;
            break;
    }
}

void GenerateEffect()
{
    GameObject effect = Instantiate(breakEffect);
    effect.transform.position = gameObject.transform.position;
}

void OnBecameInvisible()
{
    Destroy(gameObject);
}


}