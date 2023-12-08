﻿using ACadSharp.Blocks;
using ACadSharp.Entities;
using ACadSharp.Tables;

namespace ACadSharp.IO.Templates
{
	internal class CadDimensionTemplate : CadEntityTemplate
	{
		public ulong StyleHandle { get; set; }

		public ulong BlockHandle { get; set; }

		public string BlockName { get; set; }

		public string StyleName { get; set; }

		public CadDimensionTemplate() : base(new DimensionPlaceholder()) { }

		public CadDimensionTemplate(Dimension dimension) : base(dimension) { }

		public override void Build(CadDocumentBuilder builder)
		{
			base.Build(builder);

			Dimension dimension = this.CadObject as Dimension;

			if (this.getTableReference(builder, StyleHandle, StyleName, out DimensionStyle style))
			{
				dimension.Style = style;
			}

			if (this.getTableReference(builder, BlockHandle, BlockName, out BlockRecord block))
			{
				dimension.Block = block;
			}
		}

		public class DimensionPlaceholder : Dimension
		{
			public override ObjectType ObjectType { get { return ObjectType.INVALID; } }

			public DimensionPlaceholder() : base(DimensionType.Linear) { }
		}

		public void SetDimensionObject(Dimension dimensionAligned)
		{
			dimensionAligned.Handle = this.CadObject.Handle;
			dimensionAligned.Owner = this.CadObject.Owner;

			dimensionAligned.XDictionary = this.CadObject.XDictionary;
			//dimensionAligned.Reactors = this.CadObject.Reactors;
			//dimensionAligned.ExtendedData = this.CadObject.ExtendedData;

			dimensionAligned.Color = this.CadObject.Color;
			dimensionAligned.LineWeight = this.CadObject.LineWeight;
			dimensionAligned.LinetypeScale = this.CadObject.LinetypeScale;
			dimensionAligned.IsInvisible = this.CadObject.IsInvisible;
			dimensionAligned.Transparency = this.CadObject.Transparency;

			Dimension dimension = this.CadObject as Dimension;

			dimensionAligned.Version = dimension.Version;
			dimensionAligned.DefinitionPoint = dimension.DefinitionPoint;
			dimensionAligned.TextMiddlePoint = dimension.TextMiddlePoint;
			dimensionAligned.InsertionPoint = dimension.InsertionPoint;
			dimensionAligned.Normal = dimension.Normal;
			dimensionAligned.AttachmentPoint = dimension.AttachmentPoint;
			dimensionAligned.LineSpacingStyle = dimension.LineSpacingStyle;
			dimensionAligned.LineSpacingFactor = dimension.LineSpacingFactor;
			dimensionAligned.Measurement = dimension.Measurement;
			dimensionAligned.Text = dimension.Text;
			dimensionAligned.TextRotation = dimension.TextRotation;
			dimensionAligned.HorizontalDirection = dimension.HorizontalDirection;

			this.CadObject = dimensionAligned;
		}
	}
}
