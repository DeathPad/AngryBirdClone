using ProgrammingBatch.AngryBirdClone.Core;
using ProgrammingBatch.AngryBirdClone.Handler;
using System;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone.Component
{
    public sealed class SlingShotComponent : MonoBehaviour
    {
        public LineRenderer Trajectory;

        public void OnInitialize(GameStateHandler gameStateHandler, SlingHandler slingHandler)
        {
            _gameStateHandler = gameStateHandler;
            _gameStateHandler.HandleEvent -= OnGameStateChanged;
            _gameStateHandler.HandleEvent += OnGameStateChanged;

            _slingHandler = slingHandler;
            _slingHandler.HandleEvent -= OnInitiateSlingEvent;
            _slingHandler.HandleEvent += OnInitiateSlingEvent;
        }

        public void OnInitiateSlingEvent(object bird = null)
        {
            if (!(bird is BirdComponent _birdComponentTemp))
            {
                return;
            }
            _birdComponent = _birdComponentTemp;
            _birdComponent.MoveTo(gameObject.transform.position, gameObject);

            slingCollider.enabled = true;
        }


        void DisplayTrajectory(float distance)
        {
            if (_birdComponent == null)
            {
                return;
            }

            Vector2 velocity = _startPos - (Vector2)transform.position;
            int segmentCount = 5;
            Vector2[] segments = new Vector2[segmentCount];

            // Posisi awal trajectoy merupakan posisi mouse dari player saat ini
            segments[0] = transform.position;

            // Velocity awal
            Vector2 segVelocity = velocity * throwSpeed * distance;

            for (int i = 1; i < segmentCount; i++)
            {
                float elapsedTime = i * Time.fixedDeltaTime * 5;
                segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
            }

            Trajectory.positionCount = segmentCount;
            for (int i = 0; i < segmentCount; i++)
            {
                Trajectory.SetPosition(i, segments[i]);
            }
        }
        private void OnDestroy()
        {
            try
            {
                _gameStateHandler.HandleEvent -= OnGameStateChanged;
                _slingHandler.HandleEvent -= OnInitiateSlingEvent;
            }
            catch (NullReferenceException)
            {
                Debug.LogError("[SlingShotComponent] No event to unsubscribe");
            }
        }

        private void Start()
        {
            _startPos = transform.position;
        }

        private void OnGameStateChanged(GameEnum gameEnum)
        {
            _gameEnum = gameEnum;
            switch (_gameEnum)
            {
                case GameEnum.Idle:
                    _startPos = transform.position;
                    break;

                case GameEnum.Play:
                    break;

                case GameEnum.Lose:
                    break;
            }
        }
        private void OnMouseDrag()
        {
            if (_gameEnum != GameEnum.Play)
            {
                return;
            }

            // Mengubah posisi mouse ke world position
            Vector2 _mouseToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Hitung supaya 'karet' ketapel berada dalam radius yang ditentukan
            Vector2 _slingDirection = _mouseToWorldPoint - _startPos;
            if (_slingDirection.sqrMagnitude > radius)
            {
                _slingDirection = _slingDirection.normalized * radius;
            }

            float _distance = Vector2.Distance(_startPos, transform.position);
            transform.position = _startPos + _slingDirection;

            if (!Trajectory.enabled)
            {
                Trajectory.enabled = true;
            }

            DisplayTrajectory(_distance);
        }

        private void OnMouseUp()
        {
            if (_gameEnum != GameEnum.Play)
            {
                return;
            }

            slingCollider.enabled = false;
            Vector2 _velocity = _startPos - (Vector2)transform.position;
            float _distance = Vector2.Distance(_startPos, transform.position);

            _birdComponent.Shoot(_velocity, _distance, throwSpeed);
            gameObject.transform.position = _startPos;
            _slingHandler.TriggerEvent();
        }

        [SerializeField] private CircleCollider2D slingCollider = default;

        [Space]
        [SerializeField] private float radius = 0.75f;

        [SerializeField] private float throwSpeed = 30f;

        private Vector2 _startPos;

        private BirdComponent _birdComponent;

        private GameStateHandler _gameStateHandler;
        private GameEnum _gameEnum;

        private SlingHandler _slingHandler;
    }
}