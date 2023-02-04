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
            Player.chargingForLaunch += Charging;
            Player.Launched += Launched;
        }

        private void OnDestroy()
        {
            Player.chargingForLaunch -= Charging;
            Player.Launched -= Launched;
        }

        private void Charging()
        {
            NormalCamera.enabled = false;
            LaunchCamera.enabled = true;
        }

        private void Launched()
        {
            LaunchCamera.enabled = false;
            NormalCamera.enabled = true;
        }
    }
}
