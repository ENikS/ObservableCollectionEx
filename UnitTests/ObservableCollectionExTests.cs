using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableCollectionEx
{


    /// <summary>
    ///This is a test class for LazyObservableCollectionTest and is intended
    ///to contain all LazyObservableCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ObservableCollectionExTests
    {
        private Collection<NotifyCollectionChangedEventArgs> _firedCollectionEvents = new Collection<NotifyCollectionChangedEventArgs>();

        private Collection<PropertyChangedEventArgs> _firedPropertyEvents = new Collection<PropertyChangedEventArgs>();

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // TODO: TestContext.BeginTimer("TestTimer");
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // TODO: TestContext.EndTimer("TestTimer");
        }

        #endregion

        #region Tests

        [TestMethod()]
        public void DelayedNotificationTest()
        {
            DelayedNotificationTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void DisabledNotificationTest()
        {
            DisabledNotificationTestHelper<GenericParameterHelper>();
        }


        #endregion

        #region Test Helpers

        #region DelayedNotificationTest

        /// <summary>
        ///A test for GetDelayedNotifier
        ///</summary>
        public void DelayedNotificationTestHelper<T>() where T : new()
        {
            ObservableCollectionEx<T> target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();


            // Testing Add
            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();
            using (ObservableCollectionEx<T> iTarget = target.DelayNotifications())
            {
                iTarget.Add(item0);
                iTarget.Add(item1);
                iTarget.Add(item2);
                iTarget.Add(item3);
                iTarget.Add(item4);

                Assert.IsTrue(5 == target.Count, "Count is incorrect");
                Assert.IsTrue(0 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
                Assert.IsTrue(0 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
            }
            
            Assert.IsTrue(5 == target.Count, "Count is incorrect");
            Assert.IsTrue(2 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(1 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            // Testing Replace
            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();
            using (ObservableCollectionEx<T> iTarget = target.DelayNotifications())
            {
                iTarget[1] = item0;
                iTarget[2] = item1;
                iTarget[3] = item2;
                iTarget[4] = item3;
                iTarget[0] = item4;

                using(ObservableCollectionEx<T> iNested = iTarget.DelayNotifications())
                {
                    iNested.Add(item4);
                    iNested.Add(item4);

                    Assert.IsTrue(0 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
                    Assert.IsTrue(0 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
                }

                Assert.IsTrue(7 == target.Count, "Count is incorrect");
                Assert.IsTrue(2 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
                Assert.IsTrue(1 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
            }

            Assert.IsTrue(7 == target.Count, "Count is incorrect");
            Assert.IsTrue(3 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(2 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            // Testing Remove
            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();
            using (ObservableCollectionEx<T> iTarget = target.DelayNotifications())
            {
                iTarget.Remove(item0);
                iTarget.Remove(item1);
                iTarget.Remove(item2);
                iTarget.Remove(item3);
                iTarget.Remove(item4);

                Assert.IsTrue(2 == target.Count, "Count is incorrect");
                Assert.IsTrue(0 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
                Assert.IsTrue(0 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

                try
                {
                    iTarget.Add(item0);
                    Assert.Fail("Mixed operation is not handled");
                }
                catch (Exception e)
                {
                    Assert.IsInstanceOfType(e, typeof(InvalidOperationException));
                }
            }

            Assert.IsTrue(3 == target.Count, "Count is incorrect");
            Assert.IsTrue(2 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(1 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();
            using (ObservableCollectionEx<T> iTarget = target.DelayNotifications())
            {
                iTarget.Clear();
            }

            Assert.IsTrue(0 == target.Count, "Count is incorrect");
            Assert.IsTrue(2 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(1 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

        }

        #endregion

        #region DisabledNotificationTest

        /// <summary>
        ///A test for DisableNotifications
        ///</summary>
        public void DisabledNotificationTestHelper<T>() where T : new()
        {
            ObservableCollectionEx<T> target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            using (ObservableCollectionEx<T> iTarget = target.DisableNotifications())
            {
                iTarget.Add(item0);
                iTarget.Add(item1);
                iTarget.Add(item2);
                iTarget.Add(item3);
                iTarget.Add(item4);

                Assert.IsTrue(5 == target.Count, "Count is incorrect");
                Assert.IsTrue(0 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
                Assert.IsTrue(0 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
            }

            Assert.IsTrue(5 == target.Count, "Count is incorrect");
            Assert.IsTrue(0 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(0 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }

        #endregion

        #region Other

        private ObservableCollectionEx<T> CreateTargetHelper<T>()
        {
            ObservableCollectionEx<T> target = new ObservableCollectionEx<T>();
            (target as INotifyCollectionChanged).CollectionChanged += ((s, e) => this._firedCollectionEvents.Add(e));
            (target as INotifyPropertyChanged).PropertyChanged += ((s, e) => this._firedPropertyEvents.Add(e));

            return target;
        }

        #endregion

        #endregion
    }
}
