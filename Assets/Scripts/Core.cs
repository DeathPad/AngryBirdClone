using ProgrammingBatch.AngryBirdClone.Handler;
using ProgrammingBatch.AngryBirdClone.Logic;
using UnityEngine;

namespace ProgrammingBatch.AngryBirdClone.Core
{
    public class Core
    {
        public static bool IsInitialized { get; private set; }
        
        //Core Modules
        public LevelManager LevelManager { get; private set; }

        //Game Modules
        public GameStateHandler GameStateHandler { get; private set; }
        internal void Init()
        {
            LoadCoreModules();
            LoadGameModules();
            IsInitialized = true;
        }

        protected virtual void LoadCoreModules() 
        {
            LevelManager = new LevelManager();
        }
        protected virtual void LoadGameModules() 
        {
            GameStateHandler = new GameStateHandler(LevelManager);
        }

    }
}