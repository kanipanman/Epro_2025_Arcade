using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    public string mainSceneName = "Main"; // 遷移先のシーン名（Mainシーン）
    
    // ボタンから呼ぶ関数。人数を引数に取る
    public void SetPlayerCount(int count)
    {
        // GameManagerに人数を保存
        GameManager.Instance.playerCount = count;

        // スコア配列を初期化（オプション）
        GameManager.Instance.InitializeScores();

        // Mainシーンへ移動
        SceneManager.LoadScene(mainSceneName);
    }
}
