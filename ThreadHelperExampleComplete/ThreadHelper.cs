using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FuelStar.Helpers
{
    public sealed class ThreadHelper
    {
        private static volatile ThreadHelper instance;
        private static object threadLock = new object();

        private ThreadHelper() { }

        public static ThreadHelper Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (threadLock)
                    {
                        if (null == instance)
                            instance = new ThreadHelper();
                    }
                }

                return instance;
            }
        }

        public Dictionary<string, ITheadTask> ActiveTasks = new Dictionary<string, ITheadTask>();


        /// <summary>
        /// Checks to see if the task has completed.
        /// </summary>
        /// <param name="task">The task to check</param>
        /// <returns>True if the task has completed</returns>
        public async Task<bool> CheckTaskComplete<T>(string taskKey)
        {
            bool canContinue = false;
            ITheadTask checkTask;
            bool taskFound = ActiveTasks.TryGetValue(taskKey, out checkTask);
            if (taskFound)
            {
                if (null != ((TheadTask<T>)checkTask).Task)
                {
                    await ((TheadTask<T>)checkTask).Task;
                    if (!((TheadTask<T>)checkTask).CanTokenSource.IsCancellationRequested)
                        canContinue = true;
                }
            }
            return canContinue;
        }

        public async Task<T> GetTaskValue<T>(string taskKey)
        {
            ITheadTask checkTask;
            ActiveTasks.TryGetValue(taskKey, out checkTask);
            return await ((TheadTask<T>)checkTask).Task;
        }

        public void AddTask<T>(Task<T> taskValue)
        {
            string newKey = Guid.NewGuid().ToString();
            AddTask(newKey, taskValue);
        }

        public void AddTask<T>(string taskKey, Task<T> taskValue)
        {
            if (ActiveTasks.ContainsKey(taskKey))
                ActiveTasks.Remove(taskKey);
            ActiveTasks.Add(taskKey, new TheadTask<T>(taskValue));
        }

        public CancellationToken GetCancellationToken(string taskKey)
        {
            ITheadTask checkTask;
            ActiveTasks.TryGetValue(taskKey, out checkTask);
            return checkTask.CanTokenSource.Token;
        }

        public void CancelTask(string taskKey)
        {
            ITheadTask checkTask;
            ActiveTasks.TryGetValue(taskKey, out checkTask);
            CancellationTokenSource tokenSource = checkTask.CanTokenSource;

            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
        }
    }

    public interface ITheadTask
    {
        CancellationTokenSource CanTokenSource { get; }
    }

    public class TheadTask<T> : ITheadTask
    {
        public Task<T> task;
        public CancellationTokenSource canTokenSource;

        public CancellationTokenSource CanTokenSource
        {
            get { return canTokenSource; }
        }

        public Task<T> Task
        {
            get { return task; }
        }

        public TheadTask(Task<T> taskValue)
        {
            task = taskValue;
            canTokenSource = new CancellationTokenSource();
        }

    }
}
