using System;
using UnityEngine;

public class BuffDebuffManager : MonoBehaviour
{
    public float eventInterval = 10f; // イベント発生間隔（秒）
    public Area buffArea;
    public Area debuffArea;

    public ArduinoLEDController ledController; // Inspectorでアタッチ

    private System.Random rnd = new System.Random();

    void Start()
    {
        // 初回即発動、その後 eventInterval 秒ごとに繰り返す
        InvokeRepeating(nameof(SetBuffDebuffAreas), 0f, eventInterval);
    }

    void SetBuffDebuffAreas()
    {
        Area[] allAreas = (Area[])Enum.GetValues(typeof(Area));

        // バフエリアをランダム決定
        buffArea = allAreas[rnd.Next(allAreas.Length)];

        // デバフエリアはバフと被らないように決定
        Area temp;
        do
        {
            temp = allAreas[rnd.Next(allAreas.Length)];
        } while (temp == buffArea);

        debuffArea = temp;

        Debug.Log($"Buff Area: {buffArea}, Debuff Area: {debuffArea}");

        // --- Arduino LED 送信 ---
        if (ledController != null)
        {
            for (int i = 0; i < 4; i++)
            {
                char colorCode = 'Y'; // デフォルト: 黄

                if (i == (int)buffArea)
                    colorCode = 'G'; // バフ: 緑
                else if (i == (int)debuffArea)
                    colorCode = 'R'; // デバフ: 赤

                ledController.SetLED(i, colorCode);

                Debug.Log($"LED Command sent -> AreaIndex: {i}, ColorCode: {colorCode}");
            }
        }
        else
        {
            Debug.LogWarning("LED Controller not assigned in BuffDebuffManager.");
        }
    }
}
