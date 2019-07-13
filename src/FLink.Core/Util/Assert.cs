using System;

namespace FLink.Core.Util
{
    public static class Assert
    {
        public static T NotNull<T>(T reference)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            return reference;
        }

        public static T NotNull<T>(T reference, string errorMessage)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference), errorMessage);

            return reference;
        }

        public static T NotNull<T>(T reference, string errorMessage, object errorMessageArgs)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference), string.Format(errorMessage, errorMessageArgs));

            return reference;
        }

        public static void True(bool condition)
        {
            if (!condition)
                throw new ArgumentException(nameof(condition));
        }

        public static void True(bool condition, string errorMessage)
        {
            if (!condition)
                throw new ArgumentNullException(nameof(condition), errorMessage);
        }

        public static void True(bool condition, string errorMessage, object errorMessageArgs)
        {
            if (!condition)
                throw new ArgumentNullException(nameof(condition), string.Format(errorMessage, errorMessageArgs));
        }

        public static void False(bool condition)
        {
            if (condition)
                throw new ArgumentException(nameof(condition));
        }

        public static void False(bool condition, string errorMessage)
        {
            if (condition)
                throw new ArgumentNullException(nameof(condition), errorMessage);
        }

        public static void False(bool condition, string errorMessage, object errorMessageArgs)
        {
            if (condition)
                throw new ArgumentNullException(nameof(condition), string.Format(errorMessage, errorMessageArgs));
        }

        public static void ElementIndex(int index, int size)
        {
            Nonnegative(size, "Size was negative.");

            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException("Index: " + index + ", Size: " + size);
            }
        }

        public static void ElementIndex(int index, int size, string errorMessage)
        {
            Nonnegative(size, "Size was negative.");

            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException($"{errorMessage} Index: " + index + ", Size: " + size);
            }
        }

        #region [ 正数 ]
        public static int Positive(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            return value;
        }

        public static int Positive(int value, string errorMessage)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), errorMessage);

            return value;
        }

        public static int Positive(int value, string errorMessage, object errorMessageArgs)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(errorMessage, errorMessageArgs));

            return value;
        }

        public static long Positive(long value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            return value;
        }

        public static long Positive(long value, string errorMessage)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), errorMessage);

            return value;
        }

        public static long Positive(long value, string errorMessage, object errorMessageArgs)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(errorMessage, errorMessageArgs));

            return value;
        }
        #endregion

        #region [ 非负 ]
        public static void Nonnegative(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        public static void Nonnegative(int value, string errorMessage)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), errorMessage);
        }

        public static void Nonnegative(int value, string errorMessage, object errorMessageArgs)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(errorMessage, errorMessageArgs));
        }
        #endregion
    }
}
