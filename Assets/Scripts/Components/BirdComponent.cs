using ProgrammingBatch.AngryBirdClone.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ProgrammingBatch.AngryBirdClone.Component
{
    public sealed class BirdComponent : MonoBehaviour
    {
        public void OnInitialize(LevelManager levelManager, Bird identity)
        {
            _levelManager = levelManager;
            _identity = identity;
        }

        private void Start()
        {
            SetStoic();
            state = BirdStateEnum.Idle;
        }

        private void FixedUpdate()
        {
            if (state == BirdStateEnum.Idle &&
                rigidBody.velocity.sqrMagnitude >= _minVelocity)
            {
                state = BirdStateEnum.Thrown;
            }

            if ((state == BirdStateEnum.Thrown || state == BirdStateEnum.HitSomething) &&
                rigidBody.velocity.sqrMagnitude < _minVelocity &&
                !_flagDestroy)
            {
                //Hancurkan gameobject setelah 2 detik
                //jika kecepatannya sudah kurang dari batas minimum
                _flagDestroy = true;
                StartCoroutine(DestroyAfter(2));
            }
        }

        public void MoveTo(Vector2 target, GameObject parent)
        {
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = target;
        }

        public void Shoot(Vector2 velocity, float distance, float speed)
        {
            birdCollider.enabled = true;
            rigidBody.bodyType = RigidbodyType2D.Dynamic;
            rigidBody.velocity = velocity * speed * distance;
            OnBirdShot(this);
        }

        public void SetStoic()
        {
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
            birdCollider.enabled = false;
        }

        private IEnumerator DestroyAfter(float second)
        {
            yield return new WaitForSeconds(second);
            _levelManager.ReturnBirdComponentToPool(this);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            state = BirdStateEnum.HitSomething;

            var dir = (this.transform.position - col.transform.position);
            float wearoff = 1 - (dir.magnitude / 10);
            rigidBody.AddForce(dir.normalized * 1000 * wearoff);

        }

#pragma warning disable IDE0051 // Remove unused private members
        [SerializeField] private GameObject parent = default;
#pragma warning restore IDE0051 // Remove unused private members
        [SerializeField] private Rigidbody2D rigidBody = default;
        [SerializeField] private CircleCollider2D birdCollider = default;
        public BirdStateEnum state;
        private float _minVelocity = 0.05f;
        private bool _flagDestroy = false;

        private LevelManager _levelManager;
        private Bird _identity;

        public UnityAction<BirdComponent> OnBirdShot = delegate { };
    }
}