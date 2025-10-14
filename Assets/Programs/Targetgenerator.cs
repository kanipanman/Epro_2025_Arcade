using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetgenerator : MonoBehaviour
{

    [SerializeField] GameObject[] target;
    //発生させる的の種類のリスト

    public float watingTime;
    //的の出現間隔
    private float functionTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        functionTime += Time.deltaTime;
        if (functionTime >= watingTime)
        {
            Instantiate(target[0]);
            functionTime = 0.0f;
        }
    

    }
}
