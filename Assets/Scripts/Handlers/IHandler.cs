using ProgrammingBatch.FlappyBirdClone.Event;

namespace ProgrammingBatch
{
    public interface IHandler
    {
        event OnEventHandler HandleEvent;
        void TriggerEvent(object value = null);
    }
}   