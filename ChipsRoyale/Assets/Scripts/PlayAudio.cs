using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public enum Son
    {
        ZoneSafe,
        Hurt,
        BonusHP,
        WalkTable,
        WalkTrail,
        WalkSauce,
        WalkLiquide,
        Death,
        ClickMenu,
        SelectMenu,
        Victory
    }

    public Son son;

    public UnityEngine.AudioClip audioClip_ZoneSafe;

    public UnityEngine.AudioClip audioClip_Hurt1;
    public UnityEngine.AudioClip audioClip_Hurt2;
    public UnityEngine.AudioClip audioClip_Hurt3;
    public UnityEngine.AudioClip audioClip_Hurt4;
    public UnityEngine.AudioClip audioClip_Hurt5;
    public UnityEngine.AudioClip audioClip_Hurt6;

    public UnityEngine.AudioClip audioClip_BonusHP;

    public UnityEngine.AudioClip audioClip_WalkTable1;
    public UnityEngine.AudioClip audioClip_WalkTable2;
    public UnityEngine.AudioClip audioClip_WalkTable3;
    public UnityEngine.AudioClip audioClip_WalkTable4;

    public UnityEngine.AudioClip audioClip_WalkTrail1;
    public UnityEngine.AudioClip audioClip_WalkTrail2;
    public UnityEngine.AudioClip audioClip_WalkTrail3;
    public UnityEngine.AudioClip audioClip_WalkTrail4;

    public UnityEngine.AudioClip audioClip_WalkSauce1;
    public UnityEngine.AudioClip audioClip_WalkSauce2;
    public UnityEngine.AudioClip audioClip_WalkSauce3;
    public UnityEngine.AudioClip audioClip_WalkSauce4;
    public UnityEngine.AudioClip audioClip_WalkSauce5;

    public UnityEngine.AudioClip audioClip_WalkLiquide1;
    public UnityEngine.AudioClip audioClip_WalkLiquide2;
    public UnityEngine.AudioClip audioClip_WalkLiquide3;
    public UnityEngine.AudioClip audioClip_WalkLiquide4;

    public UnityEngine.AudioClip audioClip_Death1;
    public UnityEngine.AudioClip audioClip_Death2;
    public UnityEngine.AudioClip audioClip_Death3;

    public UnityEngine.AudioClip audioClip_ClickMenu;

    public UnityEngine.AudioClip audioClip_SelectMenu1;
    public UnityEngine.AudioClip audioClip_SelectMenu2;
    public UnityEngine.AudioClip audioClip_SelectMenu3;
    public UnityEngine.AudioClip audioClip_SelectMenu4;
    public UnityEngine.AudioClip audioClip_SelectMenu5;

    public UnityEngine.AudioClip audioClip_Victory;

    private AudioSource audioSource;

    [HideInInspector]
    public bool isPlaying;

    private bool m_priority = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = 0.2f;
    }

    private void Update()
    {
        isPlaying = audioSource.isPlaying;

        if (!isPlaying)
        {
            if (son == Son.Victory) m_priority = false;

            son = Son.SelectMenu;
        }
    }

    public void PlaySound(Son s)
    {
        if (m_priority) return;

        son = s;

        float r = Random.Range(0f, 1f);
        
        switch (s)
        {
            case Son.ZoneSafe:
                audioSource.clip = audioClip_ZoneSafe;
                break;

            case Son.Hurt:
                     if (r < 0.17f) audioSource.clip = audioClip_Hurt1;
                else if (r < 0.33f) audioSource.clip = audioClip_Hurt2;
                else if (r < 0.50f) audioSource.clip = audioClip_Hurt3;
                else if (r < 0.66f) audioSource.clip = audioClip_Hurt4;
                else if (r < 0.83f) audioSource.clip = audioClip_Hurt5;
                else audioSource.clip = audioClip_Hurt6;
                break;

            case Son.BonusHP:
                audioSource.clip = audioClip_BonusHP;
                break;

            case Son.WalkTable:
                     if (r < 0.25f) audioSource.clip = audioClip_WalkTable1;
                else if (r < 0.50f) audioSource.clip = audioClip_WalkTable2;
                else if (r < 0.75f) audioSource.clip = audioClip_WalkTable3;
                else audioSource.clip = audioClip_WalkTable4;
                break;

            case Son.WalkTrail:
                     if (r < 0.25f) audioSource.clip = audioClip_WalkTrail1;
                else if (r < 0.50f) audioSource.clip = audioClip_WalkTrail2;
                else if (r < 0.75f) audioSource.clip = audioClip_WalkTrail3;
                else audioSource.clip = audioClip_WalkTrail4;
                break;

            case Son.WalkSauce:
                     if (r < 0.2f) audioSource.clip = audioClip_WalkSauce1;
                else if (r < 0.4f) audioSource.clip = audioClip_WalkSauce2;
                else if (r < 0.6f) audioSource.clip = audioClip_WalkSauce3;
                else if (r < 0.8f) audioSource.clip = audioClip_WalkSauce4;
                else audioSource.clip = audioClip_WalkSauce5;
                break;

            case Son.WalkLiquide:
                     if (r < 0.25f) audioSource.clip = audioClip_WalkLiquide1;
                else if (r < 0.50f) audioSource.clip = audioClip_WalkLiquide2;
                else if (r < 0.75f) audioSource.clip = audioClip_WalkLiquide3;
                else audioSource.clip = audioClip_WalkLiquide4;
                break;

            case Son.Death:
                     if (r < 0.33f) audioSource.clip = audioClip_Death1;
                else if (r < 0.66f) audioSource.clip = audioClip_Death2;
                else audioSource.clip = audioClip_Death3;
                break;

            case Son.ClickMenu:
                audioSource.clip = audioClip_ClickMenu;
                break;

            case Son.SelectMenu:
                     if (r < 0.2f) audioSource.clip = audioClip_SelectMenu1;
                else if (r < 0.4f) audioSource.clip = audioClip_SelectMenu2;
                else if (r < 0.6f) audioSource.clip = audioClip_SelectMenu3;
                else if (r < 0.8f) audioSource.clip = audioClip_SelectMenu4;
                else audioSource.clip = audioClip_SelectMenu5;
                break;

            case Son.Victory:
                audioSource.clip = audioClip_Victory;
                m_priority = true;
                break;
        }

        audioSource.Play();
    }
}
