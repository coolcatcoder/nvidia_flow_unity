using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;
using NvFlowUint = System.UInt32;
using System;

public class clean_flow_api_old : MonoBehaviour
{

    public class flow_grid
    {
        public unsafe struct NvFlowGrid { }

        [StructLayout(LayoutKind.Sequential)]
        public struct NvFlowGridDesc
        {
            public flow_math.NvFlowFloat3 initialLocation;       //!< Initial location of axis aligned bounding box
            public flow_math.NvFlowFloat3 halfSize;              //!< Initial half size of axis aligned bounding box

            flow_math.NvFlowDim virtualDim;               //!< Resolution of virtual address space inside of bounding box
            flow_math.NvFlowMultiRes densityMultiRes;     //!< Number of density cells per velocity cell

            float residentScale;                //!< Fraction of virtual cells to allocate memory for
            float coarseResidentScaleFactor;    //!< Allows relative increase of resident scale for coarse sparse textures

            bool enableVTR;                     //!< Enable use of volume tiled resources, if supported
            bool lowLatencyMapping;             //!< Faster mapping updates, more mapping overhead but less prediction required
        }
    }

    public class flow_context
    {
        public unsafe struct NvFlowContext { }
    }

    public class flow_context_dx
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NvFlowContextDescD3D11
        {
            IntPtr device;
            IntPtr deviceContext;

            public NvFlowContextDescD3D11(IntPtr device, IntPtr deviceContext) : this()
            {
                this.device = device;
                this.deviceContext = deviceContext;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NvFlowRenderTargetViewDescD3D11
        {
            IntPtr rtv;
            RawViewportF viewport;

            public NvFlowRenderTargetViewDescD3D11(IntPtr rtv, RawViewportF viewport) : this()
            {
                this.rtv = rtv;
                this.viewport = viewport;
            }
        }

        [DllImport("NvFlowLibRelease_win64.dll")]
        extern static unsafe flow_context.NvFlowContext* NvFlowCreateContextD3D11(NvFlowUint version, NvFlowContextDescD3D11* desc);
    }

    public class flow_math
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NvFlowDim
        {
            NvFlowUint x, y, z;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NvFlowFloat3
        {
            public float x, y, z;
        }

        public enum NvFlowMultiRes
        {
            eNvFlowMultiRes1x1x1 = 0,
            eNvFlowMultiRes2x2x2 = 1
        }
    }

    public class unity_dx
    {
        public DeviceChild GetDeviceChild(IntPtr TexturePointer)
        {
            var castedTex = new SharpDX.Direct3D11.Texture2D(TexturePointer);
            var deviceChild = castedTex.QueryInterface<SharpDX.Direct3D11.DeviceChild>();
            return deviceChild;
        }
    }
}
