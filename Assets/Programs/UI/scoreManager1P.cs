using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreManager1P : MonoBehaviour
{
    public int score1P = 0;
    private TMP_Text scoreText1P;
    // Start is called before the first frame update
    void Start()
    {
        scoreText1P = GetComponentInChildren<TMP_Text>();
        scoreText1P.text = "0 pts";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText1P.text = score1P.ToString() + " pts";
    }
}
