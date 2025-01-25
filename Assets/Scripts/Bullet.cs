using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float firingForce = 10;

    void Start()
    {
        print("start");
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(firingForce * transform.right, mode:ForceMode2D.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }
}
