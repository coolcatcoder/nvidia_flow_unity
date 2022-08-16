using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using NvFlowUint = System.UInt32;
using NvAPIWrapper;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System;

public class raw_flow_api : MonoBehaviour
{
    public unsafe struct NvFlowContext { };
    public unsafe NvFlowContext* my_flow_context;

    public unsafe struct AppGraphCtx { };
    public unsafe AppGraphCtx* my_app_graph;

    public unsafe struct NvFlowGrid { };
    public unsafe NvFlowGrid* my_flow_grid;

    public unsafe struct NvFlowRenderTargetView { };
    public unsafe NvFlowRenderTargetView* my_nv_rtv;

    // end of opaque struct definitions

    [StructLayout(LayoutKind.Sequential)]
    struct NvFlowGridDesc
    {
        public NvFlowFloat3 initialLocation;       //!< Initial location of axis aligned bounding box
        public NvFlowFloat3 halfSize;              //!< Initial half size of axis aligned bounding box

        NvFlowDim virtualDim;               //!< Resolution of virtual address space inside of bounding box
        NvFlowMultiRes densityMultiRes;     //!< Number of density cells per velocity cell

        float residentScale;                //!< Fraction of virtual cells to allocate memory for
        float coarseResidentScaleFactor;    //!< Allows relative increase of resident scale for coarse sparse textures

