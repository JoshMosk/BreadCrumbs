using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CorruptPlayer : MonoBehaviour
{
	public bool m_isCorrupt;
	public bool m_isDead;

	public float m_maxhealth = 10;
	public float m_health;

	public GameObject m_gameOverScreen;
	public float m_restartTimer = 5;

	public GameObject corruptMeter;

	public Transform playerRespawnPoint = null;
	public Transform characterRespawnPoint = null;

	public Transform player = null;
	public Transform character = null;

	private void Start()
	{
		m_health = m_maxhealth;

		playerRespawnPoint = GameObject.Find("playerRespawnPoint").transform;
		characterRespawnPoint = GameObject.Find("characterRespawnPoint").transform;

		player = GameObject.Find("PlayerContainer").transform;
		character = GameObject.Find("Lylah").transform;
	}

	private void Update()
	{
		if (m_health <= 0.0f)
		{
			m_isDead = true;
		}

		if (m_isDead == true)
		{
			m_gameOverScreen.SetActive(true);
			m_restartTimer -= Time.deltaTime;

			if (m_restartTimer <= 0.0f)
			{
				Reload();
			}
		}

		if (corruptMeter != null)
		{


			//float a = corruptMeter.GetComponent<Material>().GetFloat("_DissolveSlider");
			float t = m_health / m_maxhealth;


			corruptMeter.GetComponent<Renderer>().material.SetFloat("_DissolveSlider", Mathf.Lerp(0, 1, t));
		}

		if (m_isCorrupt)
		{
			m_health -= Time.deltaTime;

			
		}
		else
		{
			if (m_health >= m_maxhealth)
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
		//need game to uncorrupt, reset death and health//TODO
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if(playerRespawnPoint != null && player != null)
		{
			player.position  = playerRespawnPoint.position;
		}

		if(characterRespawnPoint != null && character != null)
		{
			character.position = characterRespawnPoint.position;
		}

	}
}
