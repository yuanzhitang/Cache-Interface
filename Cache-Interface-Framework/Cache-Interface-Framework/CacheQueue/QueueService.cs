using System;
using System.Collections.Generic;

namespace CacheInterfaceFramework
{
	public class QueueService<TElement> : IQueueService<TElement>
	{
		#region Fields
		private Dictionary<Guid, Queue<TElement>> Queues = new Dictionary<Guid, Queue<TElement>>();
		private static object queueLocker = new object();
		#endregion

		public bool CreateQueue(Guid queueId)
		{
			try
			{
				lock (queueLocker)
				{
					if (!Queues.ContainsKey(queueId))
					{
						Queues.Add(queueId, new Queue<TElement>(queueId));
					}
					return true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to Create Queue: {queueId}");
			}
		}

		public Queue<TElement> GetQueue(Guid queueId)
		{
			Queue<TElement> queue;
			Queues.TryGetValue(queueId, out queue);
			return queue;
		}

		public bool DeleteQueue(Guid queueId)
		{
			try
			{
				lock (queueLocker)
				{
					var queue = GetQueue(queueId);
					if (queue != null)
					{
						Queues.Remove(queueId);
						queue.Dispose();
					}
					return true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to Delete Queue: {queueId}");
			}
		}

		public int Count()
		{
			lock (queueLocker)
			{
				return Queues.Count;
			}
		}

		public QueueStatus Status()
		{
			if (Queues == null)
			{
				return QueueStatus.NonInitialize;
			}
			return QueueStatus.Running;
		}

		#region Dispose

		private bool disposed;
		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					if (Queues != null)
					{
						foreach (var item in Queues)
						{
							item.Value.Dispose();
						}

						Queues.Clear();
					}
					Queues = null;
				}

				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
