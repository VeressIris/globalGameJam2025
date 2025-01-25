using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish properties")]
    [SerializeField] private string name;
    [SerializeField] private int rarity; // 0 indexed
    [SerializeField] private float moveSpeed = 1.5f;
    private SpriteRenderer sr;
    private bool moving = false;

    [Header("Spawning/moving properties")]
    private Vector2 topLeftCornerLimit;
    private Vector2 bottomRightCornerLimit;
    private Vector2 target;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetLimits();
        transform.position = NewPosition();
    }

    void Update()
    {
        if (moving)
        {
            if (Vector2.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                Flip();
            }
            else
            {
                moving = false;
            }
        }
        else
        {
            target = NewPosition();
            moving = true;
        }
    }

    // Get new position within bounds
    public Vector2 NewPosition()
    {
        float randomX = Random.Range(topLeftCornerLimit.x, bottomRightCornerLimit.x);
        float randomY = Random.Range(topLeftCornerLimit.y, bottomRightCornerLimit.y);

        return new Vector2(randomX, randomY);
    }

    // Set limits based on rarity
    private void SetLimits()
    {
        topLeftCornerLimit = new Vector2(-15f, 0f);
        bottomRightCornerLimit = new Vector2(15f, -10 * (rarity + 1));
    }

    // Flip horizontally to face target
    private void Flip()
    {
        Vector2 direction = target - (Vector2)transform.position;
        sr.flipX = direction.x < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(this.gameObject);
        }
    }
}
