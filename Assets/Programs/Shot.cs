using UnityEngine;
using System.Collections.Generic;

public class Shot : MonoBehaviour
{
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
        if (joycons.Count > joyconIndex)
        {
            j = joycons[joyconIndex];
            Debug.Log($"🎮 Joy-Con #{joyconIndex} 接続。isLeft={j.isLeft}");
        }
        else
        {
            Debug.LogWarning($"⚠ Joy-Con #{joyconIndex} が見つかりません。");
        }
    }

    void Update()
    {
        if (pointerUI == null || mainCamera == null || j == null) return;

        // --- 🎯 照準位置をRay化して砲口を向ける ---
        Vector2 screenPos = pointerUI.position;
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        transform.rotation = Quaternion.LookRotation(ray.direction);

        // --- 🎮 Joy-Conボタンによる発射処理 ---
        bool fireButtonDown = false;
        bool fireButtonUp = false;

        // 左右Joy-Conどちらでもボタン入力を拾う
        if (j.isLeft)
        {
            fireButtonDown = j.GetButtonDown(Joycon.Button.SHOULDER_1) || j.GetButtonDown(Joycon.Button.SHOULDER_2);
            fireButtonUp   = j.GetButtonUp(Joycon.Button.SHOULDER_1)   || j.GetButtonUp(Joycon.Button.SHOULDER_2);
        }
        else
        {
            fireButtonDown = j.GetButtonDown(Joycon.Button.SHOULDER_1) || j.GetButtonDown(Joycon.Button.SHOULDER_2);
            fireButtonUp   = j.GetButtonUp(Joycon.Button.SHOULDER_1)   || j.GetButtonUp(Joycon.Button.SHOULDER_2);
        }

        // --- ⚡ チャージ開始 ---
        if (fireButtonDown)
        {
            isCharging = true;
            chargeStartTime = Time.time;
        }

        // --- 💣 チャージ完了＆発射 ---
        if (fireButtonUp && isCharging)
        {
            isCharging = false;

            float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTimeMax);
            float chargeRatio = chargeDuration / chargeTimeMax;
            float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.forward = ray.direction;
            bullet.GetComponent<Rigidbody>().AddForce(ray.direction * currentPower);

            Debug.Log($"🎯 Joy-Con#{joyconIndex}（{(j.isLeft ? "左" : "右")}） 発射！威力={currentPower:F0}");
        }
    }
}
