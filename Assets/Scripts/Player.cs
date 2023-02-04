using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rudrac.GGJ2023
{
    public class Player : MonoBehaviour
    {
        public static Player instance;


        public static event Action UsingThrust;

        public Graviton Graviton;
        public Key ThurstKey = Key.Space;
        public Key leftKey = Key.A;
        public Key RightKey = Key.D;
        public float ThurstMagnitude = 1.0f;
        public float RotationSpeed = 1.0f;
        public float MovementSpeed = 1.0f;
        [Space]
        public float MaxAccumulatedForce = 10;
        public float MinAccumulatedForce = 10;
        public float AccumulationFillingForce = 0.2f;

        public bool Grounded { get; set; }

        private bool applyingThrust = false;

        private void Awake() => instance = this;

        private void Start()
        {
            JumpForceChance.Launched += Launch;
        }

        private void OnDestroy()
        {
            JumpForceChance.Launched -= Launch;

        }


        private void Update()
        {
            if (Graviton.BeingAttractedBy)
            {
                if (!Grounded)
                {
                    //Rotate();

                    if (Keyboard.current[ThurstKey].isPressed)
                    {
                        // Apply thurst opposite 
                        applyingThrust = true;
                    }

                    if (Keyboard.current[ThurstKey].wasReleasedThisFrame)
                    {
                        applyingThrust = false;
                    }

                    //if (Keyboard.current[leftKey].isPressed)
                    //{
                    //    RotateInAir(-1);
                    //}
                    //else if (Keyboard.current[RightKey].isPressed)
                    //{
                    //    RotateInAir(1);
                    //    //movement.x = 1;
                    //}

                }
                else
                {
                    applyingThrust = false;
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

                    if (Keyboard.current[ThurstKey].wasReleasedThisFrame)
                    {

                        //Launched?.Invoke();
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

        }

        private void Launch(bool obj)
        {
            Vector3 direction = GetDirection();
            // Aplying the accumulated Force
            Graviton.Rigidbody.constraints = RigidbodyConstraints2D.None;
            Graviton.Rigidbody.AddForce((obj ? MaxAccumulatedForce : MinAccumulatedForce) * direction.normalized);
            // Setting grounded to false.
            Grounded = false;

        }

        private Vector3 GetDirection() => transform.position - Graviton.AttractedBy.transform.position;

        public void ApplyThurst()
        {
            if (Graviton.AttractedBy == null)
            {
                return;
            }

            if (ThrustManager.CurrentThrust <= 0)
            {
                return;
            }

            Vector3 force =  GravityHandler.GetForceFactor(Graviton.AttractedBy.Rigidbody, Graviton.Rigidbody, true);
            Graviton.Rigidbody.AddForce(force * ThurstMagnitude);

            UsingThrust?.Invoke();
        }

        public IEnumerator RotateCharacter()
        {
            _ = Vector3.one;
            while (Graviton.AttractedBy != null)
            {
                //if (velocity.magnitude > 0.1)
                //{
                Vector3 vectorToTarget = Graviton.AttractedBy.transform.position - transform.position;

                // rotate that vector by 90 degrees around the Z axis
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;

                // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
                // (resulting in the X axis facing the target)
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

                // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
                Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed);
                Graviton.Rigidbody.MoveRotation(rot);

                yield return new WaitForEndOfFrame();
            }
        }

        public void RotateInAir(int x)
        {
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * transform.right * x;

            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed);
            Graviton.Rigidbody.MoveRotation(rot);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Grounded = true;
            Graviton.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            Graviton.Rigidbody.velocity = Vector2.zero;
            //transform.LookAt(GetDirection(),);
            //Debug.Log("Grounded set to " + Grounded);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            Grounded = false;
            Graviton.Rigidbody.constraints = RigidbodyConstraints2D.None;
            //Debug.Log("Grounded set to " + Grounded);
        }
    }
}
