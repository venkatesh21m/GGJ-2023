using UnityEngine;
using UnityEngine.UI;

namespace Rudrac.GGJ2023
{
    public class ThrustManager : MonoBehaviour
    {
        public float MaxThrust;
        public float ThrustUsageRate;
        public Slider ThrustSlider;

        [Space]
        public static float CurrentThrust;

        private void Start()
        {
            CurrentThrust = MaxThrust;
            ThrustSlider.maxValue = MaxThrust;
            ThrustSlider.value = MaxThrust;

            Player.UsingThrust += UsingThrust;
        }

        private void OnDestroy() => Player.UsingThrust -= UsingThrust;

        public void UsingThrust()
        {
            CurrentThrust -= ThrustUsageRate;
            ThrustSlider.value = CurrentThrust;
        }

    }
}