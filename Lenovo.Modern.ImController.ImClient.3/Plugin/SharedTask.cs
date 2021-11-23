using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lenovo.Modern.ImController.ImClient.Plugin
{
	// Token: 0x0200000C RID: 12
	public class SharedTask<P, T>
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002FF8 File Offset: 0x000011F8
		public SharedTask(Func<IProgress<P>, CancellationToken, Task<T>> taskFunction)
		{
			this._taskFunction = taskFunction;
			this._task = null;
			this._cancellationTokenSource = new CancellationTokenSource();
			this._lock = new SemaphoreSlim(1, 1);
			this._progress = new SharedTask<P, T>.MulticastProgress<P>();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00003034 File Offset: 0x00001234
		public bool IsRunning
		{
			get
			{
				this._lock.Wait();
				bool result;
				try
				{
					result = this._task != null && !this._task.IsCompleted;
				}
				finally
				{
					this._lock.Release();
				}
				return result;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003088 File Offset: 0x00001288
		public async Task<T> RunAsync(IProgress<P> progress, CancellationToken cancellationToken)
		{
			await this._lock.WaitAsync(cancellationToken);
			try
			{
				if (this._task == null)
				{
					this._cancellationTokenSource = new CancellationTokenSource();
					this._task = this._taskFunction(this._progress, this._cancellationTokenSource.Token);
				}
				this._progress.Add(progress);
			}
			finally
			{
				this._lock.Release();
			}
			Task delayTask = Task.Delay(-1, cancellationToken);
			Task completedTask = await Task.WhenAny(new Task[] { delayTask, this._task });
			await this._lock.WaitAsync();
			T result;
			try
			{
				bool flag = this._progress.Remove(progress);
				Task<T> savedSharedtask = this._task;
				if (flag)
				{
					this._task = null;
				}
				if (completedTask == delayTask)
				{
					if (flag)
					{
						this._cancellationTokenSource.Cancel();
						await savedSharedtask;
					}
					else
					{
						await Task.Delay(-1, cancellationToken);
					}
				}
				result = savedSharedtask.Result;
			}
			finally
			{
				this._lock.Release();
			}
			return result;
		}

		// Token: 0x04000011 RID: 17
		private Func<IProgress<P>, CancellationToken, Task<T>> _taskFunction;

		// Token: 0x04000012 RID: 18
		private Task<T> _task;

		// Token: 0x04000013 RID: 19
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04000014 RID: 20
		private SemaphoreSlim _lock;

		// Token: 0x04000015 RID: 21
		private SharedTask<P, T>.MulticastProgress<P> _progress;

		// Token: 0x0200004C RID: 76
		private class MulticastProgress<P> : IProgress<P>
		{
			// Token: 0x06000163 RID: 355 RVA: 0x0000525A File Offset: 0x0000345A
			public MulticastProgress()
			{
				this._progressList = new List<IProgress<P>>();
				this._lock = new object();
			}

			// Token: 0x06000164 RID: 356 RVA: 0x00005278 File Offset: 0x00003478
			public void Add(IProgress<P> progress)
			{
				object @lock = this._lock;
				lock (@lock)
				{
					this._progressList.Add(progress);
				}
			}

			// Token: 0x06000165 RID: 357 RVA: 0x000052C0 File Offset: 0x000034C0
			public bool Remove(IProgress<P> progress)
			{
				object @lock = this._lock;
				bool result;
				lock (@lock)
				{
					this._progressList.Remove(progress);
					result = this._progressList.Count == 0;
				}
				return result;
			}

			// Token: 0x06000166 RID: 358 RVA: 0x00005318 File Offset: 0x00003518
			public void Report(P value)
			{
				object @lock = this._lock;
				lock (@lock)
				{
					foreach (IProgress<P> progress in this._progressList)
					{
						progress.Report(value);
					}
				}
			}

			// Token: 0x040000E7 RID: 231
			private List<IProgress<P>> _progressList;

			// Token: 0x040000E8 RID: 232
			private object _lock;
		}
	}
}
