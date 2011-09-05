using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableCollectionEx
{
    
    
    /// <summary>
    ///This is a test class for LazyObservableCollectionTest and is intended
    ///to contain all LazyObservableCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ObservableCollectionTests
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
            TestContext.BeginTimer("TestTimer");
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            TestContext.EndTimer("TestTimer");
        }
        
        #endregion

        #region Tests

        [TestMethod()]
        public void ConstructorTest()
        {
            ObservableCollectionExConstructorTestHelper<GenericParameterHelper>();
        }
        
        [TestMethod()]
        public void AddTest()
        {
            IListTAddTestHelper<GenericParameterHelper>();
            IListAddTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void ClearTest()
        {
            ClearTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void ContainsTest()
        {
            IListTContainsTestHelper();
            IListContainsTestHelper();
        }

        [TestMethod()]
        public void CopyToTest()
        {
            IListTCopyToTestHelper<GenericParameterHelper>();
            IListCopyToTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            IListTIndexOfTestHelper<GenericParameterHelper>();
            IListIndexOfTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void InsertTest()
        {
            IListTInsertTestHelper<GenericParameterHelper>();
            IListInsertTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void RemoveTest()
        {
            IListTRemoveTestHelper();
            IListRemoveTestHelper();
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            IListTRemoveAtTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void ItemTest()
        {
            IListTItemTestHelper<GenericParameterHelper>();
            IListItemTestHelper<GenericParameterHelper>();
        }

        [TestMethod()]
        public void MoveTest()
        {
            MoveTestHelper();
        }

        [TestMethod()]
        public void ReEntranceTest()
        {
            ReEntranceTestHelper<GenericParameterHelper>();
        }


        #endregion

        #region Test Helpers

        #region Constructor

        /// <summary>
        ///A test for ObservableCollectionEx Constructor
        ///</summary>
        public void ObservableCollectionExConstructorTestHelper<T>() where T : new()
        {
            ObservableCollectionEx<T> target;
            List<T> list = new List<T>();
            
            list.Add(new T());
            list.Add(new T());
            list.Add(new T());
            list.Add(new T());
            list.Add(new T());
            list.Add(new T());

            target = new ObservableCollectionEx<T>();
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ObservableCollectionEx<T>));

            target = new ObservableCollectionEx<T>(list);
            Assert.IsNotNull(target);
            Assert.AreEqual(6, target.Count, "Incorrect List copy constructor");

            target = new ObservableCollectionEx<T>(list as IEnumerable<T>);
            Assert.IsNotNull(target);
            Assert.AreEqual(6, target.Count, "Incorrect List copy constructor");

            try
            {
                target = new ObservableCollectionEx<T>((List<T>)null);
                Assert.Fail("Null arguments are not allowed");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentNullException));
            }

            try
            {
                target = new ObservableCollectionEx<T>((IEnumerable<T>)null);
                Assert.Fail("Null arguments are not allowed");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentNullException));
            }
        }
        
        #endregion

        #region AddTest

        /// <summary>
        ///A test for IList<T>.Add
        ///</summary>
        public void IListTAddTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();
            
            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            target.Add(new T());
            target.Add(new T());

            Assert.IsTrue(2 == target.Count, "Added count is incorrect");
            Assert.IsTrue(4 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(2 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }

        /// <summary>
        ///A test for IList.Add
        ///</summary>
        public void IListAddTestHelper<T>() where T : new()
        {
            IList target = CreateTargetHelper<T>();

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            // Add to the end of the collection
            Assert.IsTrue(0 == target.Add(new T()), "Incorrect index");
            Assert.IsTrue(1 == target.Add(new T()), "Incorrect index");

            // Verify fired events
            Assert.IsTrue(2 == target.Count, "Added count is incorrect");
            Assert.IsTrue(4 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(2 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            try
            {
                target.Add(new object());
                Assert.Fail("Out of range error not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

        }


        #endregion

        #region ClearTest

        /// <summary>
        ///A test for Clear
        ///</summary>
        public void ClearTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();

            target.Add(new T());
            target.Add(new T());

            // Clear event caches
            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            target.Clear();

            Assert.IsTrue(0 == target.Count, "Count is incorrect");
            Assert.IsTrue(2 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(1 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }

        #endregion

        #region ContainsTest

        /// <summary>
        ///A test for Contains
        ///</summary>
        public void IListTContainsTestHelper()
        {
            IList<GenericParameterHelper> target = CreateTargetHelper<GenericParameterHelper>();

            GenericParameterHelper value = new GenericParameterHelper(1);

            target.Add(new GenericParameterHelper(0));
            target.Add(value);
            target.Add(new GenericParameterHelper(2));

            Assert.IsFalse(target.Contains(new GenericParameterHelper(3)), "Incorrect condition, found wrong item");
            Assert.IsTrue(target.Contains(value), "Incorrect condition, could not find item");
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        public void IListContainsTestHelper()
        {
            IList target = CreateTargetHelper<GenericParameterHelper>();

            GenericParameterHelper value = new GenericParameterHelper(0);

            target.Add(new GenericParameterHelper(1));
            target.Add(value);
            target.Add(new GenericParameterHelper(2));

            Assert.IsFalse(target.Contains(new GenericParameterHelper(3)), "Incorrect condition, found wrong item");
            Assert.IsFalse(target.Contains(new object()), "Incorrect condition, found wrong item");
            Assert.IsTrue(target.Contains(value), "Incorrect condition, could not find item");
        }

        #endregion

        #region CopyToTest

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        public void IListTCopyToTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();

            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            T[] array = new T[6];
            
            int arrayIndex = 1;
            target.CopyTo(array, arrayIndex);

            Assert.IsNull(array[0], "Array index 0 if off");
            Assert.IsNull(array[5], "Array index 6 if off");
            Assert.AreEqual(array[1], item1, "Array index 1 if off");
            Assert.AreEqual(array[2], item2, "Array index 2 if off");
            Assert.AreEqual(array[3], item3, "Array index 3 if off");
            Assert.AreEqual(array[4], item4, "Array index 4 if off");
        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        public void IListCopyToTestHelper<T>() where T : new()
        {
            IList target = CreateTargetHelper<T>();

            // Check for error conditions
            try
            {
                target.CopyTo(null, 0); ;
                Assert.Fail("Array is null not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentNullException));
            }

            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            object[] array = new object[6];

            int arrayIndex = 1;
            target.CopyTo(array, arrayIndex);

            Assert.IsNull(array[0], "Array index 0 if off");
            Assert.IsNull(array[5], "Array index 6 if off");
            Assert.AreEqual(array[1], item1, "Array index 1 if off");
            Assert.AreEqual(array[2], item2, "Array index 2 if off");
            Assert.AreEqual(array[3], item3, "Array index 3 if off");
            Assert.AreEqual(array[4], item4, "Array index 4 if off");
        }

        #endregion

        #region IndexOfTest

        /// <summary>
        ///A test for IndexOf
        ///</summary>
        public void IListTIndexOfTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item0);
            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            Assert.AreEqual(target[0], item0, "Array index 0 if off");
            Assert.AreEqual(target[1], item1, "Array index 1 if off");
            Assert.AreEqual(target[2], item2, "Array index 2 if off");
            Assert.AreEqual(target[3], item3, "Array index 3 if off");
            Assert.AreEqual(target[4], item4, "Array index 4 if off");

            try 
            {	        
                var temp = target[-1];
                Assert.Fail("Out of range error not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentOutOfRangeException));
            }

            try 
            {	        
                var temp = target[5];
                Assert.Fail("Out of range error not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentOutOfRangeException));
            }
        }

        /// <summary>
        ///A test for IndexOf
        ///</summary>
        public void IListIndexOfTestHelper<T>() where T : new()
        {
            IList target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item0);
            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            Assert.AreEqual(target[0], item0, "Array index 0 if off");
            Assert.AreEqual(target[1], item1, "Array index 1 if off");
            Assert.AreEqual(target[2], item2, "Array index 2 if off");
            Assert.AreEqual(target[3], item3, "Array index 3 if off");
            Assert.AreEqual(target[4], item4, "Array index 4 if off");

            try
            {
                var temp = target[-1];
                Assert.Fail("Out of range error not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentOutOfRangeException));
            }

            try
            {
                var temp = target[5];
                Assert.Fail("Out of range error not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentOutOfRangeException));
            }

            try
            {
                target[2] = target;
                Assert.Fail("Type missmatch not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }

        }

        #endregion

        #region InsertTest

        /// <summary>
        ///A test for Insert
        ///</summary>
        public void IListTInsertTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            target.Insert(0, item1);
            target.Insert(0, item0);
            target.Insert(2, item2);

            Assert.IsTrue(3 == target.Count, "Added count is incorrect");
            Assert.IsTrue(6 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(3 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            Assert.AreEqual(target[0], item0, "Array index 0 if off");
            Assert.AreEqual(target[1], item1, "Array index 1 if off");
            Assert.AreEqual(target[2], item2, "Array index 2 if off");
        }

        /// <summary>
        ///A test for Insert
        ///</summary>
        public void IListInsertTestHelper<T>() where T : new()
        {
            IList target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            target.Insert(0, item1);
            target.Insert(0, item0);
            target.Insert(2, item2);

            Assert.IsTrue(3 == target.Count, "Added count is incorrect");
            Assert.IsTrue(6 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(3 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            Assert.AreEqual(target[0], item0, "Array index 0 if off");
            Assert.AreEqual(target[1], item1, "Array index 1 if off");
            Assert.AreEqual(target[2], item2, "Array index 2 if off");

            try
            {
                target.Insert(0, new object());
                Assert.Fail("Wrond type is not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }
        #endregion

        #region RemoveTest

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void IListTRemoveTestHelper()
        {
            IList<GenericParameterHelper> target = CreateTargetHelper<GenericParameterHelper>();

            GenericParameterHelper item0 = new GenericParameterHelper(0);
            GenericParameterHelper item1 = new GenericParameterHelper(1);
            GenericParameterHelper item2 = new GenericParameterHelper(2);
            GenericParameterHelper item3 = new GenericParameterHelper(3);
            GenericParameterHelper item4 = new GenericParameterHelper(4);

            target.Add(item0);
            target.Add(item1);
            target.Add(item2);

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            Assert.IsTrue(target.Remove(item1));
            Assert.IsTrue(target.Remove(item0));
            Assert.IsFalse(target.Remove(item0));
            Assert.IsTrue(target.Remove(item2));

            Assert.IsTrue(0 == target.Count, "Added/Removed count is incorrect");
            Assert.IsTrue(6 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(3 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void IListRemoveTestHelper()
        {
            IList target = CreateTargetHelper<GenericParameterHelper>();

            GenericParameterHelper item0 = new GenericParameterHelper(0);
            GenericParameterHelper item1 = new GenericParameterHelper(1);
            GenericParameterHelper item2 = new GenericParameterHelper(2);
            GenericParameterHelper item3 = new GenericParameterHelper(3);
            GenericParameterHelper item4 = new GenericParameterHelper(4);
            GenericParameterHelper item5 = new GenericParameterHelper(5);

            target.Add(item0);
            target.Add(item1);
            target.Add(item2);

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            target.Remove(item1);
            target.Remove(item0);
            target.Remove(item2);
            target.Remove(item5);
            target.Remove(new object());

            Assert.IsTrue(0 == target.Count, "Count is incorrect");
            Assert.IsTrue(6 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(3 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }


        #endregion

        #region RemoveAtTest

        /// <summary>
        ///A test for RemoveAt
        ///</summary>
        public void IListTRemoveAtTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();

            T item0 = new T();
            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item0);
            target.Add(item1);
            target.Add(item2);

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            target.RemoveAt(2);
            target.RemoveAt(0);
            target.RemoveAt(0);

            Assert.IsTrue(0 == target.Count, "Added/Removed count is incorrect");
            Assert.IsTrue(6 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(3 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }

        #endregion

        #region ItemTest

        /// <summary>
        ///A test for Item
        ///</summary>
        public void IListTItemTestHelper<T>() where T : new()
        {
            IList<T> target = CreateTargetHelper<T>();

            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            Assert.AreEqual(target[0], item1, "Array index 1 if off");
            Assert.AreEqual(target[1], item2, "Array index 2 if off");
            Assert.AreEqual(target[2], item3, "Array index 3 if off");
            Assert.AreEqual(target[3], item4, "Array index 4 if off");

            target[2] = item1;
            Assert.AreEqual(target[2], item1, "Array index 2 if off");
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        public void IListItemTestHelper<T>() where T : new()
        {
            IList target = CreateTargetHelper<T>();

            T item1 = new T();
            T item2 = new T();
            T item3 = new T();
            T item4 = new T();

            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            Assert.AreEqual(target[0], item1, "Array index 1 if off");
            Assert.AreEqual(target[1], item2, "Array index 2 if off");
            Assert.AreEqual(target[2], item3, "Array index 3 if off");
            Assert.AreEqual(target[3], item4, "Array index 4 if off");

            target[2] = item1;
            Assert.AreEqual(target[2], item1, "Array index 2 if off");
        }

        #endregion

        #region MoveTest

        void MoveTestHelper()
        {
            ObservableCollectionEx<GenericParameterHelper> target = CreateTargetHelper<GenericParameterHelper>();

            GenericParameterHelper item0 = new GenericParameterHelper(0);
            GenericParameterHelper item1 = new GenericParameterHelper(1);
            GenericParameterHelper item2 = new GenericParameterHelper(2);
            GenericParameterHelper item3 = new GenericParameterHelper(3);
            GenericParameterHelper item4 = new GenericParameterHelper(4);

            target.Add(item0);
            target.Add(item1);
            target.Add(item2);
            target.Add(item3);
            target.Add(item4);

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();

            Assert.IsTrue(1 == target.IndexOf(item1));
            target.Move(1, 3);
            Assert.IsTrue(3 == target.IndexOf(item1));

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();
            using (var iDisabled = target.DisableNotifications())
            {
                iDisabled.Move(3, 1);
                iDisabled.Move(1, 3);
                iDisabled.Move(3, 1);
                iDisabled.Move(1, 3);
                iDisabled.Move(3, 1);
            }
            Assert.IsTrue(0 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(0 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");

            this._firedCollectionEvents.Clear();
            this._firedPropertyEvents.Clear();
            using (var iDelayed = target.DelayNotifications())
            {
                iDelayed.Move(1, 3);
                try
                {
                    iDelayed.Move(3, 1);
                    Assert.Fail("Only one move allowed");
                }
                catch (Exception e)
                {
                    Assert.IsInstanceOfType(e, typeof(InvalidOperationException));
                }
            }
            Assert.IsTrue(1 == this._firedPropertyEvents.Count, "Incorrect number of PropertyChanged notifications");
            Assert.IsTrue(1 == this._firedCollectionEvents.Count, "Incorrect number of CollectionChanged notifications");
        }

        #endregion

        #region ReEntranceTest

        public void ReEntranceTestHelper<T>() where T : new()
        {
            ObservableCollectionEx<T> target = new ObservableCollectionEx<T>();

            target.Add(new T());
            target.Add(new T());

            (target as INotifyCollectionChanged).CollectionChanged += ReEntranceHandler1TestHelper;
            target.Add(new T());
        }

        public void ReEntranceHandler1TestHelper(object sender, NotifyCollectionChangedEventArgs en)
        {
            IList target = sender as IList;

            (target as INotifyCollectionChanged).CollectionChanged -= ReEntranceHandler1TestHelper;
            (sender as INotifyCollectionChanged).CollectionChanged += ((s, ex) => ReEntranceHandler2TestHelper(s, ex));

            target.RemoveAt(0);
        }

        public void ReEntranceHandler2TestHelper(object sender, NotifyCollectionChangedEventArgs en)
        {
            IList target = sender as IList;

            (sender as INotifyCollectionChanged).CollectionChanged += ((s, ex) => ReEntranceHandler2TestHelper(s, ex));

            try
            {
                target.RemoveAt(0);
                Assert.Fail("Modifying of collection during Change notification is not handled");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(InvalidOperationException));
            }
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
