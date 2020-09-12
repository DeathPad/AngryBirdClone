using ProgrammingBatch.AngryBirdClone.Core;
using ProgrammingBatch.AngryBirdClone.Handler;
using ProgrammingBatch.AngryBirdClone.Logic;
using System;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone.Component
{
    public sealed class KillerColliderComponent : MonoBehaviour
    {
        public void OnInitialize(LevelManager levelManager, GameStateHandler gameStateHandler)
        {
            _levelManager = levelManager;
            _gameStateHandler = gameStateHandler;
            _gameStateHandler.HandleEvent -= OnGameStateChanged;
            _gameStateHandler.HandleEvent += OnGameStateChanged;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Bird")
            {
                _levelManager.ReturnBirdComponentToPool(collision.gameObject.GetComponent<BirdComponent>());
                return;
            }

            if(collision.tag == "Obstacle")
            {
                Destroy(collision.gameObject);
                return;
            }
        }

        private void OnGameStateChanged(GameEnum gameEnum)
        {
            switch(gameEnum)
            {
                case GameEnum.Idle:
                case GameEnum.Lose:
                case GameEnum.MaxLevel:
                    gameCollider2D.enabled = false;
                    break;

                case GameEnum.Play:
                    gameCollider2D.enabled = true;
                    break;
            }
        }

        private void OnDestroy()
        {
            try
            {
                _gameStateHandler.HandleEvent -= OnGameStateChanged;
            } catch(Exception)
            {
                Debug.LogWarning("[KillerColliderComponent] No event to unsubscribe");
            }
        }

        [SerializeField] private Collider2D gameCollider2D = default;

        private LevelManager _levelManager;
        private GameStateHandler _gameStateHandler;
    }
}