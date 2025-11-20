using System;
using System.Collections.Generic;
using Xunit;

namespace SimplifyCSharp.Tests
{
    public class SimplificationHelpersTests
    {
        private struct Point2D
        {
            public double X { get; }
            public double Y { get; }

            public Point2D(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        private struct Point3D
        {
            public double X { get; }
            public double Y { get; }
            public double Z { get; }

            public Point3D(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        [Fact]
        public void Simplify2D_WithEmptyList_ReturnsEmptyList()
        {
            var points = new List<Point2D>();
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y, p => p.X, p => p.Y);
            Assert.Empty(result);
        }

        [Fact]
        public void Simplify2D_WithSinglePoint_ReturnsSinglePoint()
        {
            var points = new List<Point2D> { new Point2D(0, 0) };
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y, p => p.X, p => p.Y);
            Assert.Single(result);
            Assert.Equal(new Point2D(0, 0), result[0]);
        }

        [Fact]
        public void Simplify2D_WithTwoPoints_ReturnsTwoPoints()
        {
            var points = new List<Point2D> { new Point2D(0, 0), new Point2D(1, 1) };
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y, p => p.X, p => p.Y);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Simplify2D_WithCollinearPoints_RemovesMiddlePoints()
        {
            var points = new List<Point2D>
            {
                new Point2D(0, 0),
                new Point2D(1, 1),
                new Point2D(2, 2),
                new Point2D(3, 3)
            };
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y, p => p.X, p => p.Y, tolerance: 0.1);
            Assert.Equal(2, result.Count);
            Assert.Equal(new Point2D(0, 0), result[0]);
            Assert.Equal(new Point2D(3, 3), result[1]);
        }

        [Fact]
        public void Simplify3D_WithEmptyList_ReturnsEmptyList()
        {
            var points = new List<Point3D>();
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z, p => p.X, p => p.Y, p => p.Z);
            Assert.Empty(result);
        }

        [Fact]
        public void Simplify3D_WithSinglePoint_ReturnsSinglePoint()
        {
            var points = new List<Point3D> { new Point3D(0, 0, 0) };
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z, p => p.X, p => p.Y, p => p.Z);
            Assert.Single(result);
            Assert.Equal(new Point3D(0, 0, 0), result[0]);
        }

        [Fact]
        public void Simplify3D_WithTwoPoints_ReturnsTwoPoints()
        {
            var points = new List<Point3D> { new Point3D(0, 0, 0), new Point3D(1, 1, 1) };
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z, p => p.X, p => p.Y, p => p.Z);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Simplify3D_WithCollinearPoints_RemovesMiddlePoints()
        {
            var points = new List<Point3D>
            {
                new Point3D(0, 0, 0),
                new Point3D(1, 1, 1),
                new Point3D(2, 2, 2),
                new Point3D(3, 3, 3)
            };
            var result = SimplificationHelpers.Simplify(points, (a, b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z, p => p.X, p => p.Y, p => p.Z, tolerance: 0.1);
            Assert.Equal(2, result.Count);
            Assert.Equal(new Point3D(0, 0, 0), result[0]);
            Assert.Equal(new Point3D(3, 3, 3), result[1]);
        }
    }
}