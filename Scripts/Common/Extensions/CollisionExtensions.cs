﻿using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Common.Extensions
{
    public enum CollisionLayer
    {
        Hero = 6,
        Enemy = 7,
        Collectable = 9,
        Clicker = 10
    }

    public static class CollisionExtensions
    {
        public static bool Matches(this Collider2D collider, UnityEngine.LayerMask layerMask)
        {
            return ((1 << collider.gameObject.layer) & layerMask) != 0;
        }

        public static int AsMask(this CollisionLayer layer)
        {
            return 1 << (int)layer;
        }
    }
}