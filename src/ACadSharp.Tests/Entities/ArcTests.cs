﻿using ACadSharp.Entities;
using ACadSharp.Tests.Common;
using CSMath;
using System;
using Xunit;

namespace ACadSharp.Tests.Entities
{
	public class ArcTests : CommonEntityTests<Arc>
	{
		[Fact]
		public void CreateFromBulgeTest()
		{
			XY start = new XY(1, 0);
			XY end = new XY(0, 1);
			// 90 degree bulge
			double bulge = Math.Tan(Math.PI / (2 * 4));

			XY center = Arc.GetCenter(start, end, bulge, out double radius);

#if NETFRAMEWORK
			center = MathHelper.FixZero(center);
#endif

			Assert.Equal(XY.Zero, center);
			Assert.Equal(1, radius, TestVariables.DecimalPrecision);

			Arc arc = Arc.CreateFromBulge(start, end, bulge);

#if NETFRAMEWORK
			arc.Center = MathHelper.FixZero(arc.Center);
#endif

			Assert.Equal(XYZ.Zero, arc.Center);
			Assert.Equal(1, arc.Radius, TestVariables.DecimalPrecision);
			Assert.Equal(0, arc.StartAngle, TestVariables.DecimalPrecision);
			Assert.Equal(Math.PI / 2, arc.EndAngle, TestVariables.DecimalPrecision);
		}

		[Fact]
		public void GetBoundingBoxTest()
		{
			Arc arc = new Arc();
			arc.Radius = 5;
			arc.EndAngle = Math.PI / 2;

			BoundingBox boundingBox = arc.GetBoundingBox();

			Assert.Equal(new XYZ(0, 0, 0), boundingBox.Min);
			Assert.Equal(new XYZ(5, 5, 0), boundingBox.Max);

			arc.Center = new XYZ(200.0, 200.0, 0.0);
			boundingBox = arc.GetBoundingBox();

			Assert.Equal(new XYZ(200, 200, 0), boundingBox.Min);
			Assert.Equal(new XYZ(205, 205, 0), boundingBox.Max);
		}

		[Fact]
		public void GetCenter()
		{
			XY start = new XY(1, 0);
			XY end = new XY(0, 1);
			// 90 degree bulge
			double bulge = Math.Tan(Math.PI / (2 * 4));

			XY center = Arc.GetCenter(start, end, bulge);

#if NETFRAMEWORK
			center = MathHelper.FixZero(center);
#endif

			Assert.Equal(XY.Zero, center);

			Arc arc = Arc.CreateFromBulge(start, end, bulge);

#if NETFRAMEWORK
			arc.Center = MathHelper.FixZero(arc.Center);
#endif

			Assert.Equal(XYZ.Zero, arc.Center);
			Assert.Equal(1, arc.Radius, TestVariables.DecimalPrecision);
			Assert.Equal(0, arc.StartAngle, TestVariables.DecimalPrecision);
			Assert.Equal(Math.PI / 2, arc.EndAngle, TestVariables.DecimalPrecision);
		}

		[Fact]
		public void GetEndVerticesTest()
		{
			XY start = new XY(1, 0);
			XY end = new XY(0, 1);
			// 90 degree bulge
			double bulge = Math.Tan(Math.PI / (2 * 4));

			Arc arc = Arc.CreateFromBulge(start, end, bulge);

			arc.GetEndVertices(out XY s1, out XY e2);

			AssertUtils.AreEqual<XY>(start, s1, "start point");
			AssertUtils.AreEqual<XY>(end, e2, "end point");
		}

		[Fact]
		public void TranslationTest()
		{
			double radius = 5;
			XYZ center = new XYZ(1, 1, 0);
			Arc arc = new Arc
			{
				Radius = radius,
				Center = center,
			};

			XYZ move = new XYZ(5, 5, 0);
			Transform transform = Transform.CreateTranslation(move);
			arc.ApplyTransform(transform);

			AssertUtils.AreEqual(XYZ.AxisZ, arc.Normal);
			AssertUtils.AreEqual(center.Add(move), arc.Center);
			Assert.Equal(radius, arc.Radius);
			Assert.Equal(0, arc.StartAngle);
			Assert.Equal(Math.PI, arc.EndAngle);
		}

		[Fact]
		public void RotationTest()
		{
			double radius = 5;
			XYZ center = new XYZ(1, 1, 0);
			Arc arc = new Arc
			{
				Radius = radius,
				Center = center
			};

			Transform transform = Transform.CreateRotation(XYZ.AxisX, MathHelper.DegToRad(90));
			arc.ApplyTransform(transform);

			AssertUtils.AreEqual(new XYZ(1, 0, 1), arc.Center);
			Assert.Equal(radius, arc.Radius);
			Assert.Equal(Math.PI, arc.StartAngle);
			Assert.Equal(0, arc.EndAngle);
			AssertUtils.AreEqual(XYZ.AxisY, arc.Normal);
		}

		[Fact]
		public void ScalingTest()
		{
			double radius = 5;
			XYZ center = new XYZ(1, 1, 0);
			Arc arc = new Arc
			{
				Radius = radius,
				Center = center
			};

			XYZ scale = new XYZ(2, 2, 1);
			Transform transform = Transform.CreateScaling(scale, center);
			arc.ApplyTransform(transform);

			AssertUtils.AreEqual(XYZ.AxisZ, arc.Normal);
			AssertUtils.AreEqual(center, arc.Center);
			Assert.Equal(10, arc.Radius);
			Assert.Equal(0, arc.StartAngle);
			Assert.Equal(Math.PI, arc.EndAngle);
		}
	}
}