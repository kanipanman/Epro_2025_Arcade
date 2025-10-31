using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConMangaerRoot : MonoBehaviour
{
[Header("各プレイヤーのオブジェクトを登録（順番通りに）")]
    public List<GameObject> playerObjects; // 各プレイヤーのCubeなど
    private List<Joycon> joycons;

    void Start()
    {
        joycons = JoyconManager.Instance.j;

        if (joycons.Count == 0)
        {
            Debug.LogWarning("Joy-Conが接続されていません。");
        }
        else
        {
            Debug.Log($"Joy-Con接続数: {joycons.Count}");
        }
    }

    void Update()
    {
        if (joycons == null || joycons.Count == 0) return;

        for (int i = 0; i < playerObjects.Count && i < joycons.Count; i++)
        {
            Joycon j = joycons[i];
            GameObject player = playerObjects[i];

            if (player == null || j == null) continue;

            // 🔄 リセットボタン
            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                j.Recenter();
            }

            // 🎮 各プレイヤーに回転を適用
            player.transform.rotation = j.GetVector();
        }
    }
}
