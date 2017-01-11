using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonManager : MonoBehaviour
{
    public static PylonManager ms_instance;
    [SerializeField]
    private GameObject mp_pylon;

    private List<Pylon> m_pylons;

    private List<LineRenderer> m_walls;


    float totalEnergy = 100.0f;

    // Use this for initialization
    void Start()
    {
        m_walls = new List<LineRenderer>();
        m_pylons = new List<Pylon>();
        ms_instance = this;
    }

    // Update is called once per frame
    void OnMouseDown()
    {



    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit != null && hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Pylon")
                {
                    RemovePylon(hit.collider.gameObject.GetComponent<Pylon>());
                }
            }
            else
            {

                m_pylons.Add(GameObject.Instantiate(mp_pylon, new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), transform.rotation, null).GetComponent<Pylon>());
                EvaluatePylons();
            }
        }
    }

    void FormConnection(Pylon pylonA, Pylon pylonB, float thickness)
    {
        LineRenderer lr = new GameObject().AddComponent<LineRenderer>();

        lr.numPositions = 2;
        lr.SetPosition(0, pylonA.transform.position);
        lr.SetPosition(1, pylonB.transform.position);
        lr.startWidth = Mathf.Min(.9f, thickness / 100.0f);
        lr.endWidth = Mathf.Min(.9f, thickness / 100.0f);

        m_walls.Add(lr);
    }

    void EvaluatePylons()
    {
        totalEnergy = 100.0f;

        foreach (Pylon pylon in m_pylons)
        {
            foreach (Pylon pylon2 in m_pylons)
            {
                if (pylon == pylon2)
                {
                    continue;
                }

                if (Physics2D.RaycastAll(pylon.transform.position, pylon2.transform.position - pylon.transform.position)[1].collider.gameObject != pylon2.gameObject)
                {
                    continue;
                }

                totalEnergy -= Vector2.Distance(pylon.transform.position, pylon2.transform.position);
            }
        }



        foreach (LineRenderer lr in m_walls)
        {
            Destroy(lr.gameObject);
        }

        m_walls = new List<LineRenderer>();

        foreach (Pylon pylon in m_pylons)
        {
            foreach (Pylon pylon2 in m_pylons)
            {
                if (pylon == pylon2)
                {
                    continue;
                }

                if (Physics2D.RaycastAll(pylon.transform.position, pylon2.transform.position - pylon.transform.position)[1].collider.gameObject != pylon2.gameObject)
                {
                    continue;
                }

                FormConnection(pylon, pylon2, totalEnergy);
            }
        }

    }



    public void RemovePylon(Pylon pylon)
    {

        m_pylons.Remove(pylon);
        Destroy(pylon.gameObject);
        EvaluatePylons();
    }
}
