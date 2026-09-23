

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public class DirtyState
    {
        public bool IsDirty { get; protected set; } = false;
        public void MarkDirty() => IsDirty = true;
        public void Clearn() => IsDirty = false;

        protected void SetValue<T>(ref T field, T value)
        {
            field = value;
            IsDirty = true;
        }
    }
    // public interface IState2
    // {
    //     IEnumerator Execute();
    //     void Enter();
    //     void Exit();
    //     void AddLink(ILink link);
    //     void RemoveLink(ILink link);
    //     bool ValidateLinks(out IState2 nextState);
    //     void EnableLinks();
    //     void DisableLinks();
    // }

    // public interface ILink
    // {
    //     bool Validate(out IState2 nextState);
    //     void Enable() { }
    //     void Disable() { }
    // }


    // public abstract class AbstractState : IState2
    // {
    //     /// <summary>
    //     /// The name of the state used for debugging purposes
    //     /// </summary>
    //     public virtual string Name { get; set; }

    //     // Enable debug messages
    //     protected bool m_Debug = false;
    //     readonly List<ILink> m_Links = new();

    //     public bool DebugEnabled { get => m_Debug; set => m_Debug = value; }

    //     // Called when entering the state
    //     public virtual void Enter()
    //     {
    //         EnableLinks();
    //     }

    //     // Implement in derived classes to define behavior during state execution.
    //     public abstract IEnumerator Execute();

    //     // Called when entering the state
    //     public virtual void Exit()
    //     {
    //     }

    //     public virtual void AddLink(ILink link)
    //     {
    //         if (!m_Links.Contains(link))
    //         {
    //             m_Links.Add(link);
    //         }
    //     }

    //     public virtual void RemoveLink(ILink link)
    //     {
    //         if (m_Links.Contains(link))
    //         {
    //             m_Links.Remove(link);
    //         }
    //     }

    //     public virtual void RemoveAllLinks()
    //     {
    //         m_Links.Clear();
    //     }

    //     // Validates each link and sets the nextState if a valid link is found.
    //     // Returns true if a valid transition is found.
    //     public virtual bool ValidateLinks(out IState2 nextState)
    //     {

    //         if (m_Links != null && m_Links.Count > 0)
    //         {
    //             foreach (var link in m_Links)
    //             {
    //                 var result = link.Validate(out nextState);
    //                 if (result)
    //                 {
    //                     return true;
    //                 }
    //             }
    //         }

    //         // By default, return false without a valid IState
    //         nextState = null;
    //         return false;
    //     }

    //     public void EnableLinks()
    //     {
    //         foreach (var link in m_Links)
    //         {
    //             link.Enable();
    //         }
    //     }

    //     public void DisableLinks()
    //     {
    //         foreach (var link in m_Links)
    //         {
    //             link.Disable();
    //         }
    //     }

    //     public virtual void LogCurrentState()
    //     {
    //         if (m_Debug)
    //         {
    //             string message = "[AbstractState] Current state: " + Name + "(" + this.GetType().Name + ") ----------------------------------------------------------";
    //             message = message.Substring(0, 100);
    //             Debug.Log(message);
    //         }

    //     }
    // }

    // public class State : AbstractState
    // {
    //     readonly Action m_OnExecute;

    //     public State(Action onExecute, string stateName = nameof(State), bool enableDebug = false)
    //     {
    //         m_OnExecute = onExecute;
    //         Name = stateName;

    //         DebugEnabled = enableDebug;
    //     }

    //     public override IEnumerator Execute()
    //     {
    //         yield return null;

    //         if (m_Debug)
    //             base.LogCurrentState();

    //         // Invokes the m_OnExecute Action if it exists
    //         m_OnExecute?.Invoke();
    //     }
    // }

} // namespace