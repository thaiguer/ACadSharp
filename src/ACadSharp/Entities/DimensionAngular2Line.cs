﻿using ACadSharp.Attributes;
using CSMath;

namespace ACadSharp.Entities
{
	/// <summary>
	/// Represents a <see cref="DimensionAngular2Line"/> entity.
	/// </summary>
	/// <remarks>
	/// Object name <see cref="DxfFileToken.EntityDimension"/> <br/>
	/// Dxf class name <see cref="DxfSubclassMarker.Angular2LineDimension"/>
	/// </remarks>
	[DxfName(DxfFileToken.EntityDimension)]
	[DxfSubClass(DxfSubclassMarker.Angular2LineDimension)]
	public class DimensionAngular2Line : Dimension
	{
		/// <inheritdoc/>
		public override ObjectType ObjectType => ObjectType.DIMENSION_ANG_2_Ln;

		/// <inheritdoc/>
		public override string ObjectName => DxfFileToken.EntityDimension;

		/// <inheritdoc/>
		public override string SubclassMarker => DxfSubclassMarker.Angular2LineDimension;

		/// <summary>
		/// Definition point for linear and angular dimensions (in WCS).
		/// </summary>
		[DxfCodeValue(13, 23, 33)]
		public XYZ FirstPoint { get; set; }

		/// <summary>
		/// Definition point for linear and angular dimensions (in WCS).
		/// </summary>
		[DxfCodeValue(14, 24, 34)]
		public XYZ SecondPoint { get; set; }

		/// <summary>
		/// Definition point for diameter, radius, and angular dimensions (in WCS).
		/// </summary>
		[DxfCodeValue(15, 25, 35)]
		public XYZ AngleVertex { get; set; }

		/// <summary>
		/// Point defining dimension arc for angular dimensions (in OCS).
		/// </summary>
		[DxfCodeValue(16, 26, 36)]
		public XYZ DimensionArc { get; set; }

		/// <inheritdoc/>
		public override double Measurement
		{
			get
			{
				XY v1 = (XY)(this.SecondPoint - this.FirstPoint);
				XY v2 = (XY)(this.DefinitionPoint - this.AngleVertex);

				return v1.AngleBetweenVectors(v2);
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DimensionAngular2Line() : base(DimensionType.Angular)
		{
		}

		/// <inheritdoc/>
		public override BoundingBox GetBoundingBox()
		{
			return new BoundingBox(this.FirstPoint, this.SecondPoint);
		}

		/// <inheritdoc/>
		public override void ApplyTransform(Transform transform)
		{
			base.ApplyTransform(transform);

			this.FirstPoint = transform.ApplyTransform(this.FirstPoint);
			this.SecondPoint = transform.ApplyTransform(this.SecondPoint);
			this.AngleVertex = transform.ApplyTransform(this.AngleVertex);
			this.DimensionArc = transform.ApplyTransform(this.DimensionArc);
		}
	}
}
