using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Node {
    float m_consumeInterval = 1.0f;
	// Use this for initialization
	void Start () {
        StartCoroutine(Consume());
	}
	
	IEnumerator Consume()
    {
        while (true)
        {
            while(this.m_energyOrbs.Count == 0)
            {
                yield return new WaitForSeconds(m_consumeInterval);
            }

            Destroy(this.m_energyOrbs.Pop().gameObject);
            yield return new WaitForSeconds(m_consumeInterval);
        }
    }
}
