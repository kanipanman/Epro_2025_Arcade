using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score: MonoBehaviour
{
    public int score = 0;
    private TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "score:0";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString() + " pts";
    }
}
