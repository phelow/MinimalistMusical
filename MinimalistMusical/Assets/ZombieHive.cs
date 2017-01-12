using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHive : MonoBehaviour {
    public static ZombieHive ms_instance;

    [SerializeField]
    private GameObject mp_zombie;

    private int m_zombieLifetime = 10;
    private float m_spawnInterval = 1.0f;
    private int m_zedCount = 0;
    private int m_maxZedCount = 10;

	// Use this for initialization
	void Start () {
        ms_instance = this;
	}

    public void Activate()
    {
        StartCoroutine(SpawnZombies());
    }

    void SpawnZombie()
    {
        if (m_zedCount < m_maxZedCount)
        {
            GameObject.Instantiate(mp_zombie, transform.position, transform.rotation, transform).GetComponent<Zombie>().SetLife(m_zombieLifetime);
            m_zedCount++;
        }
        m_zombieLifetime++;
    }

    public void KillZombie()
    {
        m_zedCount--;
    }

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            SpawnZombie();
            yield return new WaitForSeconds(m_spawnInterval);
        }
    }
}
