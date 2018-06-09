using System;
using System.Collections.Concurrent;

namespace CacheInterfaceFramework
{
	public class Queue<TElement> : IDisposable
	{
		public Guid ID { get; private set; }
		private BlockingCollection<TElement> collection = new BlockingCollection<TElement>();

		public Queue(Guid queueId)
		{
			ID = queueId;
		}

		public void Send(TElement element)
		{
			collection.Add(element);
		}

		#region Receive

		public TElement Receive()
		{
			TElement element;
			collection.TryTake(out element);
			return element;
		}

		public TElement Receive(int millSeconds)
		{
			TElement element;
			collection.TryTake(out element, millSeconds);
			return element;
		}

		public TElement Receive(TimeSpan millSeconds)
		{
			TElement element;
			collection.TryTake(out element, millSeconds);
			return element;
		}

		#endregion

		public int Count()
		{
			return collection.Count;
		}

		public bool IsAddingCompleted
		{
			get
			{
				return collection.IsAddingCompleted;
			}
		}

		public void CompleteAdding()
		{
			if(collection!=null)
			{
				collection.CompleteAdding();
			}
		}

		#region Dispose

		private bool disposed;
		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					collection.Dispose();
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
