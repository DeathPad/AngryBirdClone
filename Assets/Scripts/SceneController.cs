using UnityEngine;
using UnityEngine.EventSystems;

namespace ProgrammingBatch.AngryBirdClone.Core
{
    public abstract class SceneController : MonoBehaviour
    {
        public static Core Core { get; private set; }

        [SerializeField] private EventSystem eventSystem = default;

        /// <summary>
        /// Initialize all script that attached to gameobjects(component)
        /// </summary>
        public virtual void InitializeSceneComponents() { }
        
        /// <summary>
        /// allowed to do abstract proccess here
        /// </summary>
        public virtual void OnStartInitialize() { }
        
        /// <summary>
        /// Show disable visual effect in Introduction function or disable unused scripts
        /// </summary>
        public virtual void StartCompleted() { }

        protected virtual void Start()
        {
            eventSystem.enabled = false; //dont allow input while core not finished initializing

            if(!Core.IsInitialized)
            {
                Core = new Core();
                Core.Init();
            }
            OnStartInitialize();
            InitializeSceneComponents();
            StartCompleted();

            eventSystem.enabled = true;
        }

        private void Update()
        {
        }
    }
}