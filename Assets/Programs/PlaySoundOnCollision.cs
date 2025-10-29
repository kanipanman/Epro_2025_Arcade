using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip collisionSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

     void OnCollisionEnter(Collision collision)
    {
    
        // 効果音を再生
        audioSource.PlayOneShot(collisionSound);
    }
}
