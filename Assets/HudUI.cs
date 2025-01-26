using UnityEngine;

public class HudUI : MonoBehaviour
{
    [SerializeField] ShopUI shopUI;
    PlayerController player;
    
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }


    void Update()
    {
        if (player.shouldShowInventory)
            shopUI.Show();
        else
            shopUI.Hide();
    }
}
