using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    [SerializeField] Image fillImage;


    PlayerController player;

    float targetAmount;
    float currentAmount;

    float speedRef;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        int slices = 24;

        targetAmount = Mathf.Round(player.oxygen * slices) / slices;

        fillImage.fillAmount = Mathf.SmoothDamp(fillImage.fillAmount, targetAmount, ref speedRef, .1f);
    }
}
