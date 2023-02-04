using UnityEngine;

namespace Rudrac.GGJ2023
{
    [AddComponentMenu("GGJ2023/Graviton")]
    public class Graviton : MonoBehaviour
    {

        [SerializeField] private bool _isAttractee;//field
        [SerializeField] private bool _isAttractor;//field

        [SerializeField] private Vector3 _initialVelocity;
        [SerializeField] private bool _applyInitialVelocityOnStart;
        [SerializeField] private float _gravityRadius;
        [SerializeField] private bool _LevelFinish;

        [Header("For Debug")]
        public bool BeingAttractedBy = false;
        public Graviton AttractedBy = null;
        public CircleCollider2D Trigger;
        public LineRenderer CircleRenderer;

        public GameObject[] Seeds;
        [HideInInspector] public int SeedsCount =0;
        public bool IsAttractee//property 
        {
            get => _isAttractee;
            set
            {
                if (value == true)
                {
                    if (!GravityHandler.Attractees.Contains(this))
                    {
                        GravityHandler.Attractees.Add(this);
                    }
                }
                else if (value == false)
                {
                    _ = GravityHandler.Attractees.Remove(this);
                }

                _isAttractee = value;
            }
        }
        public bool IsAttractor//property
        {
            get => _isAttractor;
            set
            {
                if (value == true)
                {
                    //if (!GravityHandler.Attractors.Contains(this))
                    //    GravityHandler.Attractors.Add(this);
                }
                else if (value == false)
                {
                    //_ = GravityHandler.Attractors.Remove(this);
                }

                _isAttractor = value;
            }
        }

        public Rigidbody2D Rigidbody { get; private set; }
        public float GravityRadius => _gravityRadius;

        private void Awake() => Rigidbody = GetComponent<Rigidbody2D>();

        private void OnEnable()
        {
            IsAttractor = _isAttractor;
            IsAttractee = _isAttractee;
        }

        private void Start()
        {
            //initialVelocity = new Vector3(Random.Range(-10,10), Random.Range(-10,10), 0);
            if (_applyInitialVelocityOnStart)
            {
                ApplyVelocity(_initialVelocity);
            }
            if (CircleRenderer != null)
            {
                CircleRenderer.useWorldSpace = false;
                DrawCircle(60, GravityRadius / transform.lossyScale.x);
            }

            SeedsCount = Seeds.Length;
        }

        private void OnDisable()
        {
            _ = GravityHandler.Attractors.Remove(this);
            _ = GravityHandler.Attractees.Remove(this);
        }

        private void ApplyVelocity(Vector3 velocity) => Rigidbody.AddForce(_initialVelocity, ForceMode2D.Impulse);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GravityRadius);
        }

        private void OnValidate()
        {
            if (IsAttractee)
            {
                _gravityRadius = transform.lossyScale.x;
            }

            if (Trigger == null)
                return;
            Trigger.radius = GravityRadius / transform.lossyScale.x;


        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
                return;
            //Debug.Log("Player inside planet range", gameObject);
            if (!GravityHandler.Attractors.Contains(this))
                GravityHandler.Attractors.Add(this);
            GravityHandler.Attractees[0].AttractedBy = this;
            GravityHandler.Attractees[0].BeingAttractedBy = true;
            _ = StartCoroutine(Player.instance.RotateCharacter());


            if (_LevelFinish)
            {
                Debug.LogError("Game Over");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
                return;
            //Debug.Log("Player Outside planet range", gameObject);

            _ = GravityHandler.Attractors.Remove(this);
            GravityHandler.Attractees[0].AttractedBy = null;
            GravityHandler.Attractees[0].BeingAttractedBy = false;
            StopCoroutine(Player.instance.RotateCharacter());
        }




        public void DrawCircle(int steps, float radius)
        {
            CircleRenderer.positionCount = steps + 1;

            for (int i = 0; i <= steps; i++)
            {
                float circumferenceprogress = (float)i/steps;
                float radian = circumferenceprogress *2*Mathf.PI;
                float xScaled = Mathf.Cos(radian);
                float yScaled = Mathf.Sin(radian);

                float x = xScaled *radius;
                float y = yScaled *radius;

                Vector3 currentPos = new Vector3(x, y, 0);
                CircleRenderer.SetPosition(i, currentPos);
            }
        }
        internal void SpawnSeed()
        {
            SeedsCount--;
            var seed = Seeds[SeedsCount];

            // Rotate the seed to player pos
            Vector3 vectorToTarget = Player.instance.transform.position - seed.transform.position;

            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * vectorToTarget;

            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            seed.transform.rotation = targetRotation;

            // Enable it.
            seed.gameObject.SetActive(true);


        }
    }
}
