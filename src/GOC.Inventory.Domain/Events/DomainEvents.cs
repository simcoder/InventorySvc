using System;
using System.Collections.Generic;
using SimpleInjector;

namespace GOC.Inventory.Domain.Events
{
   public static class DomainEvents
   { 
      [ThreadStatic] //so that each thread has its own callbacks
       private static List<Delegate> actions;

       public static Container Container { get; set; } //as before
       
       //Registers a callback for the given domain event
       public static void Register<T>(Action<T> callback) where T : IDomainEvent
       {
            if (actions == null)
            {
                actions = new List<Delegate>(); 
            }

            actions.Add(callback);
        }
     
       //Clears callbacks passed to Register on the current thread
       public static void ClearCallbacks()
       {
           actions = null;
       }
            
        //Raises the given domain event async
        public static void RaiseAsync<T>(T args) where T : IDomainEvent
        {
            if (Container != null)
            {
                Container.GetInstance<IHandle<T>>().HandleAsync(args);
            }
                

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    } 
}
