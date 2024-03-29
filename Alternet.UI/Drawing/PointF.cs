// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    public struct PointF : IEquatable<PointF>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='Drawing.PointF'/> class with member data left uninitialized.
        /// </summary>
        public static readonly PointF Empty;
        private float x; // Do not rename (binary serialization)
        private float y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointF'/> class with the specified coordinates.
        /// </summary>
        public PointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Drawing.PointF'/> struct from the specified
        /// <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public PointF(Vector2 vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="System.Numerics.Vector2"/> from this <see cref="System.Drawing.PointF"/>.
        /// </summary>
        public Vector2 ToVector2() => new Vector2(x, y);

        /// <summary>
        /// Gets a value indicating whether this <see cref='Drawing.PointF'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0f && y == 0f;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='Drawing.PointF'/>.
        /// </summary>
        public float X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='Drawing.PointF'/>.
        /// </summary>
        public float Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Converts the specified <see cref="Drawing.PointF"/> to a <see cref="System.Numerics.Vector2"/>.
        /// </summary>
        public static explicit operator Vector2(PointF point) => point.ToVector2();

        /// <summary>
        /// Converts the specified <see cref="System.Numerics.Vector2"/> to a <see cref="Drawing.PointF"/>.
        /// </summary>
        public static explicit operator PointF(Vector2 vector) => new PointF(vector);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by a given <see cref='Drawing.Size'/> .
        /// </summary>
        public static PointF operator +(PointF pt, Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by the negative of a given <see cref='Drawing.Size'/> .
        /// </summary>
        public static PointF operator -(PointF pt, Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by a given <see cref='Drawing.SizeF'/> .
        /// </summary>
        public static PointF operator +(PointF pt, SizeF sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by the negative of a given <see cref='Drawing.SizeF'/> .
        /// </summary>
        public static PointF operator -(PointF pt, SizeF sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='Drawing.PointF'/> objects. The result specifies whether the values of the
        /// <see cref='Drawing.PointF.X'/> and <see cref='Drawing.PointF.Y'/> properties of the two
        /// <see cref='Drawing.PointF'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointF left, PointF right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='Drawing.PointF'/> objects. The result specifies whether the values of the
        /// <see cref='Drawing.PointF.X'/> or <see cref='Drawing.PointF.Y'/> properties of the two
        /// <see cref='Drawing.PointF'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointF left, PointF right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by a given <see cref='Drawing.Size'/> .
        /// </summary>
        public static PointF Add(PointF pt, Size sz) => new PointF(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by the negative of a given <see cref='Drawing.Size'/> .
        /// </summary>
        public static PointF Subtract(PointF pt, Size sz) => new PointF(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by a given <see cref='Drawing.SizeF'/> .
        /// </summary>
        public static PointF Add(PointF pt, SizeF sz) => new PointF(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='Drawing.PointF'/> by the negative of a given <see cref='Drawing.SizeF'/> .
        /// </summary>
        public static PointF Subtract(PointF pt, SizeF sz) => new PointF(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PointF && Equals((PointF)obj);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(PointF other) => this == other;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override readonly string ToString() => $"{{X={x}, Y={y}}}";
    }
}
