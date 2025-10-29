using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject[] players; // プレイヤーを順番に登録（最大4人）

    void Start()
    {
        // GameManagerが存在するかチェック
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManagerが存在しません！");
            return;
        }

        // 現在のプレイヤー人数を取得
        int activePlayers = GameManager.Instance.playerCount;

        // 各プレイヤーのON/OFFを切り替え
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                players[i].SetActive(i < activePlayers);
            }
        }

        Debug.Log($"プレイヤー数: {activePlayers} → アクティブ切り替え完了");
    }
}
