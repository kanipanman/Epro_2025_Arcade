using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scoreManager4P : MonoBehaviour
{
  public int score4P = 0;
    private TMP_Text scoreText4P;
    // Start is called before the first frame update
    void Start()
    {
        score4P = 0;
        scoreText4P = GetComponentInChildren<TMP_Text>();
        scoreText4P.text = "0 pts";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText4P.text = score4P.ToString() + " pts";
    }
}
