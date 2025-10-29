using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_L2 : MonoBehaviour
{
public float move;
public float counterCount;
public int scoreValue = 20;
private int counter = 0;
public GameObject breakEffect;

private GameObject scoreText1P;
private GameObject scoreText2P;
private GameObject scoreText3P;
private GameObject scoreText4P;

private ArduinoManager arduinoManager;

void Start()
{
    StartCoroutine("TargetStart");

    scoreText1P = GameObject.Find("Main_UI/Score_1P");
    scoreText2P = GameObject.Find("Main_UI/Score_2P");
    scoreText3P = GameObject.Find("Main_UI/Score_3P");
    scoreText4P = GameObject.Find("Main_UI/Score_4P");

    arduinoManager = FindObjectOfType<ArduinoManager>();
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
    GameObject targetScoreText = null;
    Component scoreManager = null;
    int playerNum = 0;

    if (collision.gameObject.CompareTag("Bullet_1P"))
    {
        targetScoreText = scoreText1P;
        scoreManager = targetScoreText?.GetComponent<scoreManager1P>();
        playerNum = 1;
    }
    else if (collision.gameObject.CompareTag("Bullet_2P"))
    {
        targetScoreText = scoreText2P;
        scoreManager = targetScoreText?.GetComponent<scoreManager2P>();
        playerNum = 2;
    }
    else if (collision.gameObject.CompareTag("Bullet_3P"))
    {
        targetScoreText = scoreText3P;
        scoreManager = targetScoreText?.GetComponent<scoreManager3P>();
        playerNum = 3;
    }
    else if (collision.gameObject.CompareTag("Bullet_4P"))
    {
        targetScoreText = scoreText4P;
        scoreManager = targetScoreText?.GetComponent<scoreManager4P>();
        playerNum = 4;
    }
    else
    {
        return;
    }

    Destroy(collision.gameObject);

    if (targetScoreText == null || scoreManager == null)
    {
        Debug.LogError($"[Target_L2] Score object or manager missing for {collision.gameObject.tag}");
        return;
    }

    // --- バフ／デバフ倍率計算 ---
    float multiplier = 1f;
    if (arduinoManager != null)
    {
        Area playerArea = UdpReceiver.GetPlayerArea(playerNum);
        Area buff = arduinoManager.buffArea;
        Area debuff = arduinoManager.debuffArea;

        Debug.Log($"[Target_L2] Player{playerNum} area={playerArea}, Buff={buff}, Debuff={debuff}");

        if (playerArea == buff)
        {
            multiplier = 2f;
            Debug.Log($"[Target_L2] Player{playerNum} is in BUFF area!");
        }
        else if (playerArea == debuff)
        {
            multiplier = 0.5f;
            Debug.Log($"[Target_L2] Player{playerNum} is in DEBUFF area!");
        }
    }
    else
    {
        Debug.LogWarning("[Target_L2] ArduinoManager not found!");
    }

    int finalScore = Mathf.RoundToInt(scoreValue * multiplier);

    // --- スコア加算 ---
    switch (playerNum)
    {
        case 1: ((scoreManager1P)scoreManager).score1P += finalScore; break;
        case 2: ((scoreManager2P)scoreManager).score2P += finalScore; break;
        case 3: ((scoreManager3P)scoreManager).score3P += finalScore; break;
        case 4: ((scoreManager4P)scoreManager).score4P += finalScore; break;
    }

    Debug.Log($"[Target_L2] Player{playerNum} scored {finalScore} points (x{multiplier})");

    GenerateEffect();
    gameObject.GetComponent<Renderer>().enabled = false;
    Invoke("switchVisible", 5.0f);
}

void switchVisible()
{
    gameObject.GetComponent<Renderer>().enabled = true;
}

void GenerateEffect()
{
    GameObject effect = Instantiate(breakEffect);
    effect.transform.position = transform.position;
}


}