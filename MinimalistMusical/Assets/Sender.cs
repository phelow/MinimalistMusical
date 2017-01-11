using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : Node {
    private float m_sendInterval = 1.0f;
	
    
    // Use this for initialization
	void Start () {
        StartCoroutine(SendOverTime());
	}

    protected IEnumerator SendOverTime()
    {
        while (true)
        {
            //Pick the acceptable target nodes
            List<Node> acceptableNodes = NodeContainer.ms_instance.GetAcceptableNodes(this);
            
            //Send one node to each acceptable target nodes
            foreach(Node node in acceptableNodes)
            {
                SendTo(node);
            }

            
            yield return new WaitForSeconds(m_sendInterval);
            
        }
    }
}
