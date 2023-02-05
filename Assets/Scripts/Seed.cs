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
        public SpriteRenderer Booster;
        private Graviton currentPlanet;
        public bool LevelExtract;


        // Start is called before the first frame update
        private void Start()
        {
            currentPlanet = Player.instance.Graviton;
            //SetRotation();
            //StartCoroutine(StartGrowing());
            StartCoroutine(StartGrowingProcedural());
        }

        private IEnumerator StartGrowingProcedural()
        {
            List<Vector3> points = GetBaseRootPoints();

            TrailRenderers[0].gameObject.SetActive(true);
            TrailRenderers[0].transform.localPosition = points[0];
            int j = 0;
            for (int i = 0; i < points.Count; i++)
            {
                var pos = TrailRenderers[0].transform.TransformPoint(points[i]);
                TrailRenderers[0].transform.DOMove(pos, 1).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1);
                if (i < 5)
                {
                    if (Random.value > 0.5f)
                    {
                        j++;
                        var _points = GetPoints(points[i]);
                        StartCoroutine(StartGrowingSubRootProcedural(j, _points));
                    }
                }
            }

            StartSpawningBooster();
        }


        private IEnumerator StartGrowingSubRootProcedural(int k, List<Vector3> points)
        {
            yield return new WaitForSeconds(1.5f);
            TrailRenderers[k].gameObject.SetActive(true);
            TrailRenderers[k].transform.localPosition = points[0];
            for (int i = 0; i < points.Count; i++)
            {
                var pos = TrailRenderers[k].transform.TransformPoint(points[i]);
                TrailRenderers[k].transform.DOMove(pos, 1).SetEase(Ease.Linear);
                yield return new WaitForSeconds(1);
            }
        }
        private void StartSpawningBooster()
        {
            Booster.color = Color.gray;
            Booster.gameObject.SetActive(true);
            Booster.transform.DOScale(0.5f, 5).OnComplete(() =>
            {
                Booster.color = Color.green;
            });

        }

        List<Vector3> points = new List<Vector3>();
        private List<Vector3> GetBaseRootPoints()
        {
            Vector3 pos = transform.localPosition;
            points = new List<Vector3>(){
                pos,
            };

            foreach (var item in GetPoints(pos))
            {
                points.Add(item);
            }

            return points;
        }

        private List<Vector3> GetPoints(Vector3 pos)
        {
            var points = new List<Vector3>();
            for (int i = 1; i < 7; i++)
            {
                var direction = (-transform.up) + (transform.right * Random.Range(-1f, 1f));
                direction.Normalize();
                direction = transform.TransformDirection(direction);
                pos += direction * Random.Range(-0.5f, 0.5f);
                points.Add(pos);
            }
            return points;
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


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            if (Booster.gameObject.activeSelf && Booster.color == Color.green)
            {
                if (LevelExtract)
                {
                    LevelExtractsManager.LevelExtractCollected?.Invoke();
                }
                else if (BoostersManager.BoosterCount == 5)
                {
                    ThrustManager.ThrustIncreased?.Invoke();
                }
                else
                {
                    BoostersManager.BoosterCollected?.Invoke();
                }

                Booster.gameObject.SetActive(false);
                Booster.transform.localScale = Vector3.one * 0.2f;
                Invoke(nameof(StartSpawningBooster), 5);
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
