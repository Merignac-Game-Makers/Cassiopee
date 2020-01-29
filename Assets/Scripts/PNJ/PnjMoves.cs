using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PnjMoves : MonoBehaviour
{

    protected NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public abstract void MoveToPos(Vector3 position);

}
