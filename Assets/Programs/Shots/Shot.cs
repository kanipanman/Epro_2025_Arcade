using UnityEngine;
using System.Collections.Generic;

public class Shot : MonoBehaviour
{
    [Header("🔫 弾設定")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] RectTransform pointerUI;
    [SerializeField] Camera mainCamera;
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
        joycons = JoyconManager.Instance.j;

        if (joycons == null || joycons.Count == 0)
        {
            Debug.LogWarning("⚠ Joy-Conが見つかりません。");
            return;
        }

        if (joyconIndex < joycons.Count)
        {
            j = joycons[joyconIndex];
            Debug.Log($"🎮 Joy-Con #{joyconIndex} 接続。isLeft={j.isLeft}");
        }
        else
        {
            Debug.LogWarning($"⚠ Joy-Con #{joyconIndex} が存在しません (接続数: {joycons.Count})");
        }
    }

    void Update()
    {
        if (pointerUI == null || mainCamera == null || joycons == null) return;

        // 🔄 照準（UI位置から方向ベクトル算出）
        Vector2 screenPos = pointerUI.position;
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        transform.rotation = Quaternion.LookRotation(ray.direction);

        // 🎮 各Joy-Conのボタンを監視（全員分チェック）
        foreach (Joycon joy in joycons)
        {
            if (joy == null) continue;

            bool fireDown = joy.GetButtonDown(Joycon.Button.SHOULDER_1) || joy.GetButtonDown(Joycon.Button.SHOULDER_2);
            bool fireUp   = joy.GetButtonUp(Joycon.Button.SHOULDER_1)   || joy.GetButtonUp(Joycon.Button.SHOULDER_2);

            // 🔋 チャージ開始
            if (fireDown && !isCharging)
            {
                isCharging = true;
                chargeStartTime = Time.time;
                Debug.Log($"⚡ チャージ開始 by {(joy.isLeft ? "左" : "右")}Joy-Con");
            }

            // 💣 発射
            if (fireUp && isCharging)
            {
                isCharging = false;
                float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTimeMax);
                float chargeRatio = chargeDuration / chargeTimeMax;
                float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.transform.forward = ray.direction;
                bullet.GetComponent<Rigidbody>().AddForce(ray.direction * currentPower);

                Debug.Log($"🎯 {(joy.isLeft ? "左" : "右")}Joy-Con 発射！威力={currentPower:F0}");
            }
        }
    }
}
