using UnityEngine;
using UnityEngine.InputSystem;

namespace Rudrac.GGJ2023
{
    public class Player : MonoBehaviour
    {
        public Graviton Graviton;
        public Key ThurstKey = Key.Space;
        public float ThurstMagnitude = 1.0f;
        public float RotationSpeed = 1.0f;
        private bool _grounded;
        private void FixedUpdate()
        {
            if (Graviton.BeingAttractedBy)
            {
                if (!_grounded)
                {
                    Rotate();

                    if (Keyboard.current[ThurstKey].isPressed)
                    {
                        // Apply thurst opposite 
                        ApplyThurst();
                    }

                }
            }
        }

        public void ApplyThurst()
        {
            var force =  GravityHandler.GetForceFactor(Graviton.AttractedBy.Rigidbody, Graviton.Rigidbody, true);
            Graviton.Rigidbody.AddForce(force * ThurstMagnitude);
        }

        public void Rotate()
        {
            var velocity = Graviton.Rigidbody.velocity;
            if (velocity.magnitude > 0.1)
            {
                Quaternion dirQ = Quaternion.LookRotation (velocity);
                Quaternion slerp = Quaternion.Slerp (transform.rotation, dirQ, velocity.magnitude * RotationSpeed * Time.deltaTime);
                Graviton.Rigidbody.MoveRotation(slerp);
            }
        }
    }
}
