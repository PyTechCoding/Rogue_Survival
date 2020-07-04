using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    public float immortalityDuration = 1f;
    public float invincCount;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        //currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincCount > 0)
        {
            invincCount -= Time.deltaTime;

            if(invincCount < 0 && PlayerController.playerInstance != null)
            {
                PlayerController.playerInstance.bodySR.color = new Color(
                                                PlayerController.playerInstance.bodySR.color.r,
                                                PlayerController.playerInstance.bodySR.color.g,
                                                PlayerController.playerInstance.bodySR.color.b,
                                                1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            currentHealth--;

            MakeInvincible(immortalityDuration);

            if (currentHealth <= 0)
            {
                Time.timeScale = 0f;
                currentHealth = 0;
                PlayerController.playerInstance.gameObject.SetActive(false);
                AudioController.instance.PlayGameOver();
                AudioController.instance.PlaySFX(9);
                
                UIController.instance.deathScreen.SetActive(true);
                
                if(CharacterTracker.instance.remainingContinues <= 0)
                {
                    UIController.instance.continueButton.SetActive(false);
                }
            }

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void MakeInvincible(float length)
    {
        invincCount = length;
        PlayerController.playerInstance.bodySR.color = new Color(
                                                PlayerController.playerInstance.bodySR.color.r,
                                                PlayerController.playerInstance.bodySR.color.g,
                                                PlayerController.playerInstance.bodySR.color.b,
                                                .5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;        
    }
        
}
