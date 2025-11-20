using System;
using System.Collections.Generic;
using Xunit;

namespace SimplifyCSharp.Tests
{
    public class AdditionalSimplificationTests
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
        public void Simplify2D_WithDuplicateConsecutivePoints_RemovesDuplicates()
        {
            // Use non-collinear points so Douglas-Peucker does not collapse
            var points = new List<Point2D>
            {
                new Point2D(0, 0),
                new Point2D(0, 0),
                new Point2D(0, 1),
                new Point2D(1, 0),
                new Point2D(2, 2)
            };

            var result = SimplificationHelpers.Simplify(points,
                (a, b) => a.X == b.X && a.Y == b.Y,
                p => p.X, p => p.Y,
                tolerance: 0.01);

            Assert.Equal(4, result.Count);
            Assert.Equal(new Point2D(0, 0), result[0]);
            Assert.Equal(new Point2D(0, 1), result[1]);
            Assert.Equal(new Point2D(1, 0), result[2]);
            Assert.Equal(new Point2D(2, 2), result[3]);
        }

        [Fact]
        public void Simplify2D_WithZeroTolerance_NoSimplification()
        {
            var points = new List<Point2D>
            {
                new Point2D(0, 0),
                new Point2D(1, 1),
                new Point2D(2, 2),
                new Point2D(3, 3)
            };

            var result = SimplificationHelpers.Simplify(points,
                (a, b) => a.X == b.X && a.Y == b.Y,
                p => p.X, p => p.Y,
                tolerance: 0.0);

            // With zero tolerance Douglas-Peucker will collapse perfectly collinear
            // middle points, so only endpoints remain.
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Simplify2D_NullInput_ReturnsNull()
        {
            List<Point2D> points = null;
            var result = SimplificationHelpers.Simplify(points,
                (a, b) => a.X == b.X && a.Y == b.Y,
                p => p.X, p => p.Y);

            Assert.Null(result);
        }

        [Fact]
        public void Simplify3D_WithDuplicateConsecutivePoints_RemovesDuplicates()
        {
            // Use non-collinear points so Douglas-Peucker does not collapse
            var points = new List<Point3D>
            {
                new Point3D(0,0,0),
                new Point3D(0,0,0),
                new Point3D(0,1,0),
                new Point3D(1,0,0),
                new Point3D(2,2,2)
            };

            var result = SimplificationHelpers.Simplify(points,
                (a, b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z,
                p => p.X, p => p.Y, p => p.Z,
                tolerance: 0.01);

            Assert.Equal(4, result.Count);
            Assert.Equal(new Point3D(0,0,0), result[0]);
            Assert.Equal(new Point3D(0,1,0), result[1]);
            Assert.Equal(new Point3D(1,0,0), result[2]);
            Assert.Equal(new Point3D(2,2,2), result[3]);
        }

        [Fact]
        public void Simplify2D_HighestQuality_Collinear_ReducesToEndpoints()
        {
            var points = new List<Point2D>
            {
                new Point2D(0, 0),
                new Point2D(1, 1),
                new Point2D(2, 2),
                new Point2D(3, 3)
            };

            var result = SimplificationHelpers.Simplify(points,
                (a, b) => a.X == b.X && a.Y == b.Y,
                p => p.X, p => p.Y,
                tolerance: 0.1,
                highestQuality: true);

            Assert.Equal(2, result.Count);
            Assert.Equal(new Point2D(0, 0), result[0]);
            Assert.Equal(new Point2D(3, 3), result[1]);
        }
    }
}
