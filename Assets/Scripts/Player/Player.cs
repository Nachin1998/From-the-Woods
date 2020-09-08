﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health = 100f;
    public Image healthBar;
    public bool isDead = false;

    public Image bloodScreen;
    void Start()
    {
        healthBar.fillAmount = health / 100;
        bloodScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        healthBar.fillAmount = health / 100;

        if(health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(ScreenDamage(1));
    }

    public IEnumerator ScreenDamage(float bloodDuration)
    {
        bloodScreen.gameObject.SetActive(true);
        bloodScreen.CrossFadeAlpha(0, bloodDuration, false);
        yield return new WaitForSeconds(bloodDuration);
        bloodScreen.gameObject.SetActive(false);
    }
}