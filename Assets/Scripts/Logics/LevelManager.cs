using ProgrammingBatch.AngryBirdClone.Component;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone.Logic
{
    public sealed class LevelManager
    {
        public LevelManager()
        {
            _loadedLevels = Resources.LoadAll<TextAsset>("Levels").ToList();
            CreateBirdPool();
        }

        public TextAsset GetLevel()
        {
            TextAsset _levelAsset = _loadedLevels[0];
            _loadedLevels.Remove(_levelAsset);
            return _levelAsset;
        }

        public BirdComponent GetBirdComponentFromPool()
        {
            if(_birdPool.Count <= 0)
            {
                Debug.LogWarning("[BirdComponent] Pool is empty");
                return null;
            }

            BirdComponent _birdComponent = _birdPool[0];
            _birdPool.Remove(_birdComponent);

            return _birdComponent;
        }

        public void ReturnBirdComponentToPool(BirdComponent birdComponent)
        {
            birdComponent.SetStoic();
            birdComponent.transform.position = Vector3.one * OUT_OF_SCREEN_POSITION;
            birdComponent.transform.parent = _poolGameObject.transform;

            _birdPool.Add(birdComponent);
        }

        private void CreateBirdPool()
        {
            _poolGameObject = new GameObject("Bird Pool");
            GameObject.DontDestroyOnLoad(_poolGameObject);

            BirdComponent _birdComponent = Resources.Load<BirdComponent>("Bird");

            Vector3 _outOfScreenVector = new Vector3(OUT_OF_SCREEN_POSITION, OUT_OF_SCREEN_POSITION, OUT_OF_SCREEN_POSITION);
            for (int i = 0; i < MAX_BIRD_POOL; i++)
            {
                BirdComponent _birdGameObject = GameObject.Instantiate(_birdComponent, _outOfScreenVector, Quaternion.identity);
                _birdGameObject.transform.parent = _poolGameObject.transform;

                _birdPool.Add(_birdGameObject);
            }
        }

        private const float OUT_OF_SCREEN_POSITION = 999f;
        private const int MAX_BIRD_POOL = 3;

        private static GameObject _poolGameObject;

        private List<TextAsset> _loadedLevels = new List<TextAsset>();
        private List<BirdComponent> _birdPool = new List<BirdComponent>();
    }
}