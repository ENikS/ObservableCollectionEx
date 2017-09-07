using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObservableCollectionEx
{
    class Program
    {
        static int MaxCount = 10000;
        static GenericParameterHelper[] data = new GenericParameterHelper[MaxCount+1];
        static Stopwatch timerCollection = new Stopwatch();
        static Stopwatch timerObservableCollection = new Stopwatch();
        static Stopwatch timerObservableCollectionEx = new Stopwatch();
        static Stopwatch timerDelayed = new Stopwatch();
        static Stopwatch timerDisabled = new Stopwatch();
        static Collection<GenericParameterHelper> _collection = new Collection<GenericParameterHelper>();
        static ObservableCollection<GenericParameterHelper> _original = new ObservableCollection<GenericParameterHelper>(data);
        static ObservableCollectionEx<GenericParameterHelper> _ex = new ObservableCollectionEx<GenericParameterHelper>(data);

        static void Main(string[] args)
        {
            int Consumers;
            try
            {
                Consumers = Convert.ToInt32(args[0]);
            
            }
            catch 
            { 
                Consumers = 0;
            }

            for (int i = 0; i < MaxCount; i++)
                data[i] = new GenericParameterHelper(i);

            Console.Write("Initializing...");
            foreach (var item in data)
            { 
                _original.Remove(item);
                _ex.Remove(item);
            }

            (_original as INotifyCollectionChanged).CollectionChanged += CollectionChangedBallast;
            (_ex as INotifyCollectionChanged).CollectionChanged += CollectionChangedBallast;

            for (int i = 1; i < Consumers; i++)
            { 
                (_original as INotifyCollectionChanged).CollectionChanged += delegate { };
                (_ex as INotifyCollectionChanged).CollectionChanged += delegate { };
            }

            //Output header 
            Console.Write("\rTesting performance...");

            TimeCollection(_collection, timerCollection);

            TimeCollection(_original, timerObservableCollection);

            TimeCollection(_ex, timerObservableCollectionEx);

            using (var disabled = _ex.DisableNotifications())
            {
                // Collect now so it is not in the way during test
                TimeCollection(disabled, timerDisabled);
            }

            Console.WriteLine("\rCollection\tObservableCollection\tObservableCollectionEx\tDisabled\t");

            Console.WriteLine(string.Format( "{0}\t\t{1}\t\t\t{2}\t\t\t{3}",  
                                             timerCollection.ElapsedMilliseconds,
                                             timerObservableCollection.ElapsedMilliseconds,
                                             timerObservableCollectionEx.ElapsedMilliseconds,
                                             timerDisabled.ElapsedMilliseconds));

            Debug.WriteLine("Done");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CollectionChangedBallast(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        static void TimeCollection(IList<GenericParameterHelper> collection, Stopwatch timer)
        {
            GC.Collect();
            GC.WaitForFullGCComplete();

            timer.Start();
            
            TimeAdd(collection);

            TimeReplace(collection);

            TimeRemove(collection);
            
            timer.Stop();
        }

        static void TimeAdd(IList<GenericParameterHelper> collection)
        {
            for (int i = 0; i < (MaxCount - 1); i++)
                collection.Add(data[i]);
        }

        static void TimeReplace(IList<GenericParameterHelper> collection)
        {
            for (int i = 0; i < (MaxCount - 1); i++)
                collection[i] = data[i+1];
        }

        static void TimeRemove(IList<GenericParameterHelper> collection)
        {
            for (int i = 1; i < MaxCount; i++)
                collection.Remove(data[i]);
        }
    }
}
