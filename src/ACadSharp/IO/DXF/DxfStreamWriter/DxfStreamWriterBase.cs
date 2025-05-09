﻿using CSMath;
using CSUtilities.Converters;
using System;


namespace ACadSharp.IO.DXF
{
	internal abstract class DxfStreamWriterBase : IDxfStreamWriter
	{
		public bool WriteOptional { get; set; } = false;

		public void Write(DxfCode code, object value, DxfClassMap map = null)
		{
			this.Write((int)code, value, map);
		}

		public void Write(DxfCode code, CSMath.IVector value, DxfClassMap map = null)
		{
			this.Write((int)code, value, map);
		}

		public void Write(int code, CSMath.IVector value, DxfClassMap map = null)
		{
			for (int i = 0; i < value.Dimension; i++)
			{
				this.Write(code + i * 10, value[i], map);
			}
		}

		public void WriteTrueColor(int code, Color color, DxfClassMap map = null)
		{
			byte[] arr = new byte[4];
			arr[0] = (byte)color.B;
			arr[1] = (byte)color.G;
			arr[2] = (byte)color.R;
			arr[3] = 0;

			this.Write(code, LittleEndianConverter.Instance.ToInt32(arr), map);
		}

		public void WriteCmColor(int code, Color color, DxfClassMap map = null)
		{
			if (GroupCodeValue.TransformValue(code) == GroupCodeValueType.Int16)
			{
				//BS: Color Index
				this.Write(code, Convert.ToInt16(color.GetApproxIndex()));
			}
			else
			{
				byte[] arr = new byte[4];

				if (color.IsTrueColor)
				{
					arr[0] = (byte)color.B;
					arr[1] = (byte)color.G;
					arr[2] = (byte)color.R;
					arr[3] = 0b1100_0010;   //	0xC2
				}
				else
				{
					arr[3] = 0b1100_0001;
					arr[0] = (byte)color.Index;
				}

				//BL: RGB value
				this.Write(code, LittleEndianConverter.Instance.ToInt32(arr), map);
			}
		}

		public void WriteHandle(int code, IHandledCadObject value, DxfClassMap map = null)
		{
			if (value != null)
			{
				this.Write(code, value.Handle, map);
			}
		}

		public void WriteName(int code, INamedCadObject value, DxfClassMap map = null)
		{
			if (value != null)
			{
				this.Write(code, value.Name, map);
			}
		}

		public void Write(int code, object value, DxfClassMap map = null)
		{
			if (value == null)
			{
				return;
			}

			if (map != null && map.DxfProperties.TryGetValue(code, out DxfProperty prop))
			{
				if (prop.ReferenceType.HasFlag(DxfReferenceType.Optional) && !WriteOptional)
				{
					return;
				}

				if (prop.ReferenceType.HasFlag(DxfReferenceType.IsAngle))
				{
					value = MathHelper.RadToDeg((double)value);
				}
			}

			this.writeDxfCode(code);

			if (value is string s)
			{
				s = s
					.Replace("^", "^ ")
					.Replace("\n", "^J")
					.Replace("\r", "^M")
					.Replace("\t", "^I");
				this.writeValue(code, s);
			}
			else
			{
				this.writeValue(code, value);
			}
		}

		/// <inheritdoc/>
		public abstract void Dispose();

		public abstract void Flush();

		public abstract void Close();

		protected abstract void writeDxfCode(int code);

		protected abstract void writeValue(int code, object value);
	}
}
