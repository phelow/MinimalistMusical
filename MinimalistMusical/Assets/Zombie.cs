using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private GameObject mp_marker;

    [SerializeField]
    private LayerMask m_markerMask;

    private ZombieMarker m_lastMarker;

    private const float m_searchRadius = 5.0f;

    private float m_movementForce = 1000.0f;

    private int m_originalLife;
    private int m_life;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(MoveEverySecond());
    }

    public void SetLife(int life)
    {
        m_movementForce += life;
        m_originalLife = life;
        m_life = life;
    }

    private void Move()
    {
        //Drop a marker
        m_lastMarker = GameObject.Instantiate(mp_marker, transform.position, transform.rotation, null).GetComponent<ZombieMarker>().GetComponent<ZombieMarker>();
        m_lastMarker.SetLife(m_originalLife - m_life);



        //Look for targets
        List<ZombieMarker> nearbyZombieMarkers = Physics2D.CircleCastAll(transform.position, m_searchRadius, Vector2.up, m_searchRadius, m_markerMask).
            Select(
            x => x.collider.gameObject.GetComponent<ZombieMarker>()).ToList();
        ZombieMarker best = null;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, m_searchRadius, Vector2.up, m_searchRadius, m_markerMask);


        if (nearbyZombieMarkers.Count > 0)
        {

            foreach (ZombieMarker zm in nearbyZombieMarkers)
            {
                if (m_lastMarker == zm)
                {
                    continue;
                }

                if (best == null)
                {
                    best = zm;
                }
                else if (best.GetScore() < zm.GetScore() && m_lastMarker.GetScore() < zm.GetScore())
                {
                    best = zm;
                }
            }

        }
        //Move towards highest in range (or move randomly)

        if (best == null || Random.Range(0, 100) < 10)
        {
            m_rigidbody.AddForce(new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * m_movementForce);
        }
        else
        {
            m_rigidbody.AddForce((best.transform.position - this.transform.position).normalized * m_movementForce);
        }

        m_life--;
        if (m_life <= 0)
        {
            Die();
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Finish")
        {
            GameManager.ms_instance.GameOver();
        }
    }

    private void Die()
    {
        ZombieHive.ms_instance.KillZombie();
        Destroy(this.gameObject);
    }

    private IEnumerator MoveEverySecond() //replace with audio integration
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Move();
        }
    }

}
