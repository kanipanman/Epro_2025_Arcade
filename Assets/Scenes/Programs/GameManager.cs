using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singletonパターン（どのシーンにも1つだけ存在）
    public static GameManager Instance;

    // --- ゲーム設定データ ---
    public bool isCoopMode;          // 協力モード：true / 対戦モード：false
    public int playerCount = 2;      // プレイヤー人数
    public int difficulty = 1;       // 難易度（1=Easy, 2=Normal, 3=Hard）

    // --- スコア関連 ---
    public int[] playerScores;       // 各プレイヤーのスコア
    public int highScore;            // 協力モード時のハイスコア

    void Awake()
    {
        // 既に存在する場合は破棄（シーン重複防止）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いでも保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // スコアを初期化
    public void InitializeScores()
    {
        playerScores = new int[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            playerScores[i] = 0;
        }
    }

    // スコアを加算
    public void AddScore(int playerIndex, int amount)
    {
        if (playerScores == null || playerIndex >= playerScores.Length) return;
        playerScores[playerIndex] += amount;
    }

    // 協力モード用の合計スコア
    public int GetTotalScore()
    {
        int total = 0;
        foreach (int s in playerScores) total += s;
        return total;
    }

    // ハイスコアを保存（PlayerPrefsを使用）
    public void SaveHighScore()
    {
        if (isCoopMode)
        {
            int total = GetTotalScore();
            if (total > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", total);
                PlayerPrefs.Save();
            }
        }
    }

    // ハイスコアを取得
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
}
