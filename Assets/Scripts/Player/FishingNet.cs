using DG.Tweening;
using UnityEngine;

public class FishingNet : MonoBehaviour
{
    [SerializeField] Transform fishPosition;

    public void SellFish(PlayerController player)
    {
        foreach (Transform t in fishPosition)
        {
            var fish = t.GetComponent<Fish>();
            if (fish)
            {
                player.money += (int)fish.value;
            }
        }
        Utils.DestroyChildren(fishPosition);
    }

    public void AddFish(GameObject fish)
    {
        fish.GetComponent<Fish>().enabled = false;
        Destroy(fish.GetComponent<Rigidbody2D>());
        var scale = fish.transform.lossyScale;
        fish.transform.parent = fishPosition;
        fish.transform.DOLocalMove(Vector3.zero,1);
    }
}
