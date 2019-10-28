using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using softaware.ViewPort.Messaging;
using System;

namespace softaware.ViewPort.Tests
{
    [TestClass]
    public class MessengerTests
    {
        private class DelegateReceiver
        {
            private readonly Action<Message<string>> receive;

            public DelegateReceiver(Action<Message<string>> receive)
            {
                this.receive = receive;
            }

            public void Receive(Message<string> message)
            {
                this.receive(message);
            }
        }

        [TestMethod]
        public void TestMessageTypes()
        {
            var receiver = A.Fake<IReceiver<Message<string>>>();

            var messenger = new Messenger();
            messenger.Register(receiver);
            messenger.Send(new Message<int>(17));

            A.CallTo(() => receiver.Receive(null)).WithAnyArguments().MustNotHaveHappened();

            var message = new Message<string>("Test");
            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappened();
        }

        [TestMethod]
        public void TestMultipleMessages()
        {
            var receiver = A.Fake<IReceiver<Message<string>>>();

            var messenger = new Messenger();
            messenger.Register(receiver);

            var message = new Message<string>("Test");
            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappenedOnceExactly();

            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappenedTwiceExactly();
        }

        [TestMethod]
        public void TestMultipleReceivers()
        {
            var receiver1 = A.Fake<IReceiver<Message<string>>>();
            var receiver2 = A.Fake<IReceiver<Message<string>>>();
            var receiver3 = A.Fake<IReceiver<Message<int>>>();

            var messenger = new Messenger();
            messenger.Register(receiver1);
            messenger.Register(receiver2);
            messenger.Register(receiver3);

            var message1 = new Message<string>("Test");
            messenger.Send(message1);

            A.CallTo(() => receiver1.Receive(message1)).MustHaveHappened();
            A.CallTo(() => receiver2.Receive(message1)).MustHaveHappened();

            var message2 = new Message<int>(17);
            messenger.Send(message2);

            A.CallTo(() => receiver3.Receive(message2)).MustHaveHappened();
        }

        [TestMethod]
        public void TestDeregister()
        {
            var receiver = A.Fake<IReceiver<Message<string>>>();

            var messenger = new Messenger();
            messenger.Register(receiver);

            var message = new Message<string>("Test");
            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappenedOnceExactly();

            messenger.Deregister(receiver);
            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public void TestDispose()
        {
            var receiver = A.Fake<IReceiver<Message<string>>>();

            var messenger = new Messenger();
            var registration = messenger.Register(receiver);

            var message = new Message<string>("Test");
            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappenedOnceExactly();

            registration.Dispose();
            messenger.Send(message);
            A.CallTo(() => receiver.Receive(message)).MustHaveHappenedOnceExactly();
        }        

        private class ReceiverReference
        {
            public ReceiverReference()
            {
                this.Receiver = A.Fake<IReceiver<Message<string>>>();
                this.WeakReference = new WeakReference(this.Receiver);
            }

            public IReceiver<Message<string>> Receiver { get; private set; }
            public WeakReference WeakReference { get; }

            public void Forget()
            {
                this.Receiver = null;
            }
        }

        [TestMethod]
        public void TestWeakReference()
        {
            bool wasReceived = false;
            
            var messenger = new Messenger();
            var message = new Message<string>("Test");

            var reference = this.CallInOwnScope(() =>
            {
                var receiver = A.Fake<IReceiver<Message<string>>>();
                A.CallTo(() => receiver.Receive(message)).Invokes(() => wasReceived = true);
                messenger.Register(receiver);

                Assert.IsFalse(wasReceived);
                messenger.Send(message);
                Assert.IsTrue(wasReceived);

                var reference = new WeakReference(receiver);
                Assert.IsTrue(reference.IsAlive);

                return reference;
            });

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.IsFalse(reference.IsAlive);

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsFalse(wasReceived);
        }

        [TestMethod]
        public void TestWeakReferenceDelegateNoReference()
        {
            bool wasReceived = false;

            var message = new Message<string>("Test");

            var messenger = new Messenger();
            
            var reference = this.CallInOwnScope(() =>
            {
                messenger.Register<Message<string>>(m => wasReceived = true);

                Assert.IsFalse(wasReceived);
                messenger.Send(message);
                Assert.IsTrue(wasReceived);

                return true;
            });

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsFalse(wasReceived);
        }

        [TestMethod]
        public void TestWeakReferenceDelegateReference()
        {
            bool wasReceived = false;

            var message = new Message<string>("Test");

            var messenger = new Messenger();
            var registration = messenger.Register<Message<string>>(m => wasReceived = true);

            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsTrue(wasReceived);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsTrue(wasReceived);

            registration.Dispose();

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsFalse(wasReceived);
        }

        [TestMethod]
        public void TestWeakReferenceDelegateReceiverNoReference()
        {
            bool wasReceived = false;
            

            var message = new Message<string>("Test");

            var messenger = new Messenger();

            this.CallInOwnScope(() =>
            {
                var receiver = new DelegateReceiver(m => wasReceived = true);

                messenger.Register<Message<string>>(receiver.Receive);

                Assert.IsFalse(wasReceived);
                messenger.Send(message);
                Assert.IsTrue(wasReceived);

                return true;
            });

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsFalse(wasReceived);
        }

        [TestMethod]
        public void TestWeakReferenceDelegateReceiverReference()
        {
            bool wasReceived = false;
            var receiver = new DelegateReceiver(m => wasReceived = true);

            var message = new Message<string>("Test");

            var messenger = new Messenger();
            var registration = messenger.Register<Message<string>>(receiver.Receive);

            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsTrue(wasReceived);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsTrue(wasReceived);

            registration.Dispose();

            wasReceived = false;
            Assert.IsFalse(wasReceived);
            messenger.Send(message);
            Assert.IsFalse(wasReceived);
        }

        [TestMethod]
        public void DoubleReceiverGetMessagesOnce()
        {
            IReceiver<int> intReceiver = A.Fake<IReceiver<int>>(b => b.Implements(typeof(IReceiver<double>)));
            IReceiver<double> doubleReceiver = (IReceiver<double>)intReceiver;

            var messenger = new Messenger();
            messenger.Register(intReceiver);
            messenger.Register(doubleReceiver);

            messenger.Send(5);

            A.CallTo(() => intReceiver.Receive(5)).MustHaveHappenedOnceExactly();
            A.CallTo(() => doubleReceiver.Receive(5.0)).WithAnyArguments().MustNotHaveHappened();
        }

        /// <summary>
        /// See https://stackoverflow.com/questions/15205891/garbage-collection-should-have-removed-object-but-weakreference-isalive-still-re
        /// </summary>

        private T CallInOwnScope<T>(Func<T> func)
        {
            return func();
        }
    }
}
