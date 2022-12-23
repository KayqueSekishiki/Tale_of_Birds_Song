using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private Transform player;
    private Transform stone;

    public static PlayerPosition instance;

    void Start()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        stone = GameObject.FindGameObjectWithTag("Stone").transform;

        if (player != null)
        {   
            CheckPoint();
        }
    }

    public void CheckPoint()
    {
        Vector3 playerPos = transform.position;
        playerPos.z = 0f;

        player.position = playerPos;
        stone.position = new Vector3(-7.8f, 0.3f, 0);
    }
}
