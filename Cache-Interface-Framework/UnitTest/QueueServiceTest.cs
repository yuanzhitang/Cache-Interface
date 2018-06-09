/// <copyright>
/// Copyright Unisys 2018.  All rights reserved.
/// </copyright>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CacheInterfaceFramework;

namespace UnitTest
{
	[TestClass]
	public class QueueServiceTest
	{
		[TestMethod]
		public void CreateQueue()
		{
			var queueService = GetQueueServiceInstance();
			var queueId = Guid.NewGuid();
			Assert.IsTrue(queueService.CreateQueue(queueId));

			var queue = queueService.GetQueue(queueId);
			Assert.IsNotNull(queue);
			Assert.AreEqual(queueId, queue.ID);
		}

		[TestMethod]
		public void GetQueue()
		{
			var queueService = GetQueueServiceInstance();
			var queueId1 = Guid.NewGuid();
			var queueId2 = Guid.NewGuid();
			var queueId3 = Guid.NewGuid();
			Assert.IsTrue(queueService.CreateQueue(queueId1));
			Assert.IsTrue(queueService.CreateQueue(queueId2));
			Assert.IsTrue(queueService.CreateQueue(queueId3));

			var queue = queueService.GetQueue(queueId2);
			Assert.IsNotNull(queue);
			Assert.AreEqual(queueId2, queue.ID);
		}

		[TestMethod]
		public void DeleteQueue()
		{
			var queueService = GetQueueServiceInstance();
			var queueId1 = Guid.NewGuid();
			var queueId2 = Guid.NewGuid();
			var queueId3 = Guid.NewGuid();
			queueService.CreateQueue(queueId1);
			queueService.CreateQueue(queueId2);
			queueService.CreateQueue(queueId3);

			Assert.AreEqual(3, queueService.Count());
			queueService.DeleteQueue(queueId1);
			Assert.AreEqual(2, queueService.Count());
			queueService.DeleteQueue(queueId2);
			Assert.AreEqual(1, queueService.Count());
			queueService.DeleteQueue(queueId3);
			Assert.AreEqual(0, queueService.Count());
		}

		[TestMethod]
		public void QueueServiceDisposeSucceed()
		{
			var queueService = GetQueueServiceInstance();
			var queueId1 = Guid.NewGuid();
			var queueId2 = Guid.NewGuid();
			var queueId3 = Guid.NewGuid();
			queueService.CreateQueue(queueId1);
			queueService.CreateQueue(queueId2);
			queueService.CreateQueue(queueId3);

			Assert.AreEqual(3, queueService.Count());
			Assert.AreEqual(QueueStatus.Running, queueService.Status());
			queueService.Dispose();
			Assert.AreEqual(QueueStatus.NonInitialize, queueService.Status());
		}

		[TestMethod]
		public void SendElementToQueue()
		{
			var queueService = GetQueueServiceInstance();
			var queueId = Guid.NewGuid();
			queueService.CreateQueue(queueId);

			var queue = queueService.GetQueue(queueId);
			Assert.AreEqual(0, queue.Count());
			queue.Send("ABC");
			Assert.AreEqual(1, queue.Count());
		}

		[TestMethod]
		public void ReceiveElementFromQueueWithLimitTime()
		{
			var queueService = GetQueueServiceInstance();
			var queueId = Guid.NewGuid();
			queueService.CreateQueue(queueId);

			var queue = queueService.GetQueue(queueId);
			queue.Send("1");
			queue.Send("2");
			queue.Send("3");
			Assert.AreEqual("1", queue.Receive(500));
			Assert.AreEqual("2", queue.Receive(500));
			Assert.AreEqual("3", queue.Receive(500));
		}

		[TestMethod]
		public void ReceiveElementFromQueueWithLimitTimeSpan()
		{
			var queueService = GetQueueServiceInstance();
			var queueId = Guid.NewGuid();
			queueService.CreateQueue(queueId);

			var queue = queueService.GetQueue(queueId);
			queue.Send("1");
			queue.Send("2");
			queue.Send("3");

			TimeSpan ts = new TimeSpan(0, 0, 0, 0, 500);
			Assert.AreEqual("1", queue.Receive(ts));
			Assert.AreEqual("2", queue.Receive(ts));
			Assert.AreEqual("3", queue.Receive(ts));
		}

		[TestMethod]
		public void ReceiveElementInOrderFromQueue()
		{
			var queueService = GetQueueServiceInstance();
			var queueId = Guid.NewGuid();
			queueService.CreateQueue(queueId);

			var queue = queueService.GetQueue(queueId);
			queue.Send("1");
			queue.Send("2");
			queue.Send("3");
			Assert.AreEqual("1", queue.Receive());
			Assert.AreEqual("2", queue.Receive());
			Assert.AreEqual("3", queue.Receive());
		}

		private IQueueService<string> GetQueueServiceInstance()
		{
			return new QueueService<string>();
		}
	}
}
