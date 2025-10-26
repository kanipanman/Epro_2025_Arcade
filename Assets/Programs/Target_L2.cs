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

    public int baseScore = 20;
    private BuffDebuffManager buffDebuffManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TargetStart");
        scoreText1P = GameObject.Find("Main_UI/Score_1P");
        scoreText2P = GameObject.Find("Main_UI/Score_2P");
        scoreText3P = GameObject.Find("Main_UI/Score_3P");
        scoreText4P = GameObject.Find("Main_UI/Score_4P");
        buffDebuffManager = FindObjectOfType<BuffDebuffManager>();
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
    int playerNum = 0; // 1〜4P判定用

    if (collision.gameObject.CompareTag("Bullet_1P"))
    {
        targetScoreText = scoreText1P;
        scoreManager = targetScoreText != null ? targetScoreText.GetComponent<scoreManager1P>() : null;
        playerNum = 1;
    }
    else if (collision.gameObject.CompareTag("Bullet_2P"))
    {
        targetScoreText = scoreText2P;
        scoreManager = targetScoreText != null ? targetScoreText.GetComponent<scoreManager2P>() : null;
        playerNum = 2;
    }
    else if (collision.gameObject.CompareTag("Bullet_3P"))
    {
        targetScoreText = scoreText3P;
        scoreManager = targetScoreText != null ? targetScoreText.GetComponent<scoreManager3P>() : null;
        playerNum = 3;
    }
    else if (collision.gameObject.CompareTag("Bullet_4P"))
    {
        targetScoreText = scoreText4P;
        scoreManager = targetScoreText != null ? targetScoreText.GetComponent<scoreManager4P>() : null;
        playerNum = 4;
    }
    else
    {
        return;
    }

    Destroy(collision.gameObject);

    if (targetScoreText == null)
    {
        Debug.LogError($"Target scoreText is null for {collision.gameObject.tag}");
        return;
    }
    if (scoreManager == null)
    {
        Debug.LogError($"Score manager component missing on {targetScoreText.name}");
        return;
    }

    // --- バフ/デバフ倍率適用（デバッグ強化版） ---
    float multiplier = 1f;

    // use the cached manager from Start() if available
    BuffDebuffManager manager = buffDebuffManager ?? FindObjectOfType<BuffDebuffManager>();
    if (manager == null)
    {
        Debug.LogWarning("[Target_L2] BuffDebuffManager not found in scene.");
    }
    else
    {
        // PlayerArea を取得（GetPlayerArea が未定義の playerNum を返す場合もある）
        Area playerArea = Area.None;
        try
        {
            playerArea = PlayerAreaTracker.GetPlayerArea(playerNum);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Target_L2] PlayerAreaTracker.GetPlayerArea threw: {e.Message}");
        }

        Debug.Log($"[Target_L2] Player{playerNum} area={playerArea}, Buff={manager.buffArea}, Debuff={manager.debuffArea}");

        // 比較 — enumが一致するかを確認
        if (playerArea == manager.buffArea)
        {
            multiplier = 2f;
            Debug.Log($"[Target_L2] Player{playerNum} is in BUFF area -> multiplier {multiplier}");
        }
        else if (playerArea == manager.debuffArea)
        {
            multiplier = 0.5f;
            Debug.Log($"[Target_L2] Player{playerNum} is in DEBUFF area -> multiplier {multiplier}");
        }
        else
        {
            Debug.Log($"[Target_L2] Player{playerNum} not in buff/debuff area -> multiplier {multiplier}");
        }
    }

    int finalScore = Mathf.RoundToInt(scoreValue * multiplier);
    Debug.Log($"[Target_L2] scoreValue={scoreValue}, multiplier={multiplier}, finalScore={finalScore}");


    // --- スコア加算 ---
    if (playerNum == 1) ((scoreManager1P)scoreManager).score1P += finalScore;
    if (playerNum == 2) ((scoreManager2P)scoreManager).score2P += finalScore;
    if (playerNum == 3) ((scoreManager3P)scoreManager).score3P += finalScore;
    if (playerNum == 4) ((scoreManager4P)scoreManager).score4P += finalScore;

    GenerateEffect();
    gameObject.GetComponent<Renderer>().enabled = false;
    Invoke("switchVisible", 5.0f);
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

