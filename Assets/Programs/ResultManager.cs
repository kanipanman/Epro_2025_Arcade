using UnityEngine;
using TMPro; // ← これを追加！
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText; // ← Text ではなく TMP 用
    public TextMeshProUGUI extraText;  // ← 同上

    void Start()
    {
        var gm = GameManager.Instance;

        if (gm.isCoopMode)
        {
            int total = gm.GetTotalScore();
            resultText.text = "協力モード：合計スコア " + total;

            gm.SaveHighScore();
            extraText.text = "ハイスコア：" + gm.GetHighScore();
        }
        else
        {
            int winner = (gm.playerScores[0] > gm.playerScores[1]) ? 1 : 2;
            resultText.text = "対戦モード：勝者は Player " + winner + "！";
            extraText.text = "";
        }
    }
}
