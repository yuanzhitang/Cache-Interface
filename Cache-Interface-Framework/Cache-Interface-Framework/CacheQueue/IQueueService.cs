using System;

namespace CacheInterfaceFramework
{
	public interface IQueueService<TElement>: IDisposable
	{
		/// <summary>
		/// Create a queue by queueId
		/// </summary>
		/// <param name="queueId"></param>
		bool CreateQueue(Guid queueId);

		/// <summary>
		/// Find a queue by queueId
		/// </summary>
		/// <param name="queueId"></param>
		/// <returns></returns>
		Queue<TElement> GetQueue(Guid queueId);

		/// <summary>
		/// Delete the queue by queueId
		/// </summary>
		/// <param name="queueId"></param>
		bool DeleteQueue(Guid queueId);

		/// <summary>
		/// The count of queues
		/// </summary>
		/// <returns></returns>
		int Count();

		/// <summary>
		/// The status of Queue service
		/// </summary>
		/// <returns></returns>
		QueueStatus Status();
	}

	public enum QueueStatus
	{
		NonInitialize,
		Running
	}
}
