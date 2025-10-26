using System;
using UnityEngine;

public class BuffDebuffManager : MonoBehaviour
{
    public float eventInterval = 10f; // イベント発生間隔（秒）
    public Area buffArea;
    public Area debuffArea;

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
    }
}
