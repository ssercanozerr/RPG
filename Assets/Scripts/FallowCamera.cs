using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowCamera : MonoBehaviour
{
    [SerializeField] Transform player;

    void Update()
    {
        transform.position = player.position;        
    }
}
