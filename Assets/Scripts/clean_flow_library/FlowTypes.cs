using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;
using NvFlowUint = System.UInt32;
using System;

public class FlowTypes : MonoBehaviour
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
