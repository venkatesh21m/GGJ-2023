using System.Collections.Generic;
using UnityEngine;

namespace Rudrac.GGJ2023
{
    [AddComponentMenu("GGJ2023/GravityHandler")]
    public class GravityHandler : MonoBehaviour
    {
        [SerializeField] private float g = 1f;
        private static float G;
        //in physical universe every body would be both attractor and attractee
        public static List<Graviton> Attractors = new();
        public static List<Graviton> Attractees = new();
        public static bool IsSimulatingLive = true;

        private void FixedUpdate()
        {
            G = g;//in case g is changed in editor
            if (IsSimulatingLive)//PathHandler changes this
                SimulateGravities();
        }
        public static void SimulateGravities()
        {
            foreach (Graviton attractee in Attractees)
            {
                foreach (Graviton attractor in Attractors)
                {
                    float _dist = Vector2.Distance(attractor.transform.position, attractee.transform.position);
                    if (_dist < attractor.GravityRadius)
                    {
                        attractee.transform.SetParent(attractor.transform, true);
                        if (attractor != attractee)
                        {

                            AddGravityForce(attractor.Rigidbody, attractee.Rigidbody);
                        }
                    }
                }
            }
        }

        public static void AddGravityForce(Rigidbody2D attractor, Rigidbody2D target, bool reverse = false)
        {
            Vector3 forceVector = GetForceFactor(attractor, target, reverse);
            target.AddForce(forceVector);
        }

        public static Vector3 GetForceFactor(Rigidbody2D attractor, Rigidbody2D target, bool reverse)
        {
            float massProduct = attractor.mass*target.mass*G;

            //You could also do
            //float distance = Vector3.Distance(attractor.position,target.position.

            Vector3 difference = reverse ? target.position - attractor.position : attractor.position - target.position;
            float distance = difference.magnitude; // r = Mathf.Sqrt((x*x)+(y*y))

            //F = G * ((m1*m2)/r^2)
            float unScaledforceMagnitude = massProduct/Mathf.Pow(distance,2);
            float forceMagnitude = G*unScaledforceMagnitude;

            Vector3 forceDirection = difference.normalized;

            Vector3 forceVector = forceDirection*forceMagnitude;
            return forceVector;
        }
    }
}