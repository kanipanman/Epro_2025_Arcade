using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerContoller : MonoBehaviour
{
    public float time = 0;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        //timeを減少させる

        GetComponent<Text>().text = time.ToString("F2");
       
    }
}
