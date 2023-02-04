using Cinemachine;
using UnityEngine;

namespace Rudrac.GGJ2023
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineVirtualCamera NormalCamera;
        public CinemachineVirtualCamera LaunchCamera;
        public CinemachineVirtualCamera TravellingCamera;

        private void Start()
        {
            JumpForceChance.ChargingForLaunch += Charging;
            JumpForceChance.Launched += Launched;
            Player.GroundedEvent += Grounded;
        }

        private void OnDestroy()
        {
            JumpForceChance.ChargingForLaunch -= Charging;
            JumpForceChance.Launched -= Launched;
            Player.GroundedEvent -= Grounded;
        }

        private void Grounded()
        {
            NormalCamera.enabled = true;
            TravellingCamera.enabled = false;
            LaunchCamera.enabled = false;
        }

        private void Charging()
        {
            NormalCamera.enabled = false;
            TravellingCamera.enabled = false;
            LaunchCamera.enabled = true;
        }

        private void Launched(bool state)
        {
            LaunchCamera.enabled = false;
            if (state)
            {
                TravellingCamera.enabled = true;
                NormalCamera.enabled = false;
            }
            else
            {
                TravellingCamera.enabled = false;
                NormalCamera.enabled = true;
            }
        }
    }
}
