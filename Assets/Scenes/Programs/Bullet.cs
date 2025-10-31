using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 他の弾との衝突を無効化
        Bullet[] otherBullets = FindObjectsOfType<Bullet>();
        Collider myCol = GetComponent<Collider>();

        foreach (Bullet other in otherBullets)
        {
            if (other != this)
            {
                Collider otherCol = other.GetComponent<Collider>();
                if (myCol && otherCol)
                    Physics.IgnoreCollision(myCol, otherCol);
            }
        }

        // 5秒後に自動で消滅
        Destroy(gameObject, 5f);
    }
}
