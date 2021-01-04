using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud : MonoBehaviour
{
    public GameObject bossHealthBar;
    public GameObject playerHealthBar;
    public Slider playerHealthBarSlider;
    public Slider bossHealthBarSlider;
    public TextMeshProUGUI playerHealth;


    // Start is called before the first frame update
    private void Start()
    {
        bossHealthBarSlider = bossHealthBar.GetComponent<Slider>();
        GameEvents.instance.OnPlayerEntersBoosArea += OnPlayerEntersBoosArea;
        GameEvents.instance.OnBossDeath += OnBossDeath;
        GameEvents.instance.OnBossHit += OnBossHit;
        GameEvents.instance.OnPlayerModHealth += OnPlayerModHealth;

        SetHealth();
    }


    private void OnPlayerEntersBoosArea(string bossTag, int maxHealth)
    {
        bossHealthBar.SetActive(true);
        bossHealthBarSlider.maxValue = maxHealth;
        bossHealthBarSlider.value = maxHealth;
        bossHealthBarSlider.enabled = true;
    }

    private void OnBossDeath(string bossTag)
    {
        bossHealthBar.SetActive(false);
    }

    private void OnBossHit(int maxHealth)
    {
        bossHealthBarSlider.value = maxHealth;
    }

    private void OnPlayerModHealth()
    {
        this.SetHealth();
    }


    private void SetHealth()
    {
        Player player = Player.instance;
        playerHealth.text = player.getHealth().ToString();
        playerHealthBarSlider.maxValue = player.maxHealth;
        playerHealthBarSlider.value = player.getHealth();
    }
}
