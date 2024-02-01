﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FallowCamera : MonoBehaviour
    {
        [SerializeField] Transform player;

        void LateUpdate()
        {
            transform.position = player.position;        
        }
    }
}
