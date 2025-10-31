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

    void Start()
    {
        // Joy-Con取得
        joycons = JoyconManager.Instance.j;
        if (joycons.Count > joyconIndex)
        {
            j = joycons[joyconIndex];
            Debug.Log($"Joy-Con #{joyconIndex} 接続 ({(j.isLeft ? "左" : "右")})");
        }

        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (pointerUI == null || raycaster == null || eventSystem == null || j == null)
            return;

        // --- 📍ポインター位置をUI座標に変換 ---
        // RectTransform → スクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, pointerUI.position);

        pointerEventData = new PointerEventData(eventSystem)
        {
            position = screenPos
        };

        // --- 🎯 ポインターが指しているUIを検出 ---
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        // --- 💡 UIハイライト処理 ---
        if (results.Count > 0)
        {
            // 先頭のUI要素を取得
            GameObject target = results[0].gameObject;

            // ボタンならハイライト
            var button = target.GetComponent<Button>();
            if (button != null)
            {
                // ボタンにカーソルが乗ってるとき
                // （UnityのSelectable標準ハイライトに任せてもOK）
            }

            // --- 🎮 Joy-ConのL/Rボタンでクリック ---
            if (j.GetButtonDown(Joycon.Button.SHOULDER_1) ||
                j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                ExecuteEvents.Execute(target, pointerEventData, ExecuteEvents.pointerClickHandler);
                Debug.Log($"🎯 {target.name} をクリック！");
            }

        }
        if (results.Count > 0)
        {
            Debug.Log($"🎯 Hovering: {results[0].gameObject.name}");
        }

    }
}
