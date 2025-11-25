using System;

namespace VDEApp
{
    /// <summary>
    /// ExampleClass
    /// </summary>
    public class ExampleClass
    { 
        /// <summary>
        /// Property1
        /// </summary>
        public string Property1 { get; set; }
        /// <summary>
        /// Property1
        /// </summary>
        public int Property2 { get; set; }

        private string _propertyExample;
        /// <summary>
        /// Property
        /// </summary>
        public string PropertyExample
        {
            get { return _propertyExample; }
            set { _propertyExample = value; }
        }

        /// <summary>
        /// Private method
        /// </summary>
        private void privateMethod()
        {

        }
        /// <summary>
        /// Public method
        /// </summary>
        public void PublicMethod()
        {

        }

        /// <summary>
        /// Property
        /// </summary>
        public int Item1 { get; set; }
        public string Item2 { get; set; }
    }
}