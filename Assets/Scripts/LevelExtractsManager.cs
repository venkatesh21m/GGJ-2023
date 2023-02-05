using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rudrac.GGJ2023
{
    public class LevelExtractsManager : MonoBehaviour
    {
        public static int LevelExtractsCount;
        public static Action LevelExtractCollected;
        public Image[] LevelExtracts;

        private void Start()
        {
            LevelExtractsCount = 0;
            LevelExtractCollected += LevelExtractCollectedMethod;
            SetUpLevelExtracts();
        }

        private void OnDestroy()
        {
            LevelExtractCollected -= LevelExtractCollectedMethod;
        }

        private void LevelExtractCollectedMethod()
        {
            LevelExtractsCount++;
            SetUpLevelExtracts();
        }

        private void SetUpLevelExtracts()
        {
            for (int i = 0; i < LevelExtracts.Length; i++)
            {
                if (i < LevelExtractsCount)
                {
                    LevelExtracts[i].color = Color.white;
                }
                else
                {
                    LevelExtracts[i].color = Color.black;
                }
            }
        }
    }
}