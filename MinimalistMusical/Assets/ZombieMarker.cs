using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMarker : MonoBehaviour {
    private int m_life;
    private float m_score;
	// Use this for initialization
	void Start () {
        StartCoroutine(AgeEachSecond());
        CalculateScore();
	}

    public void SetLife(int life)
    {
        m_life = life;
    }

    private void CalculateScore()
    {
        m_score = Mathf.Max(0,1000.0f- Vector2.Distance(Treasure.ms_instance.gameObject.transform.position,transform.position));
    }

    private IEnumerator AgeEachSecond() // To be replaced with vbeat aging
    {
        while (true)
        {
            Age();
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void Age()
    {
        m_life--;
        if(m_life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
    public float GetScore()
    {
        return m_score;
    }
}
