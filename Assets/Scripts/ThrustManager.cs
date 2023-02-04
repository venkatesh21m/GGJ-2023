using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rudrac.GGJ2023
{
    public class ThrustManager : MonoBehaviour
    {
        public static Action ThrustIncreased;
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
            ThrustIncreased += ThrustIncreasedMethod;
        }

        private void OnDestroy()
        {
            Player.UsingThrust -= UsingThrust;
            ThrustIncreased -= ThrustIncreasedMethod;
        }

        private void ThrustIncreasedMethod()
        {
            CurrentThrust += 3;
            if (CurrentThrust > MaxThrust) CurrentThrust = MaxThrust;
            ThrustSlider.value = CurrentThrust;

        }

        public void UsingThrust()
        {
            CurrentThrust -= ThrustUsageRate;
            ThrustSlider.value = CurrentThrust;
        }

    }
}