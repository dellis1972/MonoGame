// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using MonoGame.Effect.Compiler.Effect.Spirv;
using System;
using System.Linq;

namespace MonoGame.Effect
{
    internal partial class ConstantBufferData
    {
        /// <summary>
        /// Compute the std140 base alignment for a SPIR-V type.
        /// See GLSL ES 3.0 spec §2.12.6.4 "Standard Uniform Block Layout".
        /// </summary>
        static uint Std140BaseAlignment(SpirvTypeBase type)
        {
            if (type is SpirvTypeScalar scalar)
                return scalar.Width / 8; // N (4 for float/int)

            if (type is SpirvTypeVector vector)
            {
                uint N = vector.ElementType.Width / 8;
                // vec2 → 2N, vec3/vec4 → 4N
                return vector.Dimensions == 2 ? 2 * N : 4 * N;
            }

            if (type is SpirvTypeMatrix matrix)
            {
                // Matrices are treated as arrays of column (or row) vectors.
                // Each vector in the array has alignment rounded up to vec4 = 4N.
                uint N = matrix.ColumnType.ElementType.Width / 8;
                return 4 * N; // 16 for float matrices
            }

            if (type is SpirvTypeArray array)
            {
                // Array elements are rounded up to vec4 alignment.
                uint elemAlign = Std140BaseAlignment(array.ElementType);
                uint vec4Align = 4 * 4; // 16
                return Math.Max(elemAlign, vec4Align);
            }

            return 4;
        }

        /// <summary>
        /// Compute the std140 size consumed by a member, including any
        /// internal padding (e.g. mat3 stored as 3×vec4).
        /// </summary>
        static uint Std140SizeForMember(SpirvTypeBase type)
        {
            if (type is SpirvTypeScalar scalar)
                return scalar.Width / 8;

            if (type is SpirvTypeVector vector)
                return vector.Dimensions * (vector.ElementType.Width / 8);

            if (type is SpirvTypeMatrix matrix)
            {
                // Each column/row is padded to vec4 (16 bytes for float).
                uint vec4Size = 4 * (matrix.ColumnType.ElementType.Width / 8); // 16
                return vec4Size * matrix.Columns;
            }

            if (type is SpirvTypeArray array)
            {
                // Each element is rounded up to vec4 alignment.
                uint elemSize = Std140SizeForMember(array.ElementType);
                uint vec4Align = 4 * 4; // 16
                uint stride = ((elemSize + vec4Align - 1) / vec4Align) * vec4Align;
                return stride * array.Length;
            }

            return 4;
        }

        /// <summary>
        /// Build a ConstantBufferData using strict std140 layout rules.
        /// For OpenGL / GLES, the GL driver computes UBO layout from std140
        /// rules independently of any SPIR-V offset decorations, so the
        /// buffer size and parameter offsets must match std140 exactly.
        /// </summary>
        public static ConstantBufferData BuildFromSpirvStructStd140(SpirvTypeStruct svStruct)
        {
            var cbuffer = new ConstantBufferData(svStruct.Name);

            // Process members in their declaration order (by SPIR-V offset).
            var byOffset = svStruct.Members.OrderBy(m => m.Offset.Value);

            uint currentOffset = 0;

            foreach (var member in byOffset)
            {
                uint baseAlign = Std140BaseAlignment(member.Type);

                // Align currentOffset to this member's base alignment.
                currentOffset = ((currentOffset + baseAlign - 1) / baseAlign) * baseAlign;

                var param = new EffectObject.d3dx_parameter();
                param.name = member.Name;
                param.semantic = string.Empty;
                param.bufferOffset = (int)currentOffset;

                (param.rows, param.columns, param.class_) = DimensionsForType(member.Type);
                param.type = ToParamType(member.Type);
                var dataSize = DataSizeForMember(member.Type);

                if (member.Type is SpirvTypeArray array)
                {
                    param.element_count = array.Length;
                    param.member_handles = new EffectObject.d3dx_parameter[param.element_count];

                    for (uint i = 0; i < array.Length; i++)
                    {
                        var mparam = new EffectObject.d3dx_parameter();
                        mparam.name = string.Empty;
                        mparam.semantic = string.Empty;
                        mparam.type = param.type;
                        mparam.class_ = param.class_;
                        mparam.rows = param.rows;
                        mparam.columns = param.columns;
                        mparam.data = new byte[dataSize];
                        param.member_handles[i] = mparam;
                    }
                }
                else
                {
                    param.data = new byte[dataSize];
                }

                cbuffer.Parameters.Add(param);
                cbuffer.ParameterOffset.Add(param.bufferOffset);

                // Advance past this member's std140 footprint.
                currentOffset += Std140SizeForMember(member.Type);
            }

            // Struct size is rounded up to the struct's base alignment (max member alignment, ≥ vec4).
            uint structAlign = 16; // At minimum vec4 alignment
            foreach (var member in svStruct.Members)
            {
                uint a = Std140BaseAlignment(member.Type);
                if (a > structAlign)
                    structAlign = a;
            }
            cbuffer.Size = (int)(((currentOffset + structAlign - 1) / structAlign) * structAlign);

            return cbuffer;
        }
    }
}