using ProgrammingBatch.AngryBirdClone.Component;
using ProgrammingBatch.AngryBirdClone.Core;
using ProgrammingBatch.AngryBirdClone.Handler;
using System.Collections.Generic;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone
{
    public sealed class AngryBirdSceneController : SceneController
    {
        public Core.Core GameCore { get; private set; }

        public override void StartCompleted()
        {
            GameCore.GameStateHandler.TriggerEvent(GameEnum.Play);
        }

        /// <summary>
        /// Initialize all script that attached to gameobjects(component)
        /// </summary>
        public override void InitializeSceneComponents()
        {
            slingShotComponent.OnInitialize(GameCore.GameStateHandler, _slingHandler as SlingHandler);
            killerColliderList.ForEach(x =>
            {
                x.OnInitialize(GameCore.LevelManager, GameCore.GameStateHandler);
            });
        }

        /// <summary>
        /// allowed to do abstract proccess here
        /// </summary>
        public override void OnStartInitialize()
        {
            GameCore = Core;

            bool _isLevelAvailable = GameCore.GameStateHandler.LoadLevel();
            if (_isLevelAvailable is false)
            {
                GameCore.GameStateHandler.TriggerEvent(GameEnum.MaxLevel);
                return;
            }
            GenerateLevel(GameCore.GameStateHandler.GetLevelData());
            _slingHandler = new SlingHandler(GameCore.LevelManager, GameCore.GameStateHandler, trailComponent);
        }

        private void GenerateLevel(LevelData levelData)
        {
            foreach (ObstaclesData _obstacle in levelData.obstacles)
            {
                GameObject _obstacleGameObject = Instantiate(woodsPrefab, _obstacle.position, _obstacle.rotationData.CreateQuaternion());
                _obstacleGameObject.transform.parent = woods.transform;
            }
        }

        [Space]
        [SerializeField] private GameObject woodsPrefab = default;

        [Space]
        [SerializeField] private GameObject woods = default;

        [SerializeField] private List<KillerColliderComponent> killerColliderList = default;

        [Space]
        [SerializeField] private SlingShotComponent slingShotComponent = default;
        [SerializeField] private TrailComponent trailComponent = default;

        private IHandler _slingHandler;
    }
}