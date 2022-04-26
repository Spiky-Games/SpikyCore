namespace EventSystem
{
    public interface Invoker
    {
        object Target { get; }

        void Invoke(GameEvent gameEvent);
    }

    public class SpecificInvoker<T> : Invoker where T : GameEvent
    {
        public object Target { get { return Handler; } }
        public System.Action<T> Handler { get; set; }

        public void Invoke(GameEvent gameEvent)
        {
            Handler.Invoke((T)gameEvent);
        }
    }

    public class ParameterlessInvoker : Invoker
    {
        public object Target { get { return Handler; } }
        public System.Action Handler { get; set; }

        public void Invoke(GameEvent gameEvent)
        {
            Handler.Invoke();
        }
    }
}
