using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour {
    public static NodeContainer ms_instance;
    [SerializeField]
    private GameObject mp_sink;
    [SerializeField]
    private GameObject mp_source;
    private List<Node> m_nodeList;

    // Use this for initialization
    void Start ()
    {
        ms_instance = this;
        m_nodeList = new List<Node>();

        PlaceSink();
        PlaceSource();
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
    }

    void PlaceSink()
    {
        m_nodeList.Add(GameObject.Instantiate(mp_sink, GetRandomPosition(), transform.rotation, null).GetComponent<Node>());
    }

    void PlaceSource()
    {
        m_nodeList.Add(GameObject.Instantiate(mp_source, GetRandomPosition(), transform.rotation, null).GetComponent<Node>());
    }

	// Update is called once per frame
	void Update () {
		
	}

    public List<Node> GetAcceptableNodes(Node node)
    {
        List<Node> acceptableNodes = new List<Node>();

        foreach(Node candidate in m_nodeList)
        {
            if(candidate != node)
            {
                acceptableNodes.Add(candidate);
            }
        }

        return acceptableNodes;
    }
}
