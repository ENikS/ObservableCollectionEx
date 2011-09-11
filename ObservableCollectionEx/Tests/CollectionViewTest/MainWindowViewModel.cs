using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace CollectionViewTest
{
    /// <summary>
    /// The main window view model.
    /// </summary>
    public class MainWindowViewModel
    {
        #region Constants and Fields

        /// <summary>
        ///     The _persons.
        /// </summary>
        private readonly ObservableCollectionEx<Person> _persons;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this._persons = new ObservableCollectionEx<Person>();

            for (var index = 0; index < 5; index++)
            {
                this._persons.Add(new Person("FirstName_" + index, "LastName_" + index));
            }
        }

        /// <summary>
        /// Adds the ten persons.
        /// </summary>
        public void AddTenPersons()
        {
            var count = this._persons.Count;

            using (var delayedCollection = this._persons.DelayNotifications())
            {
                for (var index = 0; index < 10; index++)
                {
                    delayedCollection.Add(new Person("FirstName_" + count + index, "LastName_" + count + index));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets Persons.
        /// </summary>
        public IEnumerable<Person> Persons
        {
            get
            {
                return this._persons;
            }
        }

        #endregion
    }
}
