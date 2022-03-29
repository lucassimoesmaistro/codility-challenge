using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomStringEnumeratorFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new string[] { "te", null, "tes", "Tes", "Teste", "123456789101112" };
            var config = new EnumeratorConfig
            {
                MinLength = 3,
                MaxLength = 10,
                StartWithCapitalLetter = false
            };

            var enumerator = new CustomStringEnumerator(collection, config);
            foreach (var s in enumerator)
            {
                Console.WriteLine(s);
            }
        }
    }
    public class CustomStringEnumerator : IEnumerable<string>
    {
        private IEnumerable<string> _collection;
        private EnumeratorConfig _config;
        /// <summary> Constructor </summary>
        /// <exception cref="System.ArgumentNullException">If a collection is null</exception>
        /// <exception cref="System.ArgumentNullException">If an config is null</exception>
        public CustomStringEnumerator(IEnumerable<string> collection, EnumeratorConfig config)
        {
            if (!this.IsValid(collection) || config == null)
                throw new ArgumentNullException();

            _collection = collection;
            _config = config;
        }

        public IEnumerator<string> GetEnumerator()
        {
            var enumerator = _collection.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext())
            {
                count++;
                if (enumerator.Current != null)
                {
                    Console.WriteLine($"{count} - {enumerator.Current}");
                    if ((enumerator.Current.Length >= _config.MinLength) &&
                        (enumerator.Current.Length <= _config.MaxLength))
                    {
                        char firstChar = enumerator.Current.ToCharArray()[0];
                        if (Char.IsDigit(firstChar) && _config.StartWithCapitalLetter.Equals(false))
                        {

                            Console.WriteLine($"------->>>>> {count} - {enumerator.Current}");
                            yield return enumerator.Current;
                        }
                        else
                            if (Char.IsUpper(enumerator.Current.ToCharArray()[0]).Equals(_config.StartWithCapitalLetter))
                        {
                            Console.WriteLine($"------->>>>> {count} - {enumerator.Current}");
                            yield return enumerator.Current;
                        }
                    }
                }
                else
                    Console.WriteLine($"{count} - NULL");

            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool IsValid(IEnumerable<string> collection)
        {
            return collection != null && collection.Any();
        }

    }

    //public class CustomStringEnumerator : IEnumerable<string>
    //{
    //    /// <summary> Constructor </summary>
    //    /// <exception cref="System.ArgumentNullException">If a collection is null</exception>
    //    /// <exception cref="System.ArgumentNullException">If an config is null</exception>
    //    public CustomStringEnumerator(IEnumerable<string> collection, EnumeratorConfig config)
    //    {
    //        if (!collection.IsValid(collection) || config == null)
    //            throw new ArgumentNullException();

    //        _collection = collection;
    //    }

    //    public IEnumerator<string> GetEnumerator()
    //    {
    //        return new CustomStringEnumerator(_collection);
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private bool IsValid(IEnumerable<string> collection)
    //    {
    //        return collection != null && collection.Any();
    //    }
    //}

    public class EnumeratorConfig
    {
        // Specifies the minimal length of strings that should be returned by a custom enumerator.
        // If it is set to negative value then this option is ignored.
        public int MinLength { get; set; } = -1;

        // Specifies the maximum length of strings that should be returned by a custom enumerator.
        // If it is set to negative value then this option is ignored.
        public int MaxLength { get; set; } = -1;

        // Specifies that only strings that start with a capital letter should be returned by a custom enumerator.
        // Please note that empty or null strings do not meet this condition.
        public bool StartWithCapitalLetter { get; set; }

        // Specifies that only strings that start with a digit should be returned by a custom enumerator.
        // Please note that empty or null strings do not meet this condition.
        public bool StartWithDigit { get; set; }
    }
}
