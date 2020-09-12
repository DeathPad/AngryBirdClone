using Newtonsoft.Json;
using ProgrammingBatch.AngryBirdClone;
using ProgrammingBatch.Magnetize.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProgrammingBatch.Magnetize
{
    public sealed class LevelExporterComponent : MonoBehaviour
    {
        [SerializeField] private string levelName = default;

        [SerializeField] private GameObject woods = default;
        [SerializeField] private List<string> birds = default;
        [Space]
        [SerializeField] private bool isExport;

        private void Start()
        {
            if(!isExport)
            {
                return;
            }

            LevelData _levelData = new LevelData
            {
                obstacles = GetObstaclesData(),
                birds = this.birds
            };
            ExportToJSON(_levelData);
        }

        private List<ObstaclesData> GetObstaclesData()
        {
            List<ObstaclesData> _woodsData = new List<ObstaclesData>();
            foreach(Transform wood in woods.transform)
            {
                ObstaclesData _obstaclesData = new ObstaclesData()
                {
                    position = wood.position,
                    rotationData = new RotationData(wood.rotation)
                };

                _woodsData.Add(_obstaclesData);
            }
            return _woodsData;
        }

        private void ExportToJSON(LevelData levelData)
        {
            string _serializedLevelData = string.Empty;
            try
            {
                _serializedLevelData = JsonConvert.SerializeObject(levelData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to serialize level. {e}");
            }

            string _pathName = string.Empty;
#if UNITY_EDITOR
            _pathName = EditorUtility.OpenFolderPanel("Save created level", "Assets", "Levels");
#endif
            try
            {
                File.WriteAllText($"{_pathName}/{levelName}.json", _serializedLevelData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}