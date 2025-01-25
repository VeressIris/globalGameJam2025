using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private int maxNumOfFish;
    private GameObject player;
    private List<GameObject> fishList = new List<GameObject>();

    void Start()
    {
        player = GameObject.Find("Player");    
    }

    void Update()
    {
        float dist = Vector2.Distance(player.transform.position, transform.position);
        // Might be able to work around this by using collider triggers instead, but then i'd have to rethink the fish script a bit too
        if (dist < 6.5f)
        {
            if (fishList.Count < maxNumOfFish / 2)
            {
                SpawnFish(maxNumOfFish - fishList.Count);
            }
        }
        else if (dist > 20)
        {
            // remove all fish
            foreach (GameObject fish in fishList)
            {
                Destroy(fish);
            }
            fishList.Clear();
        }
    }

    private void SpawnFish(int numberOfFish)
    { 
        for (int i = 0; i < numberOfFish; i++)
        {
            fishList.Add(Instantiate(fishPrefab));
        }
    }
}
