using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConSphere : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject Plane;

    void Update()
    {
        if (StartPoint == null || Plane == null)
        {
            Debug.LogWarning("JoyConSphere: StartPoint または Plane が未設定です。");
            return;
        }

        var n = Plane.transform.up;              // 平面の法線ベクトル
        var x = Plane.transform.position;        // 平面上の1点
        var x0 = StartPoint.transform.position;  // 射出点
        var m = StartPoint.transform.forward;    // 射出方向

        float denom = Vector3.Dot(n, m);         // 分母（法線と方向ベクトルの内積）

        // ⚠️ ほぼ平行な場合はスキップ
        if (Mathf.Abs(denom) < 1e-6f)
        {
            Debug.LogWarning("JoyConSphere: Plane と Ray が平行です。交点なし。");
            return;
        }

        float h = Vector3.Dot(n, x);
        float t = (h - Vector3.Dot(n, x0)) / denom;
        Vector3 intersectPoint = x0 + t * m;

        // ⚠️ NaN / Infinity チェック
        if (float.IsNaN(intersectPoint.x) || float.IsInfinity(intersectPoint.x))
        {
            Debug.LogWarning("JoyConSphere: 無効な交点座標です。");
            return;
        }

        transform.position = intersectPoint;
    }
}
