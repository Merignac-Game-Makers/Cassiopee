﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHouse : MonoBehaviour
{
    public Collider sas;
    public Collider inside;
    PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerControl.Instance;
    }

    // Update is called once per frame
    public void Enter()
    {
        if (!isInside())
            player.m_Agent.destination = inside.transform.position;
    }

    bool isInside() {
        return inside.bounds.Contains(player.transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player.gameObject)
            Enter();
    }
}
