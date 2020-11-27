﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Player player;
    PlayerMovement pm;

    [Space]

    public Image healthBar;
    public Image bloodScreen;
    public Image sprintBar;

    void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
        healthBar.fillAmount = player.currentHealth / 100;
        bloodScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = player.currentHealth / 100;
        sprintBar.fillAmount = pm.currentSprint / 100;

        if (pm.canSprint)
        {
            sprintBar.color = Color.white;
        }
        else
        {
            sprintBar.color = Color.red;
        }

        if (player.tookDamage)
        {
            StartCoroutine(ScreenDamage(1));
        }
    }

    public IEnumerator ScreenDamage(float bloodDuration)
    {
        bloodScreen.gameObject.SetActive(true);
        bloodScreen.CrossFadeAlpha(0, bloodDuration, false);
        yield return new WaitForSeconds(bloodDuration);
        bloodScreen.gameObject.SetActive(false);
    }
}