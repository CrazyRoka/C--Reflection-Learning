using System;
using System.Collections;
using System.Collections.Generic;
using WhatsNewAttributes;

[assembly: SupportsWhatsNew]
namespace VectorClass
{
    [LastModified("19 Jul 2017", "updated for C# 7 and .NET Core 2")]
    [LastModified("6 Jun 2015", "updated for C# 6 and .NET Core")]
    [LastModified("14 Dec 2010", "IEnumerable interface implemented: Vector can be treated as a collection")]
    [LastModified("10 Feb 2010", "IFormattable interface implemented Vector accepts N and VE format specifiers")]
    public class Vector : IEnumerable<double>
    {
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [LastModified("19 Jul 2017", "Reduced the number of code lines")]
        public Vector(Vector vector) : this(vector.X, vector.Y, vector.Z) { }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        [LastModified("17 Jul 2018", "Flexible implementation in class")]
        public IEnumerator<double> GetEnumerator() => new VectorEnumerator(this);

        [LastModified("19 Jul 2018", "Used arrow instead")]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public double this[uint i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new IndexOutOfRangeException("Attempt to retrieve Vector element " + i);
                }
            }
        }

        #region enumerator class
        [LastModified("6 Jun 2015", "Change to implement the generic version IEnumerator<T>")]
        [LastModified("14 Feb 2010", "Class created as part of collection support for Vector")]
        private class VectorEnumerator : IEnumerator<double>
        {
            readonly Vector _vector;
            int _location;
            public VectorEnumerator(Vector vector)
            {
                _vector = vector;
                _location = -1;
            }

            public double Current
            {
                get
                {
                    if(_location < 0 || _location > 2)
                        throw new InvalidOperationException(
                            "The enumerator is either before the first element or " +
                            "after the last element of the Vector");
                    return _vector[(uint)_location];
                }
            }

            object IEnumerator.Current => Current;


            public bool MoveNext()
            {
                _location++;
                return _location <= 2;
            }

            public void Reset()
            {
                _location = -1;
            }

            public void Dispose()
            {
                // nothing to clean
            }
        }
        #endregion
    }


}
