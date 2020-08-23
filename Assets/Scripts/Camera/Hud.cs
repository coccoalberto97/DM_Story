using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject bossHealthBar;
    Slider healthBarSlider;
    // Start is called before the first frame update
    private void Start()
    {
        healthBarSlider = bossHealthBar.GetComponent<Slider>();
        GameEvents.instance.OnPlayerEntersBoosArea += OnPlayerEntersBoosArea;
        GameEvents.instance.OnBossDeath += OnBossDeath;
        GameEvents.instance.OnBossHit += OnBossHit;
    }


    private void OnPlayerEntersBoosArea(string bossTag, int maxHealth)
    {
        bossHealthBar.SetActive(true);
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        healthBarSlider.enabled = true;
    }

    private void OnBossDeath(string bossTag)
    {
        bossHealthBar.SetActive(false);
    }

    private void OnBossHit(int maxHealth)
    {
        healthBarSlider.value = maxHealth;
    }

}
