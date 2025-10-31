using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] RectTransform pointerUI;   // 🎯 RawImage（照準UI）をここに設定
    [SerializeField] Camera mainCamera;         // 🎥 メインカメラ
    [SerializeField] float minPower = 1000f;
    [SerializeField] float maxPower = 3000f;
    [SerializeField] float chargeTimeMax = 2f;

    private float chargeStartTime;
    private bool isCharging = false;

    void Update()
    {
        if (pointerUI == null || mainCamera == null) return;

        // --- 🎯 UI照準のスクリーン座標を取得 ---
        Vector2 screenPos = pointerUI.position;

        // --- 🎥 カメラからその方向にレイを飛ばす ---
        Ray ray = mainCamera.ScreenPointToRay(screenPos);

        // --- 🔄 Rayの方向を向くように砲口を回転 ---
        transform.rotation = Quaternion.LookRotation(ray.direction);

        // --- ⚡ チャージ処理 ---
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            chargeStartTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;

            float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTimeMax);
            float chargeRatio = chargeDuration / chargeTimeMax;
            float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

            // --- 💣 弾生成＆発射 ---
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.forward = ray.direction;
            bullet.GetComponent<Rigidbody>().AddForce(ray.direction * currentPower);

            Debug.Log($"発射！UI位置: {screenPos} / 威力: {currentPower:F0}");
        }
    }
}
