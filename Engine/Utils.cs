﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using Rectangle = Basics.Rectangle;

namespace Engine
{
    public static class Utils
    {
        public static Vector2 UnitVector2(float angleRad) => new Vector2((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
        public static Vector2 RandomUnitVector2() => UnitVector2((float)(Basics.Utils.RandomDouble() * Math.PI * 2));
        public static Vector3 To3D(this Vector2 _vector, float _z=0) => new Vector3(_vector.X, _vector.Y, _z);

        public static void Rotate(this List<Vector3> _vertices, float _radians, Vector3 _center)
        {
            for (var i = 0; i < _vertices.Count; i++)
                _vertices[i] = _vertices[i].Rotate(_radians, _center);
        }
        public static Vector3 Rotate(this Vector3 _vertex, float _radians, Vector3 _center)
        {
            var diff = _vertex - _center;
            diff = new Vector3(
                (float)(diff.X * Math.Cos(_radians) - diff.Y * Math.Sin(_radians)),
                (float)(diff.Y * Math.Cos(_radians) + diff.X * Math.Sin(_radians)),
                diff.Z);
            return diff + _center;
        }

        public static bool PointIsRightOfLine(Vector3 point, Vector3 a, Vector3 b) => PointSideOfLine(point, a, b) < 0;
        public static bool PointIsLeftOfLine(Vector3 point, Vector3 a, Vector3 b) => PointSideOfLine(point, a, b) > 0;
        public static bool PointOnLine(Vector3 point, Vector3 a, Vector3 b) => PointSideOfLine(point, a, b) == 0;
        public static int PointSideOfLine(Vector3 point, Vector3 a, Vector3 b) => Math.Sign((b.X - a.X) * (point.Y - a.Y) - (b.Y - a.Y) * (point.X - a.X));
        public static bool PointInTriangle(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
        {
            var bc = PointIsRightOfLine(point, b, c);
            return PointIsRightOfLine(point, a, b) == bc && bc == PointIsRightOfLine(point, c, a);
        }
        
        public static Vector3 PointOnLineAtX(Vector3 a, Vector3 b, float x)
        {
            var diffX = b.X - a.X;
            return diffX == 0 ? (a + b) / 2 : new Vector3(x, (x - a.X) / diffX * (b.Y - a.Y) + a.Y, 0);
        }
        public static Vector3 PointOnLineAtY(Vector3 a, Vector3 b, float y)
        {
            var diffY = b.Y - a.Y;
            return diffY == 0 ? (a + b) / 2 : new Vector3((y - a.Y) / diffY * (b.X - a.X) + a.X, y, 0);
        }
        
        public static Vector3? LinesIntersectionPoint(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, bool asSeg = true)
        {
            if (asSeg && !Rectangle.Collide(
                Basics.Utils.Min(a1.X, a2.X), Basics.Utils.Min(a1.Y, a2.Y), Math.Abs(a1.X - a2.X), Math.Abs(a1.Y - a2.Y),
                Basics.Utils.Min(b1.X, b2.X), Basics.Utils.Min(b1.Y, b2.Y), Math.Abs(b1.X - b2.X), Math.Abs(b1.Y - b2.Y)))
                return null;

            var adx = a1.X - a2.X;
            var ady = a2.Y - a1.Y;
            var bdx = b1.X - b2.X;
            var bdy = b2.Y - b1.Y;
            var c1 = a2.X * a1.Y - a1.X * a2.Y;
            var c2 = b2.X * b1.Y - b1.X * b2.Y;

            var denom = ady * bdx - bdy * adx;
            if (denom == 0)
                return null;
            var ip = new Vector3((adx * c2 - bdx * c1) / denom, (bdy * c1 - ady * c2) / denom, 0);

            if (asSeg &&
                (ip.X - a2.X) * (ip.X - a2.X) + (ip.Y - a2.Y) * (ip.Y - a2.Y) > (a1.X - a2.X) * (a1.X - a2.X) + (a1.Y - a2.Y) * (a1.Y - a2.Y) ||
                (ip.X - a1.X) * (ip.X - a1.X) + (ip.Y - a1.Y) * (ip.Y - a1.Y) > (a1.X - a2.X) * (a1.X - a2.X) + (a1.Y - a2.Y) * (a1.Y - a2.Y) ||
                (ip.X - b2.X) * (ip.X - b2.X) + (ip.Y - b2.Y) * (ip.Y - b2.Y) > (b1.X - b2.X) * (b1.X - b2.X) + (b1.Y - b2.Y) * (b1.Y - b2.Y) ||
                (ip.X - b1.X) * (ip.X - b1.X) + (ip.Y - b1.Y) * (ip.Y - b1.Y) > (b1.X - b2.X) * (b1.X - b2.X) + (b1.Y - b2.Y) * (b1.Y - b2.Y))
                return null;
            return ip;
        }

        public static Vector3 Avg(this IEnumerable<Vector3> list)
        {
            var sum = new Vector3();
            int count = 0;
            foreach (var v in list)
            {
                sum += v;
                count++;
            }
            return sum / count;
        }
        public static Vector3 Sum(this IEnumerable<Vector3> list)
        {
            var sum = new Vector3();
            foreach (var v in list)
                sum += v;
            return sum;
        }
    }
}
