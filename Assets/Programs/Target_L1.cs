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

void Start()
{
    scoreText1P = GameObject.Find("Main_UI/Score_1P");
    scoreText2P = GameObject.Find("Main_UI/Score_2P");
    scoreText3P = GameObject.Find("Main_UI/Score_3P");
    scoreText4P = GameObject.Find("Main_UI/Score_4P");
}

void Update()
{
    transform.position += Vector3.left * speed * Time.deltaTime;
}

void OnCollisionEnter(Collision collision)
{
    int playerNum = 0;
    Component scoreManager = null;

    if (collision.gameObject.CompareTag("Bullet_1P")) { scoreManager = scoreText1P?.GetComponent<scoreManager1P>(); playerNum = 1; }
    else if (collision.gameObject.CompareTag("Bullet_2P")) { scoreManager = scoreText2P?.GetComponent<scoreManager2P>(); playerNum = 2; }
    else if (collision.gameObject.CompareTag("Bullet_3P")) { scoreManager = scoreText3P?.GetComponent<scoreManager3P>(); playerNum = 3; }
    else if (collision.gameObject.CompareTag("Bullet_4P")) { scoreManager = scoreText4P?.GetComponent<scoreManager4P>(); playerNum = 4; }
    else { return; }

    Destroy(gameObject);
    Destroy(collision.gameObject);

    if (scoreManager == null)
    {
        Debug.LogError($"[Target_L1] ScoreManager missing for {collision.gameObject.tag}");
        return;
    }

    // -------------------------
    // ① プレイヤーの現在エリア（UDP経由）
    // -------------------------
    Area playerArea = UdpReceiver.GetPlayerArea(playerNum);
    Debug.Log($"[Target_L1] Player{playerNum} current area = {playerArea}");

    // -------------------------
    // ② バフ／デバフエリアを ArduinoManager から取得
    // -------------------------
    ArduinoManager arduinoManager = FindObjectOfType<ArduinoManager>();
    if (arduinoManager == null)
    {
        Debug.LogError("[Target_L1] ArduinoManager not found!");
        return;
    }

    Area buffArea = arduinoManager.buffArea;
    Area debuffArea = arduinoManager.debuffArea;

    // -------------------------
    // ③ スコア倍率の判定
    // -------------------------
    float multiplier = 1f;
    if (playerArea == buffArea) multiplier = 2f;
    else if (playerArea == debuffArea) multiplier = 0.5f;

    int finalScore = Mathf.RoundToInt(scoreValue * multiplier);

    // -------------------------
    // ④ スコア反映
    // -------------------------
    switch (playerNum)
    {
        case 1: ((scoreManager1P)scoreManager).score1P += finalScore; break;
        case 2: ((scoreManager2P)scoreManager).score2P += finalScore; break;
        case 3: ((scoreManager3P)scoreManager).score3P += finalScore; break;
        case 4: ((scoreManager4P)scoreManager).score4P += finalScore; break;
    }

    Debug.Log($"[Target_L1] Player{playerNum} scored {finalScore} points (x{multiplier}) | Buff: {buffArea}, Debuff: {debuffArea}");
    GenerateEffect();
}

void GenerateEffect()
{
    if (breakEffect == null) return;
    GameObject effect = Instantiate(breakEffect);
    effect.transform.position = transform.position;
}

void OnBecameInvisible()
{
    Destroy(gameObject);
}


}