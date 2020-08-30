﻿using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Feet climber")]
    [RequireComponent(typeof(Creature))]
    public class CreatureFeetClimber : MonoBehaviour
    {
        public LayerMask sphereCastLayerMask;
        public float footSpeed = 5;
        public float sweepMinDelay = 0.5f;
        public float sweepMaxVerticalAngle = 30;
        public float sweepMaxHorizontalAngle = 30;

        public float sphereCastRadius = 0.05f;
        public float sweepDistance = 1;

        public float moveOutWeight = 0.2f;

        public float legLenghtMultiplier = 1.3f;
        public float minFootSpacing = 0.2f;

        public float upMaxAngle = 30;
        public bool showDebug;

    }
}
