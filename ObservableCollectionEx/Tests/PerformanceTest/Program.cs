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
        static int Consumers = 1;
        static GenericParameterHelper[] data = new GenericParameterHelper[MaxCount+1];
        static Stopwatch timer = new Stopwatch();
        static Collection<GenericParameterHelper> _collection = new Collection<GenericParameterHelper>();
        static ObservableCollection<GenericParameterHelper> _original = new ObservableCollection<GenericParameterHelper>(data);
        static ObservableCollectionEx<GenericParameterHelper> _ex = new ObservableCollectionEx<GenericParameterHelper>(data);

        static void Main(string[] args)
        {
            for (int i = 0; i < MaxCount; i++)
                data[i] = new GenericParameterHelper(i);

            Console.WriteLine("Initializing...");
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
            Console.WriteLine("Testing performance...");

            //timer.Reset();
            //TimeCollection(_collection);
            //Console.WriteLine("Collection:\t\t" + timer.ElapsedTicks.ToString());

            timer.Reset();
            TimeCollection(_original);
            Console.WriteLine("ObservableCollection:\t" + timer.ElapsedTicks.ToString());

            timer.Reset();
            TimeCollection(_ex);
            Console.WriteLine("ObservableCollectionEx:\t" + timer.ElapsedTicks.ToString());

            using (var disabled = _ex.DisableNotifications())
            {
                // Collect now so it is not in the way during test
                timer.Reset();
                TimeCollection(disabled);
            }
            Console.WriteLine("Disabled Notification:\t" + timer.ElapsedTicks.ToString());


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
        static void TimeCollection(IList<GenericParameterHelper> collection)
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
