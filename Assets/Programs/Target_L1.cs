using System.Collections;
using System.Collections.Generic;
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

private BuffDebuffManager buffDebuffManager;

void Start()
{
    // UIの参照
    scoreText1P = GameObject.Find("Main_UI/Score_1P");
    scoreText2P = GameObject.Find("Main_UI/Score_2P");
    scoreText3P = GameObject.Find("Main_UI/Score_3P");
    scoreText4P = GameObject.Find("Main_UI/Score_4P");

    buffDebuffManager = FindObjectOfType<BuffDebuffManager>();
}

void Update()
{
    transform.position += Vector3.left * speed * Time.deltaTime;
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

    Destroy(gameObject);
    Destroy(collision.gameObject);

    if (targetScoreText == null || scoreManager == null)
    {
        Debug.LogError($"[Target_L1] Score object or manager missing for {collision.gameObject.tag}");
        return;
    }

    // --- バフ／デバフ倍率計算 ---
    float multiplier = 1f;
    if (buffDebuffManager != null)
    {
        Area playerArea = PlayerAreaTracker.GetPlayerArea(playerNum);

        if (playerArea == buffDebuffManager.buffArea)
            multiplier = 2f;
        else if (playerArea == buffDebuffManager.debuffArea)
            multiplier = 0.5f;
    }

    int finalScore = Mathf.RoundToInt(scoreValue * multiplier);

    // --- スコア加算 ---
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

    Debug.Log($"[Target_L1] Player{playerNum} scored {finalScore} points (x{multiplier})");
    GenerateEffect();
}

void GenerateEffect()
{
    GameObject effect = Instantiate(breakEffect);
    effect.transform.position = transform.position;
}

void OnBecameInvisible()
{
    Destroy(gameObject);
}

}

