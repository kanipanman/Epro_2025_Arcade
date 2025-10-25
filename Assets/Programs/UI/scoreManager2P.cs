using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreManager2P : MonoBehaviour
{
    public int score2P = 0;
    private TMP_Text scoreText2P;
    // Start is called before the first frame update
    void Start()
    {
        score2P = 0;
        scoreText2P = GetComponentInChildren<TMP_Text>();
        scoreText2P.text = "0 pts";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText2P.text = score2P.ToString() + " pts";
    }
}
