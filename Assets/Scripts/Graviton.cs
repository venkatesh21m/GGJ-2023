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

        [Header("For Debug")]
        public bool BeingAttractedBy = false;
        public Graviton AttractedBy = null;
        public CircleCollider2D Trigger;


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



    }
}
