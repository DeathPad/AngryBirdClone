using Newtonsoft.Json;
using ProgrammingBatch.AngryBirdClone.Core;
using ProgrammingBatch.AngryBirdClone.Logic;
using System;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone.Handler
{
    public sealed class GameStateHandler
    {
        public event GameEventHandler HandleEvent;

        public GameStateHandler(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public bool LoadLevel()
        {
            TextAsset _textAsset = _levelManager.GetLevel();
            if(_textAsset is null)
            {
                return false;
            }

            _currentLevelData = JsonConvert.DeserializeObject<LevelData>(_textAsset.text);
            return true;
        }

        public LevelData GetLevelData()
        {
            return _currentLevelData;
        }

        public Bird GetNextBird()
        {
            string _birdTypeName = string.Empty;
            try
            {
                _birdTypeName = _currentLevelData.birds[0];
            } catch(ArgumentOutOfRangeException)
            {
                Debug.Log("[GameStateHandler] Out of bird");
            }
            if (string.IsNullOrEmpty(_birdTypeName))
            {
                return null;
            }
            _currentLevelData.birds.Remove(_birdTypeName);

            Type _birdType = Type.GetType($"ProgrammingBatch.AngryBirdClone.Logic.{_birdTypeName}, Assembly-CSharp", true);
            Bird _birdObject = (Bird)Activator.CreateInstance(_birdType);
            return _birdObject;
        }

        public void TriggerEvent(GameEnum gameEnum)
        {
            HandleEvent?.Invoke(gameEnum);
        }

        private LevelManager _levelManager;
        private LevelData _currentLevelData;
    }
}