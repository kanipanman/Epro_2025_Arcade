using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LorR
{
    Left,
    Right
}
public class joyCon : MonoBehaviour
{
     [SerializeField] LorR joyconType;
    // Start is called before the first frame update
    List<Joycon> joycons;
    Joycon joycon;
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        switch (joyconType)
        {
            case LorR.Left:
                joycon = joycons.Find(c => c.isLeft);
                break;
            case LorR.Right:
                joycon = joycons.Find(c => !c.isLeft);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
         var gyro = joycon.GetGyro();   //ジャイロ入力
        var angle = transform.localEulerAngles; //オブジェクトの現在の回転
        //座標系の違いを補正
        angle.x += -gyro.y;
        angle.y += gyro.z;
        angle.z += -gyro.x;
        transform.localEulerAngles = angle;

        //トリガーボタン（LRボタン）が押されたときに回転リセット
        if(joycon.GetButtonDown(Joycon.Button.DPAD_LEFT))
            transform.localEulerAngles = Vector3.zero;
    }
}
