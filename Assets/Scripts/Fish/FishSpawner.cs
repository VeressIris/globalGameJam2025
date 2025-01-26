using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public bool isPlayerInRadius;

    [SerializeField] GameObject fishPrefab;
    [SerializeField] public float radius;
    [SerializeField] float playerExlusionRadius;
    [SerializeField] int fishCount;
    PlayerController player;

    public Vector3 RandomPointInCircle()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Mathf.Sqrt(Random.value) * radius; // Scale distance by sqrt for uniform distribution
        float x = Mathf.Cos(angle) * distance;
        float y = Mathf.Sin(angle) * distance;
        return transform.position + new Vector3(x, y, 0);
    }


    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        isPlayerInRadius = (player.transform.position - transform.position).magnitude > playerExlusionRadius;
        if (isPlayerInRadius && transform.childCount < fishCount)
        {
            var clone = Instantiate(fishPrefab, transform);
            clone.GetComponent<Fish>().fishSpawner = this;
            clone.transform.rotation = Quaternion.identity;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, playerExlusionRadius);
    }
}
