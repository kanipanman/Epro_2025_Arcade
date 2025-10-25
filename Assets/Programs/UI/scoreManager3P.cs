using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scoreManager3P : MonoBehaviour
{
    public int score3P = 0;
    private TMP_Text scoreText3P;
    // Start is called before the first frame update
    void Start()
    {
        score3P = 0;
        scoreText3P = GetComponentInChildren<TMP_Text>();
        scoreText3P.text = "0 pts";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText3P.text = score3P.ToString() + " pts";
    }
}
