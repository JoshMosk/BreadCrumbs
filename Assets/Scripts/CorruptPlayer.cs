﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorruptPlayer : MonoBehaviour
{
    public bool m_isCorrupt;
    public bool m_isDead;

    public float m_maxhealth = 10;
    public float m_health;

    public GameObject m_gameOverScreen;
    float m_restartTimer = 3;

    private void Start()
    {
        m_health = m_maxhealth;
    }

    private void Update()
    {
        if (m_health <= 0.0f)
        {
            m_isDead = true;
        }

        if(m_isDead == true)
        {
            m_gameOverScreen.SetActive(true);
            m_restartTimer -= Time.deltaTime;

            if(m_restartTimer <= 0.0f)
            {
                Reload();
            }
        }

        if (m_isCorrupt)
        {
            m_health -= Time.deltaTime;
        }
        else
        {
            if(m_health >= m_maxhealth)
            {
                m_health = m_maxhealth;
            }
            else
            {
                m_health += Time.deltaTime;
            }
        }

    }

    public void Corrupt()
    {
        m_isCorrupt = true;
    }

    public void UnCorrupt()
    {
        m_isCorrupt = false;
    }

    public void Reload()
    {
        //SceneManager.LoadScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
