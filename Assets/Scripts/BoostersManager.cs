using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rudrac.GGJ2023
{
    public class BoostersManager : MonoBehaviour
    {
        public static Action BoosterCollected;

        public Image[] Boosters;
        public static int BoosterCount = 5;

        private void Start()
        {
            BoosterCount = Boosters.Length;
            JumpForceChance.Launched += BoosterUsed;
            BoosterCollected += BoosterCollectedMethod;
        }

        private void OnDestroy()
        {
            JumpForceChance.Launched -= BoosterUsed;
            BoosterCollected -= BoosterCollectedMethod;
        }

        private void BoosterCollectedMethod()
        {
            BoosterCount++;
            BoosterCountChanged();
        }

        private void BoosterUsed(bool obj)
        {
            BoosterCount--;
            BoosterCountChanged();
        }

        public void BoosterCountChanged()
        {
            for (int i = 0; i < Boosters.Length; i++)
            {
                if (i < BoosterCount)
                {
                    Boosters[i].color = Color.yellow;
                }
                else
                {
                    Boosters[i].color = Color.black;
                }
            }
        }

    }
}
