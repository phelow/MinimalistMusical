using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : Sender
{
    [SerializeField]
    private GameObject mp_orb;

    private float m_generationInterval = 1.0f;

	
    void Start()
    {
        StartCoroutine(GenerateEnergy());
        StartCoroutine(SendOverTime());
    }

    private IEnumerator GenerateEnergy()
    {
        while (true)
        {
            AddEnergy(GameObject.Instantiate(mp_orb).GetComponent<Orb>());
            yield return new WaitForSeconds(m_generationInterval);
        }
    }
}
