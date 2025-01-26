using UnityEngine;

public class DepthUI : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] Transform diverImage;

    PlayerController player;

    void Update()
    {
        diverImage.position = Vector3.Lerp(startPos.position, endPos.position,1- player.depthNormalized);
    }
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();    
    }
}
