using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // RawImage用

public class Shot : MonoBehaviour
{
    [Header("🔫 発射設定")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] RectTransform pointerUI;
    [SerializeField] Camera mainCamera;
    [SerializeField] float minPower = 1000f;
    [SerializeField] float maxPower = 3000f;
    [SerializeField] float chargeTimeMax = 2f;
    [SerializeField] float coolTime = 1.5f;

    [Header("🎮 Joy-Con設定")]
    [SerializeField] int joyconIndex = 0;

    [Header("🔊 効果音設定")]
    [SerializeField] AudioSource audioSource;  // 🎧 AudioSourceをアタッチ
    [SerializeField] AudioClip chargeSE;       // ⚡ チャージ開始音
    [SerializeField] AudioClip fireSE;         // 💥 発射音

    private List<Joycon> joycons;
    private Joycon j;

    private float chargeStartTime;
    private bool isCharging = false;
    private bool isCooling = false;
    private float coolTimer = 0f;

    // 🎯 照準UI
    private RawImage pointerImage;
    private Color originalColor;
    private Coroutine blinkCoroutine;

    void Start()
    {
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > joyconIndex)
        {
            j = joycons[joyconIndex];
        }
        else
        {
            Debug.LogWarning($"⚠ Joy-Con #{joyconIndex} が見つかりません。");
        }

        if (pointerUI != null)
        {
            pointerImage = pointerUI.GetComponent<RawImage>();
            if (pointerImage != null)
                originalColor = pointerImage.color;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogWarning("⚠ AudioSource が未設定です！");
        }
    }

    void Update()
    {
        if (pointerUI == null || mainCamera == null || j == null) return;

        // --- 🕒 クールタイム処理 ---
        if (isCooling)
        {
            coolTimer -= Time.deltaTime;

            if (coolTimer <= coolTime * 0.2f && blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkPointer());
            }

            if (coolTimer <= 0)
            {
                isCooling = false;
                coolTimer = 0;

                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                    blinkCoroutine = null;
                }

                if (pointerImage != null)
                    pointerImage.color = originalColor;
            }
            return;
        }

        // --- 🎯 UIの方向をRayで指定 ---
        Vector2 screenPos = pointerUI.position;
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        transform.rotation = Quaternion.LookRotation(ray.direction);

        // --- ⚡ L/Rボタンでチャージ ---
        if (j.GetButtonDown(Joycon.Button.SHOULDER_1))
        {
            isCharging = true;
            chargeStartTime = Time.time;

            // 🎵 チャージ開始音
            if (audioSource != null && chargeSE != null)
                audioSource.PlayOneShot(chargeSE);
        }

        if (j.GetButtonUp(Joycon.Button.SHOULDER_1) && isCharging)
        {
            isCharging = false;

            float chargeDuration = Mathf.Clamp(Time.time - chargeStartTime, 0, chargeTimeMax);
            float chargeRatio = chargeDuration / chargeTimeMax;
            float currentPower = Mathf.Lerp(minPower, maxPower, chargeRatio);

            // 💣 弾を発射
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.forward = ray.direction;
            bullet.GetComponent<Rigidbody>().AddForce(ray.direction * currentPower);

            Debug.Log($"🎯 Joy-Con#{joyconIndex} 発射！威力={currentPower:F0}");

            // 🎵 発射音
            if (audioSource != null && fireSE != null)
                audioSource.PlayOneShot(fireSE);

            // ⏳ クールタイム開始
            isCooling = true;
            coolTimer = coolTime;

            if (pointerImage != null)
            {
                Color c = pointerImage.color;
                c.a = 0.3f;
                pointerImage.color = c;
            }
        }
    }

    // --- 💫 点滅アニメーション ---
    private System.Collections.IEnumerator BlinkPointer()
    {
        float blinkSpeed = 0.15f;
        bool fadeOut = true;

        while (true)
        {
            if (pointerImage == null) yield break;

            Color c = pointerImage.color;
            c.a = fadeOut ? 0.2f : 1f;
            pointerImage.color = c;

            fadeOut = !fadeOut;
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}
