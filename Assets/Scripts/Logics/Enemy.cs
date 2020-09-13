namespace ProgrammingBatch.AngryBirdClone.Logic
{
    public class Enemy
    {
        public virtual float GetHealth()
        {
            return health;
        }

        protected float health = 50;
    }
}