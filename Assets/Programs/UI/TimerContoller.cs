using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TimerContoller : MonoBehaviour
{
    public float time = 0;
    private AudioSource audioSource;
    public AudioClip audioClip;
    private bool hasPlayedTenSecondSound = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        time -= Time.deltaTime;

        GetComponent<Text>().text = time.ToString("F2");
        if (time <= 10f && !hasPlayedTenSecondSound)
        {
            // 効果音を一度だけ再生
            // AudioSource.PlayOneShot() を使うと、既存の再生音を中断せず、単発で音を鳴らせます
            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);

                // フラグをtrueにして、二度と再生しないようにする
                hasPlayedTenSecondSound = true;
            }
        }
      

        
    
    }
}
