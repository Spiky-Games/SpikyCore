using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSystem
{
    public class GameEventSystem
    {
        private readonly Dictionary<Type, List<Invoker>> handlers = new Dictionary<Type, List<Invoker>>();
        private readonly HashSet<List<Invoker>> toRemove = new HashSet<List<Invoker>>();
        private int dispatchingCounter;

        public void Clear()
        {
            handlers.Clear();
        }

        public void AddEventListener<T>(Action handler) where T : GameEvent
        {
            ParameterlessInvoker invoker = new ParameterlessInvoker
            {
                Handler = handler
            };

            AddInvoker(typeof(T), invoker);
        }

        public void AddEventListener<T>(Action<T> handler) where T : GameEvent
        {
            SpecificInvoker<T> invoker = new SpecificInvoker<T>
            {
                Handler = handler
            };

            AddInvoker(typeof(T), invoker);
        }
        
        
        public void RemoveEvent<T>(Action handler) where T : GameEvent
        {
            RemoveEvent(typeof(T), handler);
        }

        public void RemoveEventListener<T>(Action<T> handler) where T : GameEvent
        {
            RemoveEvent(typeof(T), handler);
        }
        
        private  void RemoveEvent(Type eventType, object handler)
        {
            if (handlers.TryGetValue(eventType, out List<Invoker> handlerList))
            {
                int index = handlerList.FindIndex(invoker => (invoker != null) && invoker.Target.Equals(handler));
                if (index >= 0)
                {
                    handlerList[index] = null;
                }
                else
                {
                    handlerList = null;
                }
                toRemove.Add(handlerList);
            }
        }

        public bool Dispatch<T>(T gameEvent) where T : GameEvent
        {
            bool dispatched = false;
            
            dispatchingCounter++;
            if (handlers.TryGetValue(typeof(T), out List<Invoker> handlerList))
            {
                foreach (Invoker invoker in handlerList)
                {
                    if (invoker != null)
                    {
                        invoker.Invoke(gameEvent);
                        dispatched = true;
                    }
                }
            }
            dispatchingCounter--;
            RemoveInvokers();
            return dispatched;
        }

        private void RemoveInvokers()
        {
            if (dispatchingCounter == 0)
            {
                foreach (List<Invoker> handlerList in toRemove)
                {
                    handlerList.RemoveAll(invoker => (invoker == null));
                }

                toRemove.Clear();
            }
        }

        private void AddInvoker(Type eventType, Invoker invoker)
        {
            if (!handlers.TryGetValue(eventType, out List<Invoker> handlerList))
            {
                handlerList = new List<Invoker>();
                handlers.Add(eventType, handlerList);
            }
            handlerList.Add(invoker);
        }
    }
}
