using UnityEngine;
using System.Collections.Generic;

public class Shot2 : MonoBehaviour
{
    [Header("🔫 弾設定")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] RectTransform pointerUI;
    [SerializeField] Camera mainCamera;
    [SerializeField] float minPower = 1000f;
    [SerializeField] float maxPower = 3000f;
    [SerializeField] float chargeTimeMax = 2f;

    private List<Joycon> joycons;
    private Joycon j;
    private float chargeStartTime;
    private bool isCharging = false;
    private int joyconIndex = 1; // 🎮 2P = Joy-Con #1

    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > joyconIndex)
            j = joycons[joyconIndex];
    }

    void Update()
    {
        if (j == null || pointerUI == null || mainCamera == null) return;

        Vector2 screenPos = pointerUI.position;
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        transform.rotation = Quaternion.LookRotation(ray.direction);

        bool fireDown = j.GetButtonDown(Joycon.Button.SHOULDER_1) || j.GetButtonDown(Joycon.Button.SHOULDER_2);
        bool fireUp   = j.GetButtonUp(Joycon.Button.SHOULDER_1)   || j.GetButtonUp(Joycon.Button.SHOULDER_2);

        if (fireDown && !isCharging)
        {
            isCharging = true;
            chargeStartTime = Time.time;
            Debug.Log("⚡ 2P チャージ開始");
        }

        if (fireUp && isCharging)
        {
            isCharging = false;
            float chargeRatio = Mathf.Clamp((Time.time - chargeStartTime) / chargeTimeMax, 0, 1);
            float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.forward = ray.direction;
            bullet.GetComponent<Rigidbody>().AddForce(ray.direction * currentPower);
            Debug.Log($"🎯 2P 発射！威力={currentPower:F0}");
        }
    }
}
