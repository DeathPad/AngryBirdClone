using ProgrammingBatch.AngryBirdClone.Logic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace ProgrammingBatch.AngryBirdClone.Component
{
    public class EnemyComponent : MonoBehaviour
    {
        private bool _isHit = false;

        public void SetEnemyData(string enemy)
        {
            Type _enemyType = Type.GetType($"ProgrammingBatch.AngryBirdClone.Logic.{enemy}, Assembly-CSharp");
            Enemy _enemyObject = (Enemy)Activator.CreateInstance(_enemyType);

            _enemy = _enemyObject;
            _currentHealth = _enemy.GetHealth();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

            if (col.gameObject.tag == "Bird")
            {
                _isHit = true;
                Destroy(gameObject);
            }
            else if (col.gameObject.tag == "Obstacle")
            {
                //Hitung damage yang diperoleh
                float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
                _currentHealth -= damage;

                if (_currentHealth <= 0)
                {
                    _isHit = true;
                    Destroy(gameObject);
                }
            }
        }

        private Enemy _enemy;

        private float _currentHealth;
    }
}