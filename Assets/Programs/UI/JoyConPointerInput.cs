using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class JoyConPointerInput : MonoBehaviour
{
    [Header("🎯 Pointer設定")]
    [SerializeField] RectTransform pointerUI;     // RawImage照準
    [SerializeField] Canvas canvas;               // UIキャンバス
    [SerializeField] Camera uiCamera;             // UI用カメラ

    [Header("🎮 Joy-Con設定")]
    [SerializeField] int joyconIndex = 0;         // 使用するJoy-Con番号（0〜3）

    private List<Joycon> joycons;
    private Joycon j;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private float clickDelay = 0.2f;  // 連打防止
    private float lastClickTime = 0f;

    void Start()
    {
        // Joy-Con取得
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > joyconIndex)
        {
            j = joycons[joyconIndex];
            Debug.Log($"🎮 Joy-Con #{joyconIndex} 接続 ({(j.isLeft ? "左" : "右")})");
        }
        else
        {
            Debug.LogWarning($"⚠ Joy-Con #{joyconIndex} が見つかりません。");
        }

        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        if (raycaster == null)
            Debug.LogError("❌ CanvasにGraphicRaycasterが付いていません。");
        if (eventSystem == null)
            Debug.LogError("❌ EventSystemがシーン内に存在しません。");
    }

    void Update()
    {
        if (pointerUI == null || raycaster == null || eventSystem == null || j == null)
            return;

        // --- 📍ポインター位置をUI座標に変換 ---
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, pointerUI.position);
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = screenPos
        };

        // --- 🎯 ポインターが指しているUIを検出 ---
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            GameObject hovered = results[0].gameObject;

            // --- 💡 ハイライト確認用ログ ---
            Debug.Log($"🎯 Hovering: {hovered.name}");

            // --- 🎮 L/Rボタンでクリック ---
            if ((j.GetButtonDown(Joycon.Button.SHOULDER_1) || j.GetButtonDown(Joycon.Button.SHOULDER_2))
                && Time.time - lastClickTime > clickDelay)
            {
                // ✅ Textなどに当たっても親のButtonにイベントを伝える
                Button btn = hovered.GetComponentInParent<Button>();
                if (btn != null)
                {
                    ExecuteEvents.Execute(btn.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
                    Debug.Log($"✅ {btn.name} をクリックしました！");
                }
                else
                {
                    Debug.Log($"⚠ ボタンではないUIをクリック: {hovered.name}");
                }

                lastClickTime = Time.time;
            }
        }
    }
}
