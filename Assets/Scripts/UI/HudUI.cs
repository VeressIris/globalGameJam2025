using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    [SerializeField] ShopUI shopUI;
    [SerializeField] Image deathScreenBackground;
    [SerializeField] Image deathScreen;
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


        if(player.isDead) // if died
        {
            ShowDeathScreen();
        }
    }

    public Tween ShowNextScreen()
    {
        deathScreenBackground.gameObject.SetActive(true);
        return deathScreenBackground.DOColor(Color.black, 2);
    }

    public void ShowDeathScreen()
    {
        if (deathScreenBackground.gameObject.activeSelf)
            return;
        deathScreenBackground.gameObject.SetActive(true);
        DOVirtual.DelayedCall(1, () =>
        {
            AudioManager.instance.PlayOnly("DeathScreen");
        });
        deathScreenBackground.DOColor(Color.black, 2).OnComplete(() =>
        {
            deathScreen.DOColor(Color.white, 5).OnComplete(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        });
    }
}
