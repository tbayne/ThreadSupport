using NUnit.Framework;


namespace ThreadSupport.Test
{
    [TestFixture]
    public class ThreadSupportTest
    {
        /*
        [Test]
        public void ShouldCreateNewQueueWhenCreatingThread()
        {
            
        }
        */

        [Test]
        public void ShouldEnqueAndDequeTheSameMessage()
        {
            BlockingQueue<ThreadMessage> x = new BlockingQueue<ThreadMessage>();
            // ReSharper disable once UseObjectOrCollectionInitializer
            ThreadMessage msg1 = new ThreadMessage(1);
            msg1.Add("Test1", "Some String Value");
            msg1.Add("TEST2", 1);
            msg1.Add("Test3", true);
            x.Enqueue(msg1);
            ThreadMessage msg2 = x.Dequeue();
            Assert.AreEqual(msg1.Cmd, msg2.Cmd);
            Assert.AreEqual(msg1.GetString("Test1"),msg2.GetString("Test1"));
            Assert.AreEqual(msg1.GetInt("Test2"), msg2.GetInt("TEST2"));
            Assert.AreEqual(msg1.GetBool("Test3"), msg2.GetBool("Test3"));
        }

        [Test]
        // ReSharper disable once InconsistentNaming
        public void ShouldReturnMessagesInFIFOOrder()
        {
            BlockingQueue<ThreadMessage> x = new BlockingQueue<ThreadMessage>();

            ThreadMessage msg1 = new ThreadMessage(1);
            ThreadMessage msg2 = new ThreadMessage(2);
            ThreadMessage msg3 = new ThreadMessage(3);

            x.Enqueue(msg1);
            x.Enqueue(msg2);
            x.Enqueue(msg3);
            Assert.AreEqual(x.Dequeue().Cmd,msg1.Cmd);
            Assert.AreEqual(x.Dequeue().Cmd, msg2.Cmd);
            Assert.AreEqual(x.Dequeue().Cmd, msg3.Cmd);

        }
    }
}


