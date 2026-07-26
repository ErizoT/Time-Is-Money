using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace KLRB.Utility
{
    public static class TaskExtensions
    {
        public static void RunSafe(this Task task)
        {
            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                    Debug.LogException(t.Exception.Flatten());
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public static void RunSafe(this Task task, Action<Exception> onError)
        {
            task.ContinueWith(t =>
            {
                if (t.Exception != null)
                    onError(t.Exception.Flatten());
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
        
        
        public class Awaiter<T>
        {
            private readonly Task<T> task;

            public Awaiter(Task<T> task) => this.task = task;
            

            public IEnumerator Wait()
            {
                yield return new WaitUntil(() => task.IsCompleted);
            }

            public T Result => task.Result;
        }


        public static Awaiter<T> Yield<T>(this Task<T> task) => new Awaiter<T>(task);
    }
}