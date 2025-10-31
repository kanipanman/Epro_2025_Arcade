using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerSet
    {
        public GameObject playerObject;     // 🎮 プレイヤー本体
        public GameObject pointerUI;        // 🎯 照準UI
        public GameObject cameraObject;     // 🎥 専用カメラ（分割画面用など）
        public GameObject weapon;           // 💣 ショット発射スクリプト付き武器
        public GameObject[] extras;         // その他の連動オブジェクト（任意）
    }

    [Header("👥 プレイヤー一式をまとめて登録（最大4人分）")]
    [SerializeField] PlayerSet[] playerSets; // 各プレイヤーの関連オブジェクトをまとめて設定

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManagerが存在しません！");
            return;
        }

        int activePlayers = GameManager.Instance.playerCount;
        Debug.Log($"プレイヤー数: {activePlayers}");

        // 各プレイヤーセットを順番にON/OFF切り替え
        for (int i = 0; i < playerSets.Length; i++)
        {
            bool isActive = (i < activePlayers);

            TogglePlayerSet(playerSets[i], isActive);
        }

        Debug.Log("✅ プレイヤー構成切り替え完了");
    }

    private void TogglePlayerSet(PlayerSet set, bool active)
    {
        if (set.playerObject) set.playerObject.SetActive(active);
        if (set.pointerUI) set.pointerUI.SetActive(active);
        if (set.cameraObject) set.cameraObject.SetActive(active);
        if (set.weapon) set.weapon.SetActive(active);

        if (set.extras != null)
        {
            foreach (var obj in set.extras)
            {
                if (obj) obj.SetActive(active);
            }
        }
    }
}
