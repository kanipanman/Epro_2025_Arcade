using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter_1P : MonoBehaviour
{

    public GameObject bulletPrefab;
    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f;
            Instantiate(bulletPrefab, Camera.main.ScreenToWorldPoint(mousePosition), Quaternion.identity);
        }
    }
}
