namespace ProgrammingBatch.AngryBirdClone.Component
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TrailComponent : MonoBehaviour
    {
        public GameObject trail;
        

        private List<GameObject> _trails = new List<GameObject>();

        public void SetBird(BirdComponent bird)
        {
            _targetBird = bird;

            for (int i = 0; i < _trails.Count; i++)
            {
                Destroy(_trails[i].gameObject);
            }

            _trails.Clear();
        }

        private IEnumerator InstantiateTrails()
        {
            while (_targetBird != null && _targetBird.state != BirdStateEnum.HitSomething)
            {
                yield return new WaitForSeconds(0.1f);
                _trails.Add(Instantiate(trail, _targetBird.transform.position, Quaternion.identity));
            }
        }

        public void SpawnTrail()
        {
            StartCoroutine(InstantiateTrails());
        }

        private BirdComponent _targetBird;
    }
}