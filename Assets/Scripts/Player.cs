using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rudrac.GGJ2023
{
    public class Player : MonoBehaviour
    {

        public static event Action chargingForLaunch;
        public static event Action Launched;

        public Graviton Graviton;
        public Key ThurstKey = Key.Space;
        public Key leftKey = Key.A;
        public Key RightKey = Key.D;
        public float ThurstMagnitude = 1.0f;
        public float RotationSpeed = 1.0f;
        public float MovementSpeed = 1.0f;
        [Space]
        public float AccumulatedForce = 10;
        public float AccumulationFillingForce = 0.2f;

        public bool Grounded { get; set; }

        private bool applyingThrust = false;
        private bool launch = false;
        private Vector2 movement;
        private void Update()
        {
            if (Graviton.BeingAttractedBy)
            {
                if (!Grounded)
                {
                    Rotate();

                    if (Keyboard.current[ThurstKey].isPressed)
                    {
                        // Apply thurst opposite 
                        applyingThrust = true;
                    }
                    if (Keyboard.current[ThurstKey].wasReleasedThisFrame)
                    {
                        applyingThrust = false;
                    }
                }
                else
                {

                    if (Keyboard.current[leftKey].isPressed)
                    {
                        transform.RotateAround(transform.parent.transform.position, Vector3.back, MovementSpeed * Time.deltaTime);
                    }
                    else if (Keyboard.current[RightKey].isPressed)
                    {
                        transform.RotateAround(transform.parent.transform.position, Vector3.forward, MovementSpeed * Time.deltaTime);
                        //movement.x = 1;
                    }
                    else
                    {
                        //movement.x = 0;
                    }


                    if (Keyboard.current[ThurstKey].wasPressedThisFrame)
                    {
                        chargingForLaunch?.Invoke();
                    }

                    // Check input
                    if (Keyboard.current[ThurstKey].isPressed)
                    {
                        // Thrust bar filling
                        AccumulatedForce += AccumulationFillingForce;
                        // Accumulating Force

                    }

                    if (Keyboard.current[ThurstKey].wasReleasedThisFrame)
                    {
                        launch = true;
                        Launched?.Invoke();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (applyingThrust)
            {
                ApplyThurst();
            }
            else if (launch)
            {
                var direction = GetDirection();
                // Aplying the accumulated Force
                Graviton.Rigidbody.constraints = RigidbodyConstraints2D.None;
                if (AccumulatedForce > 30) AccumulatedForce = 30;
                Graviton.Rigidbody.AddForce(AccumulatedForce * direction.normalized);
                // Setting grounded to false.
                Grounded = false;
                launch = false;
                //Debug.Log("Grounded set to " + _grounded);
            }
        }

        private Vector3 GetDirection() => transform.position - Graviton.AttractedBy.transform.position;

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
                Quaternion dirQ = Quaternion.LookRotation (-velocity);
                Quaternion slerp = Quaternion.Slerp (transform.rotation, dirQ, velocity.magnitude * RotationSpeed * Time.deltaTime);
                Graviton.Rigidbody.MoveRotation(slerp);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Grounded = true;
            Graviton.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            AccumulatedForce = 10;
            //transform.LookAt(GetDirection(),);
            //Debug.Log("Grounded set to " + _grounded);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            Grounded = false;
            Graviton.Rigidbody.constraints = RigidbodyConstraints2D.None;
            //Debug.Log("Grounded set to " + _grounded);
        }
    }
}
