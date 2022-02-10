using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // el sistema de vidas funciona bien lo qeu no funciona son los sprites que no se ponen bien cuando ganas vida
    public int health;

    public Image[] Ships;
    public Sprite ship;
    public GameObject[] damagedSprites;
    public AudioSource audios;

    void Awake()
    {
        EventManager.Subscribe("LifeLoss", LifeLoss);
        EventManager.Subscribe("GainLife", GainLife);
    }

    void GainLife(params object[] parameters)
    {
        if (health < 5 && health > 0)
        {
            health += (int)parameters[0];

        }
        for (int i = 0; i < health; i++)
        {

            Ships[i].sprite = ship;
            Ships[i].enabled = true;

        }
    }

    void LifeLoss(params object[] parameters)
    {
        health = health - 1;
        if (audios != null)
        {

            audios.Play();
        }
        else
        {
            audios = FindObjectOfType<AudioSource>();
        }

        if (health <= 0)
        {
            EventManager.Trigger("LoseScene");
        }
        for (int i = 0; i < Ships.Length; i++)
        {
            if (i < health)
            {
                Ships[i].sprite = ship;

            }
            else
            {
                Ships[i].enabled = false;
            }

        }
        for (int i = 0; i < damagedSprites.Length; i++)
        {
            if (health >= 3)
            {
                damagedSprites[0].SetActive(true);
            }
            else if (health == 2)
            {
                damagedSprites[1].SetActive(true);
            }
            else if (health == 1)
            {
                damagedSprites[2].SetActive(true);
            }
            else
            {
                damagedSprites[i].SetActive(false);
            }
        }

    }
    void ResetSprites()
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            if (i > health)
            {
                Ships[i].sprite = ship;

            }
            else
            {
                Ships[i].enabled = false;
            }
        }


    }
    public void Save()
    {
        var data = new HealthData();
        data.health = health;

        BinarySerializer.SaveBinary(data, $"{Application.dataPath}\\HealthData.dat");
    }
    public void Load()
    {
        var data = BinarySerializer.LoadBinary<HealthData>($"{Application.dataPath}\\HealthData.dat");
        health = data.health;
        ResetSprites();
    }


}
