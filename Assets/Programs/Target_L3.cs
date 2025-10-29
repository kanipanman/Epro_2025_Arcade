using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_L3 : MonoBehaviour
{
public float breakCount;
public GameObject breakEffect;

public Vector3 targetPosition = new Vector3(5.0f, 2.0f, -3.0f);
public float speed = 2.0f;
public int scoreValue = 100;

private GameObject scoreText1P;
private GameObject scoreText2P;
private GameObject scoreText3P;
private GameObject scoreText4P;

private ArduinoManager arduinoManager;

void Start()
{
    scoreText1P = GameObject.Find("Main_UI/Score_1P");
    scoreText2P = GameObject.Find("Main_UI/Score_2P");
    scoreText3P = GameObject.Find("Main_UI/Score_3P");
    scoreText4P = GameObject.Find("Main_UI/Score_4P");

    arduinoManager = FindObjectOfType<ArduinoManager>();
}

void Update()
{
    // targetPositionに滑らかに移動
    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
}

void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Bullet_1P") ||
        collision.gameObject.CompareTag("Bullet_2P") ||
        collision.gameObject.CompareTag("Bullet_3P") ||
        collision.gameObject.CompareTag("Bullet_4P"))
    {
        breakCount--;
        Debug.Log($"[Target_L3] breakCount = {breakCount}");

        if (breakCount <= 0)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            GenerateEffect();

            // --- 各プレイヤーにスコア適用 ---
            ApplyScoreWithBuff(scoreText1P, 1, scoreValue);
            ApplyScoreWithBuff(scoreText2P, 2, scoreValue);
            ApplyScoreWithBuff(scoreText3P, 3, scoreValue);
            ApplyScoreWithBuff(scoreText4P, 4, scoreValue);
        }
    }
}

void ApplyScoreWithBuff(GameObject scoreText, int playerNum, int baseScore)
{
    if (scoreText == null)
    {
        Debug.LogError($"[Target_L3] ScoreText missing for Player{playerNum}");
        return;
    }

    float multiplier = 1f;

    if (arduinoManager != null)
    {
        Area playerArea = UdpReceiver.GetPlayerArea(playerNum);
        Area buff = arduinoManager.buffArea;
        Area debuff = arduinoManager.debuffArea;

        Debug.Log($"[Target_L3] Player{playerNum} area={playerArea}, Buff={buff}, Debuff={debuff}");

        if (playerArea == buff)
        {
            multiplier = 2f;
            Debug.Log($"[Target_L3] Player{playerNum} is in BUFF area!");
        }
        else if (playerArea == debuff)
        {
            multiplier = 0.5f;
            Debug.Log($"[Target_L3] Player{playerNum} is in DEBUFF area!");
        }
    }
    else
    {
        Debug.LogWarning("[Target_L3] ArduinoManager not found!");
    }

    int finalScore = Mathf.RoundToInt(baseScore * multiplier);

    switch (playerNum)
    {
        case 1:
            scoreText.GetComponent<scoreManager1P>().score1P += finalScore;
            break;
        case 2:
            scoreText.GetComponent<scoreManager2P>().score2P += finalScore;
            break;
        case 3:
            scoreText.GetComponent<scoreManager3P>().score3P += finalScore;
            break;
        case 4:
            scoreText.GetComponent<scoreManager4P>().score4P += finalScore;
            break;
    }

    Debug.Log($"[Target_L3] Player{playerNum} gained {finalScore} (x{multiplier})");
}

void GenerateEffect()
{
    GameObject effect = Instantiate(breakEffect);
    effect.transform.position = transform.position;
}


}