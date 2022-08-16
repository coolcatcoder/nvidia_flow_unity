using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;
using System;

public class FlowGrid : MonoBehaviour
{
    public unsafe struct NvFlowGrid { }

    [StructLayout(LayoutKind.Sequential)]
    public struct NvFlowGridDesc
    {
        public FlowTypes.NvFlowFloat3 initialLocation;       //!< Initial location of axis aligned bounding box
        public FlowTypes.NvFlowFloat3 halfSize;              //!< Initial half size of axis aligned bounding box

        FlowTypes.NvFlowDim virtualDim;               //!< Resolution of virtual address space inside of bounding box
        FlowTypes.NvFlowMultiRes densityMultiRes;     //!< Number of density cells per velocity cell

        float residentScale;                //!< Fraction of virtual cells to allocate memory for
        float coarseResidentScaleFactor;    //!< Allows relative increase of resident scale for coarse sparse textures

        bool enableVTR;                     //!< Enable use of volume tiled resources, if supported
        bool lowLatencyMapping;             //!< Faster mapping updates, more mapping overhead but less prediction required
    }

    [DllImport("NvFlowLibRelease_win64.dll")]
    public extern static unsafe void NvFlowGridDescDefaults(NvFlowGridDesc* desc);
}
