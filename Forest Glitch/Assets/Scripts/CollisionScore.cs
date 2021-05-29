using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScore : MonoBehaviour
{
    private void OnCollisionEnter(Collision Hunt)
    {
        if (Hunt.gameObject.tag == "Animal")
        {
            Destroy(Hunt.gameObject);
            GameStats.score += 1;
        }
    }
}
