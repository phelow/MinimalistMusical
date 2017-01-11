using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    protected float m_radius = 1.0f;
    protected Stack<Orb> m_energyOrbs;
    private float m_nextAngle = 0.0f;

    void Awake()
    {
        m_energyOrbs = new Stack<Orb>();
    }

    private void IncrementNextAngle()
    {
        m_nextAngle += 30.0f;
    }

    private void DecrementNextAngle()
    {
        m_nextAngle -= 30.0f;
    }

    private Vector3 GetNextPosition()
    {
        return new Vector3(transform.position.x + m_radius * Mathf.Cos(Mathf.Deg2Rad * m_nextAngle), transform.position.y + m_radius * Mathf.Sin(Mathf.Deg2Rad * m_nextAngle), 0);
    }

    protected void AddEnergy(Orb orb)
    {
        m_energyOrbs.Push(orb);
        m_energyOrbs.Peek().SetPosition(GetNextPosition());
        IncrementNextAngle();
    }

    public void SendTo(Node n)
    {
        n.AddEnergy(m_energyOrbs.Pop());
        DecrementNextAngle();
    }

    public float Range
    {
        get; set;
    }
}
