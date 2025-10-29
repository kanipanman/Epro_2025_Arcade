using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI extraText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowResult()
    {
        gameObject.SetActive(true);

        var gm = GameManager.Instance;
        gm.InitializeScores();

        // --- 各スコアスクリプトを探して値を反映 ---
        var score1 = FindObjectOfType<scoreManager1P>();
        var score2 = FindObjectOfType<scoreManager2P>();
        var score3 = FindObjectOfType<scoreManager3P>();
        var score4 = FindObjectOfType<scoreManager4P>();

        if (score1 != null && gm.playerCount >= 1)
            gm.playerScores[0] = score1.score1P;
        if (score2 != null && gm.playerCount >= 2)
            gm.playerScores[1] = score2.score2P;
        if (score3 != null && gm.playerCount >= 3)
            gm.playerScores[2] = score3.score3P;
        if (score4 != null && gm.playerCount >= 4)
            gm.playerScores[3] = score4.score4P;

        // --- 協力モード or 対戦モードの結果表示 ---
        if (gm.isCoopMode)
        {
            int total = gm.GetTotalScore();
            resultText.text = "協力モード：合計スコア " + total;
            gm.SaveHighScore();
            extraText.text = "ハイスコア：" + gm.GetHighScore();
        }
        else
        {
            int topIndex = 0;
            for (int i = 1; i < gm.playerCount; i++)
            {
                if (gm.playerScores[i] > gm.playerScores[topIndex])
                    topIndex = i;
            }

            resultText.text = "対戦モード：勝者は Player " + (topIndex + 1) + "！";
            extraText.text = "";
        }
    }
}
