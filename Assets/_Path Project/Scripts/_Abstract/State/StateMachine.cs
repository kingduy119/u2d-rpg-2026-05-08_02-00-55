
using System;
using System.Collections;
using UnityEngine;

namespace TDGame
{
    public class StateMachine
    {
        public IState CurrentState { get; set; }
        // public event Action<IState> stateChanged;

        public void Initialize(IState state)
        {
            CurrentState = state;
            state.Enter();

            // notify other objects that state has changed
            // stateChanged?.Invoke(state);
        }

        public void TransitionTo(IState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            newState.Enter();
        }

        public void Execute()
        {
            CurrentState?.Execute();
        }
    }


    // public class StateMachine2
    // {
    //     public IState2 CurrentState { get; set; }
    //     Coroutine m_CurrentPlayCoroutine;
    //     bool m_PlayLock;

    //     public virtual void SetCurrentState(IState2 state)
    //     {
    //         if (state == null)
    //             throw new ArgumentNullException(nameof(state));

    //         if (CurrentState != null && m_CurrentPlayCoroutine != null)
    //         {
    //             Skip();
    //         }
    //         CurrentState = state;
    //         Coroutines.StartCoroutine(Play());
    //     }

    //     void Skip()
    //     {
    //         if (CurrentState == null)
    //             throw new Exception($"{nameof(CurrentState)} is null!");

    //         if (m_CurrentPlayCoroutine != null)
    //         {
    //             Coroutines.StopCoroutine(ref m_CurrentPlayCoroutine);
    //             CurrentState.Exit();
    //             m_CurrentPlayCoroutine = null;
    //             m_PlayLock = false;
    //         }
    //     }

    //     IEnumerator Play()
    //     {
    //         if (!m_PlayLock)
    //         {
    //             m_PlayLock = true;

    //             CurrentState.Enter();

    //             m_CurrentPlayCoroutine = Coroutines.StartCoroutine(CurrentState.Execute());

    //             yield return m_CurrentPlayCoroutine;

    //             m_CurrentPlayCoroutine = null;
    //         }
    //     }

    //     public virtual void Run(IState2 state)
    //     {
    //         SetCurrentState(state);
    //         Run();
    //     }

    //     Coroutine m_LoopCoroutine;
    //     public virtual void Run()
    //     {
    //         if (m_LoopCoroutine != null) //already running
    //             return;

    //         m_LoopCoroutine = Coroutines.StartCoroutine(Loop());
    //     }

    //     protected virtual IEnumerator Loop()
    //     {
    //         while (true)
    //         {
    //             if (CurrentState != null && m_CurrentPlayCoroutine == null) //current state is done playing
    //             {
    //                 if (CurrentState.ValidateLinks(out var nextState))
    //                 {
    //                     if (m_PlayLock)
    //                     {
    //                         //finalize current state
    //                         CurrentState.Exit();
    //                         m_PlayLock = false;
    //                     }

    //                     CurrentState.DisableLinks();
    //                     SetCurrentState(nextState);
    //                     CurrentState.EnableLinks();
    //                 }
    //             }
    //             yield return null;
    //         }
    //     }

    //     public bool IsRunning => m_LoopCoroutine != null;
    // } // StateMachine2

} // end namespace TDGame