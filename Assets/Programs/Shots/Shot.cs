using UnityEngine;
using System.Collections.Generic;

public class Shot : MonoBehaviour
{
    [Header("🔫 発射設定")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] RectTransform pointerUI;   // 🎯 RawImage照準
    [SerializeField] Camera mainCamera;         // 🎥 メインカメラ
    [SerializeField] float minPower = 1000f;
    [SerializeField] float maxPower = 3000f;
    [SerializeField] float chargeTimeMax = 2f;

    [Header("🎮 Joy-Con設定")]
    [SerializeField] int joyconIndex = 0; // Joy-Con番号（0〜3）

    private List<Joycon> joycons;
    private Joycon j;

    private float chargeStartTime;
    private bool isCharging = false;

    void Start()
    {
        // Joy-Con初期化
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > joyconIndex)
        {
            j = joycons[joyconIndex];
        }
        else
        {
            Debug.LogWarning($"Joy-Con #{joyconIndex} が見つかりません。");
        }
    }

    void Update()
    {
        if (pointerUI == null || mainCamera == null || j == null) return;

        // --- 🎯 UI照準位置に向ける ---
        Vector2 screenPos = pointerUI.position;
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        transform.rotation = Quaternion.LookRotation(ray.direction);

        // --- ⚡ L/Rボタンでチャージ ---
        if (j.GetButtonDown(Joycon.Button.SHOULDER_1)) // L/Rボタン押下
        {
            isCharging = true;
            chargeStartTime = Time.time;
        }

        if (j.GetButtonUp(Joycon.Button.SHOULDER_1) && isCharging)
        {
            isCharging = false;

            float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTimeMax);
            float chargeRatio = chargeDuration / chargeTimeMax;
            float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

            // --- 💣 弾を発射 ---
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.forward = ray.direction;
            bullet.GetComponent<Rigidbody>().AddForce(ray.direction * currentPower);

            Debug.Log($"🎯 Joy-Con#{joyconIndex} 発射！（L/Rボタン）チャージ: {chargeDuration:F2}s / 威力: {currentPower:F0}");
        }
    }
}