        bool enableVTR;                     //!< Enable use of volume tiled resources, if supported
        bool lowLatencyMapping;				//!< Faster mapping updates, more mapping overhead but less prediction required
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NvFlowDim
    {
        NvFlowUint x, y, z;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NvFlowFloat3
    {
        public float x, y, z;
    };

    enum NvFlowMultiRes
    {
        eNvFlowMultiRes1x1x1 = 0,
        eNvFlowMultiRes2x2x2 = 1
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NvFlowContextDescD3D11
    {
        //unsafe IntPtr* device;
        //unsafe IntPtr* deviceContext;
        IntPtr device;
        IntPtr deviceContext;

        public NvFlowContextDescD3D11(IntPtr device, IntPtr deviceContext) : this()
        {
            this.device = device;
            this.deviceContext = deviceContext;
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NvFlowRenderTargetViewDescD3D11
    {
        IntPtr rtv;
        RawViewportF viewport;

        public NvFlowRenderTargetViewDescD3D11(IntPtr rtv, RawViewportF viewport) : this()
        {
            this.rtv = rtv;
            this.viewport = viewport;
        }
    };

    [DllImport("NvFlowLibRelease_win64.dll")]
    extern static unsafe NvFlowContext* NvFlowInteropCreateContext(AppGraphCtx* appctx);

    [DllImport("NvFlowLibRelease_win64.dll")]
    extern static unsafe AppGraphCtx* AppGraphCtxCreate(int deviceID);

    [DllImport("NvFlowLibRelease_win64.dll")]
    extern static unsafe void NvFlowGridDescDefaults(NvFlowGridDesc* desc);

    [DllImport("NvFlowLibRelease_win64.dll")]
    extern static unsafe NvFlowGrid* NvFlowCreateGrid(NvFlowContext* context, NvFlowGridDesc* desc);

    [DllImport("NvFlowLibRelease_win64.dll")]
    extern static unsafe NvFlowContext* NvFlowCreateContextD3D11(NvFlowUint version, NvFlowContextDescD3D11* desc);

    [DllImport("NvFlowLibRelease_win64.dll")]
    extern static unsafe NvFlowRenderTargetView* NvFlowCreateRenderTargetViewD3D11(NvFlowContext* context, NvFlowRenderTargetViewDescD3D11* desc);

    //____________________end of dll importing___________________

    unsafe NvFlowGridDesc test_grid_desc;
    NvFlowContextDescD3D11 testing_nv_dx_context_desc;

    Device d3d11device;
    DeviceContext d3d11device_context;
    RenderTargetView render_target_view;
    RawViewportF view_port;

    IntPtr ptr_device;
    IntPtr ptr_context;

    NvFlowUint test_version = 1;

    NvFlowFloat3 test_grid_location;

    public Camera player_camera;
    int res_width;
    int res_height;
    public RenderTexture screen_texture;
    public RectTransform rt_displayer;

    NvFlowRenderTargetViewDescD3D11 nv_dx_rtv_desc;

    // Start is called before the first frame update
    void Start()
    {
        res_width = Screen.width;
        res_height = Screen.height;
        screen_texture.width = res_width;
        screen_texture.height = res_height;
        rt_displayer.sizeDelta = new Vector2(res_width, res_height);
        view_port.X = Screen.mainWindowPosition.x;
        view_port.Y = Screen.mainWindowPosition.y;
        view_port.Width = res_width;
        view_port.Height = res_height;

        var texture = new UnityEngine.Texture2D(128, 128);
        var castedTex = new SharpDX.Direct3D11.Texture2D(texture.GetNativeTexturePtr());
        var deviceChild = castedTex.QueryInterface<SharpDX.Direct3D11.DeviceChild>();
        d3d11device = deviceChild.Device;
        d3d11device_context = d3d11device.ImmediateContext;

        var screen_texture_dx_version = screen_texture.GetNativeTexturePtr();
        render_target_view = new RenderTargetView(screen_texture_dx_version);

        //var device_size = 500; //Marshal.SizeOf(d3d11device);
        //var context_size = 500; //Marshal.SizeOf(d3d11device_context);

        //Debug.Log(device_size);
        //Debug.Log(context_size);

        //ptr_device = Marshal.AllocHGlobal(device_size);
        //Marshal.StructureToPtr(d3d11device, ptr_device, false);

        //ptr_context = Marshal.AllocHGlobal(context_size);
        //Marshal.StructureToPtr(d3d11device_context, ptr_context, false);

        testing_nv_dx_context_desc = new NvFlowContextDescD3D11((IntPtr)d3d11device, (IntPtr)d3d11device_context);
        nv_dx_rtv_desc = new NvFlowRenderTargetViewDescD3D11((IntPtr)render_target_view, view_port);

        NVIDIA.Initialize();
        Debug.Log(NVIDIA.ChipsetInfo.DeviceId);
        unsafe
        {
            //NvFlowGridDescDefaults(test_grid_desc);
            //my_flow_grid = NvFlowCreateGrid(my_flow_context, test_grid_desc);
            fixed (NvFlowContextDescD3D11* pointer_of_dx_desc = &testing_nv_dx_context_desc) { my_flow_context = NvFlowCreateContextD3D11(test_version, pointer_of_dx_desc); }
            //fixed (NvFlowRenderTargetViewDescD3D11* pointer_of_rtv_desc = &nv_dx_rtv_desc) { my_nv_rtv = NvFlowCreateRenderTargetViewD3D11(my_flow_context, pointer_of_rtv_desc); }
            fixed (NvFlowGridDesc* pointer_of_grid_desc = &test_grid_desc) { NvFlowGridDescDefaults(pointer_of_grid_desc); }
            Debug.Log(test_grid_desc.halfSize.x.ToString() + test_grid_desc.halfSize.y.ToString() + test_grid_desc.halfSize.z.ToString());

            test_grid_location.x = transform.position.x;
            test_grid_location.y = transform.position.y;
            test_grid_location.z = transform.position.z;

            //fixed (NvFlowGridDesc* pointer_of_grid_desc = &test_grid_desc) { my_flow_grid = NvFlowCreateGrid(my_flow_context, pointer_of_grid_desc); }

            Debug.Log("unsafe stuff done");
        }

        //      try
        //      {
        //          unsafe
        //          {
        //              my_app_graph = AppGraphCtxCreate(NVIDIA.ChipsetInfo.DeviceId);
        //              my_flow_context = NvFlowInteropCreateContext(my_app_graph);
        //          };
        //}
        //catch(System.Exception e)
        //      {
        //	Debug.Log("Exception: " + e.Message);
        //      }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        d3d11device_context.Dispose();
        Marshal.FreeHGlobal(ptr_device);
        Marshal.FreeHGlobal(ptr_context);
    }
}
