using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private GameObject mp_marker;

    [SerializeField]
    private LayerMask m_markerMask;

    private ZombieMarker m_lastMarker;

    private const float m_searchRadius = 5.0f;

    private float m_movementForce = 100.0f;

    private int m_originalLife;
    private int m_life;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(MoveEverySecond());
    }

    public void SetLife(int life)
    {
        m_movementForce += life * 3;
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
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            m_rigidbody.AddForce(new Vector2(x,y).normalized * m_movementForce);
            float angle = (Mathf.Atan2(y, x) * Mathf.Rad2Deg);

            transform.eulerAngles = new Vector3(0, 0, 270 + angle);
        }
        else
        {
            m_rigidbody.AddForce((best.transform.position - this.transform.position).normalized * m_movementForce);

            float angle = (Mathf.Atan2(best.transform.position.y - transform.position.y, best.transform.position.x - transform.position.x) * Mathf.Rad2Deg);

            transform.eulerAngles = new Vector3(0, 0, 270 + angle);
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
            for(int i = 0; i< 100; i++)
            {
                SoundManager.ms_instance.PlaySoundAt(transform.position);
            }

            GameManager.ms_instance.GameOver();
            StartCoroutine(DieRoutine());
        }
    }

    private IEnumerator DieRoutine()
    {
        float t = 0.0f;
        SoundManager.ms_instance.PlaySoundAt(transform.position);
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            m_spriteRenderer.color = Color.Lerp(Color.white, Color.clear, t);
            yield return new WaitForEndOfFrame();
        }


        ZombieHive.ms_instance.KillZombie();
        Destroy(this.gameObject);
    }

    private void Die()
    {
        StartCoroutine(DieRoutine());
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
