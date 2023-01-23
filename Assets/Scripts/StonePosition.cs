using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePosition : MonoBehaviour
{
    private Transform stone;

    public static StonePosition instance;

    void Start()
    {
        instance = this;

        stone = GameObject.FindGameObjectWithTag("Stone").transform;

        if (stone != null)
        {
            CheckPoint();
        }
    }

    public void CheckPoint()
    {
        Vector3 stonePos = transform.position;
        stonePos.z = 0f;
        stone.position = stonePos;
    }
}
