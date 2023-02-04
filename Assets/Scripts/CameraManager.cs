using Cinemachine;
using UnityEngine;

namespace Rudrac.GGJ2023
{
    public class CameraManager : MonoBehaviour
    {
        public CinemachineVirtualCamera NormalCamera;
        public CinemachineVirtualCamera LaunchCamera;

        private void Start()
        {
            JumpForceChance.ChargingForLaunch += Charging;
            JumpForceChance.Launched += Launched;
        }

        private void OnDestroy()
        {
            JumpForceChance.ChargingForLaunch -= Charging;
            JumpForceChance.Launched -= Launched;
        }

        private void Charging()
        {
            NormalCamera.enabled = false;
            LaunchCamera.enabled = true;
        }

        private void Launched(bool state)
        {
            LaunchCamera.enabled = false;
            NormalCamera.enabled = true;
        }
    }
}
