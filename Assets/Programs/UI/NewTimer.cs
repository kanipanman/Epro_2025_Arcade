using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NewTimer : MonoBehaviour
{
   public float time = 10f;
    public ResultManager resultManager; // ← インスペクタでアサイン

    public TextMeshProUGUI timerText;
    private bool isEnded = false;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isEnded) return; // 一度だけ実行

        time -= Time.deltaTime;
        if (time < 0) time = 0;
        timerText.text = time.ToString("F2");

        if (time <= 0)
        {
            isEnded = true;
            resultManager.ShowResult(); // ← 結果を表示！
        }
    }
}
