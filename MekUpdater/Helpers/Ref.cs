/// Copyright 2021 Henri Vainio 

// NOT USED CURRENTLY, BUT MIGHT BE USEFUL AT SOME POINT

namespace MekPathLibraryTests.Helpers
{
    /// <summary>
    /// Object that can be passed as ref also in async methods 
    /// </summary>
    public class Ref<T>
    {
        /// <summary>
        /// Make empty object than can be used as ref also on async methods
        /// </summary>
        public Ref() { }
        /// <summary>
        /// Make object than can be used as ref also on async methods
        /// </summary>
        public Ref (T value)
        { 
            Value = value; 
        }

        /// <summary>
        /// Get referenced object value
        /// </summary>
        public T? Value { get; set; }

        public override string ToString() => Value?.ToString() ?? string.Empty;


        // Convert T and Ref<T> back to each 
        public static implicit operator T?(Ref<T> r) => r.Value;
        public static implicit operator Ref<T>(T value) => new(value);
    }
}
