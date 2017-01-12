using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : Segment
{
    [SerializeField]
    private Rigidbody2D m_headRigidbody;

    private float m_force = 2000.0f;
    private int m_segments = 1;
    private Segment m_lastSegment;

    [SerializeField]
    private GameObject mp_segment;
    void Start()
    {
        m_lastSegment = this;
        StartCoroutine(MoveEverySecond());
    }

    private IEnumerator MoveEverySecond() //To be replaced with beat
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            MoveTowardsPylon();
        }
    }

    private void MoveTowardsPylon()
    {
        //find closest pylon
        Pylon target = PylonManager.ms_instance.FindClosestPylon(this);

        if (target == null)
        {
            return;
        }

        //Move the head towards it
        m_headRigidbody.AddForce((m_segments * .9f) * m_force * (target.transform.position - transform.position).normalized);

    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Food")
        {
            PylonManager.ms_instance.RemovePylon(coll.gameObject.GetComponent<Pylon>());

            GameObject newSegment = GameObject.Instantiate(mp_segment, m_lastSegment.transform.position, m_lastSegment.transform.rotation, null);
            newSegment.GetComponent<SpringJoint2D>().connectedBody = m_lastSegment.GetComponent<Rigidbody2D>();

            m_lastSegment = newSegment.GetComponent<Segment>();
        }
    }
}
