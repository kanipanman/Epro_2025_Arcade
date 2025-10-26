using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI extraText;

    void Start()
    {
        // 最初は非表示
        gameObject.SetActive(false);
    }

    // 外部から呼び出して表示する
    public void ShowResult()
    {
        gameObject.SetActive(true);

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
