using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionViewTest
{
    public class Person
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="Lastname">
        /// The lastname.
        /// </param>
        public Person(string firstName, string Lastname)
        {
            this.FirstName = firstName;
            this.LastName = Lastname;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets Lastname.
        /// </summary>
        public string LastName { get; set; }

        #endregion
    }
}
