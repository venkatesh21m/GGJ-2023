using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Rudrac.GGJ2023
{
    public class Seed : MonoBehaviour
    {


        public List<TrailRenderer> TrailRenderers;
        public List<RootPoints> RootPoints;

        private Graviton currentPlanet;

        // Start is called before the first frame update
        private void Start()
        {
            currentPlanet = Player.instance.Graviton;
            //SetRotation();
            StartCoroutine(StartGrowing());
        }

        private void SetRotation()
        {
            Vector3 vectorToTarget = currentPlanet.transform.position - transform.position;

            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;

            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            transform.rotation = targetRotation;
            // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
            //Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetRotation, 50);
            //Graviton.Rigidbody.MoveRotation(rot);
        }

        private IEnumerator StartGrowing()
        {
            var rootPoins = RootPoints[Random.Range(0,RootPoints.Count-1)];
            int i = 0;
            var points = rootPoins.MainRootPos;
            TrailRenderers[i].transform.position = points[0].position;
            TrailRenderers[i].gameObject.SetActive(true);
            for (int j = 0; j < points.Length; j++)
            {
                TrailRenderers[0].transform.DOMove(points[j].position, 1).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1);

                if (rootPoins.SubRootPosition.Any(x => x.index == j))
                {
                    i++;
                    StartCoroutine(StartSubRoot(i, rootPoins.SubRootPosition.Find(x => x.index == j)));
                }
            }
        }

        private IEnumerator StartSubRoot(int i, SubRoots subroot)
        {
            yield return new WaitForSeconds(2);
            TrailRenderers[i].transform.position = subroot.RootPos[0].position;
            TrailRenderers[i].gameObject.SetActive(true);
            foreach (var item in subroot.RootPos)
            {
                TrailRenderers[i].transform.DOMove(item.position, 1);
                yield return new WaitForSeconds(1);
            }
        }
    }
    [System.Serializable]
    public struct RootPoints
    {
        public Transform[] MainRootPos;
        public List<SubRoots> SubRootPosition;

    }
    [System.Serializable]
    public struct SubRoots
    {
        public int index;
        public Transform[] RootPos;
    }
}
