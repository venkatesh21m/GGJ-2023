using Cinemachine;
using UnityEngine;

namespace Rudrac.GGJ2023
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineVirtualCamera NormalCamera;
        public CinemachineVirtualCamera LaunchCamera;
        public CinemachineVirtualCamera TravellingCamera;
        public CinemachineVirtualCamera MapCamera;

        private void Start()
        {
            JumpForceChance.ChargingForLaunch += Charging;
            JumpForceChance.Launched += Launched;
            Player.GroundedEvent += Grounded;
            Player.MapCameraState += MapCameraState;
        }

        private void OnDestroy()
        {
            JumpForceChance.ChargingForLaunch -= Charging;
            JumpForceChance.Launched -= Launched;
            Player.GroundedEvent -= Grounded;
            Player.MapCameraState -= MapCameraState;
        }

        private void MapCameraState(bool obj)
        {
            TravellingCamera.enabled = false;
            LaunchCamera.enabled = false;
            if (obj)
            {
                NormalCamera.enabled = false;
                MapCamera.enabled = true;
            }
            else
            {
                NormalCamera.enabled = true;
                MapCamera.enabled = false;
            }
        }

        private void Grounded()
        {
            NormalCamera.enabled = true;
            TravellingCamera.enabled = false;
            LaunchCamera.enabled = false;
            MapCamera.enabled = false;
        }

        private void Charging()
        {
            NormalCamera.enabled = false;
            TravellingCamera.enabled = false;
            LaunchCamera.enabled = true;
            MapCamera.enabled = false;
        }

        private void Launched(bool state)
        {
            LaunchCamera.enabled = false;
            MapCamera.enabled = false;
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
