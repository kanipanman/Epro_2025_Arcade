using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float minPower = 1000f;  // 最小威力
    [SerializeField] float maxPower = 3000f;  // 最大威力
    [SerializeField] float chargeTimeMax = 2f; // フルチャージまでの時間
    [SerializeField] float aimSpeed = 0.05f;    // マウス操作での回転速度

    private float chargeStartTime;
    private bool isCharging = false;

    void Update()
    {
        // --- 🔄 マウスで照準回転 ---
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;

        Vector3 angle = transform.eulerAngles;
        angle.x = -mousePos.y * aimSpeed;
        angle.y = mousePos.x * aimSpeed;
        angle.z = 0;
        transform.eulerAngles = angle;

        // --- ⚡ チャージ処理 ---
        if (Input.GetMouseButtonDown(0))
        {
            // チャージ開始
            isCharging = true;
            chargeStartTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            // チャージ完了 → 発射
            isCharging = false;

            float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTimeMax);
            float chargeRatio = chargeDuration / chargeTimeMax;

            // 威力を補間
            float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

            // 弾生成＆発射
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * currentPower);

            Debug.Log($"弾発射！チャージ時間: {chargeDuration:F2}s / 威力: {currentPower:F0}");
        }
    }
}
