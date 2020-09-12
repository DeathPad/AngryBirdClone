namespace ProgrammingBatch.AngryBirdClone.Logic
{
    public class Enemy
    {
        public virtual int GetHealth()
        {
            return health;
        }

        protected int health = 50;
    }
}