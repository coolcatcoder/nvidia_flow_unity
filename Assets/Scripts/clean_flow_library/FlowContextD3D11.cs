using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;
using System;
using NvFlowUint = System.UInt32;

public class FlowContextD3D11 : MonoBehaviour
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
    extern static unsafe public FlowContext.NvFlowContext* NvFlowCreateContextD3D11(NvFlowUint version, NvFlowContextDescD3D11* desc);

    public DeviceChild GetDeviceChild()
    {
        var texture = new UnityEngine.Texture2D(128, 128);
        var castedTex = new SharpDX.Direct3D11.Texture2D(texture.GetNativeTexturePtr());
        var deviceChild = castedTex.QueryInterface<SharpDX.Direct3D11.DeviceChild>();
        return deviceChild;
    }
}
