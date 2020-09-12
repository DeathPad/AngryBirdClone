using ProgrammingBatch.AngryBirdClone.Component;
using ProgrammingBatch.AngryBirdClone.Core;
using ProgrammingBatch.AngryBirdClone.Logic;
using ProgrammingBatch.FlappyBirdClone.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone.Handler
{
    public sealed class SlingHandler : IHandler
    {
        public event OnEventHandler HandleEvent;

        public SlingHandler(LevelManager levelManager, GameStateHandler gameStateHandler, TrailComponent trailComponent)
        {
            _levelManager = levelManager;
            
            _gameStateHandler = gameStateHandler;
            _gameStateHandler.HandleEvent -= OnGameStateHandler;
            _gameStateHandler.HandleEvent += OnGameStateHandler;

            _trailComponent = trailComponent;
        }

        public void TriggerEvent(object value = null)
        {
            BirdComponent _birdComponent = _levelManager.GetBirdComponentFromPool();
            Bird _bird = _gameStateHandler.GetNextBird();
            if (_birdComponent is null ||
                _bird is null)
            {
                return;
            }
            _birdComponent.OnInitialize(_levelManager, _bird);
            _birdComponent.OnBirdShot += AssignTrail;
            HandleEvent?.Invoke(_birdComponent);
        }

        public void AssignTrail(BirdComponent bird)
        {
            _trailComponent.SetBird(bird);
            _trailComponent.SpawnTrail();
        }

        private void OnGameStateHandler(GameEnum gameEnum)
        {
            switch(gameEnum)
            {
                case GameEnum.Idle:
                    break;

                case GameEnum.Play:
                    TriggerEvent();
                    break;
            }
        }

        ~SlingHandler()
        {
            try
            {
                _gameStateHandler.HandleEvent -= OnGameStateHandler;
            } catch(Exception)
            {
                Debug.LogWarning("[SlingHandler] No event to unsubscribe");
            }
        }

        private LevelManager _levelManager;
        private GameStateHandler _gameStateHandler;

        private TrailComponent _trailComponent;
    }
}