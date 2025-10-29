using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Vector3 mousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f; // カメラからの距離
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            Instantiate(bulletPrefab, worldPos, Quaternion.identity);
        }
    }
}
