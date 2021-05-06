using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCSetup : MonoBehaviour
{
    public Player player;

    private void Awake()
    {
        GC.player = player;
    }
}
