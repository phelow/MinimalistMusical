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

    void Awake()
    {

        ms_instance = this;
    }

    // Use this for initialization
    void Start()
    {
        m_walls = new List<LineRenderer>();
        m_pylons = new List<Pylon>();
    }

    public Pylon FindClosestPylon(SnakeHead head)
    {
        Pylon best = null;
        foreach (Pylon pylon in m_pylons)
        {
            if (best == null)
            {
                best = pylon;
            }

            if (Vector2.Distance(best.transform.position, head.transform.position) > Vector2.Distance(pylon.transform.position, head.transform.position))
            {
                best = pylon;
            }
        }

        return best;
    }

    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            
            m_pylons.Add(GameObject.Instantiate(mp_pylon, new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y), transform.rotation, null).GetComponent<Pylon>());
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_pylons.Add(GameObject.Instantiate(mp_pylon, new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), transform.rotation, null).GetComponent<Pylon>());

        }
    }


    public void RemovePylon(Pylon pylon)
    {

        m_pylons.Remove(pylon);
        Destroy(pylon.gameObject);
    }


}
