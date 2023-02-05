using Rudrac.GGJ2023;
using UnityEngine;

public class RotateArround : MonoBehaviour
{
    public Transform Pivot;
    public Vector2 RotationSpeedRange;
    private float RotationSpeed;
    private void Start()
    {
        RotationSpeed = Random.Range(RotationSpeedRange.x, RotationSpeedRange.y);
        RotationSpeed = Random.value > 0.5f ? RotationSpeed : -RotationSpeed;
    }
    void Update()
    {
        transform.RotateAround(Pivot.position, Vector3.back, RotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Scenesmanager.instance.ReloadScene();
        }
    }

}
