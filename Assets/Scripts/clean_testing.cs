using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;
using System;

public class clean_testing : MonoBehaviour
{

    int ResWidth;
    int ResHeight;

    public RenderTexture ScreenTexture;
    public RectTransform RTDisplayer;
    public RawViewportF ViewPort;

    DeviceChild DXDeviceChild;

    public GameObject flow_holder;

    public uint FlowVersion = 1;

    // Start is called before the first frame update
    void Start()
    {
        var Flow = flow_holder.GetComponent<Flow>();

        ResWidth = Screen.width;
        ResHeight = Screen.height;
        ScreenTexture.width = ResWidth;
        ScreenTexture.height = ResHeight;
        RTDisplayer.sizeDelta = new Vector2(ResWidth, ResHeight);
        ViewPort.X = Screen.mainWindowPosition.x;
        ViewPort.Y = Screen.mainWindowPosition.y;
        ViewPort.Width = ResWidth;
        ViewPort.Height = ResHeight;

        DXDeviceChild = Flow.FlowContextD3D11.GetDeviceChild();

        //render texture stuff done

        FlowContextD3D11.NvFlowContextDescD3D11 DXContextDesc = new FlowContextD3D11.NvFlowContextDescD3D11((IntPtr)DXDeviceChild.Device, (IntPtr)DXDeviceChild.Device.ImmediateContext);
        FlowGrid.NvFlowGridDesc TestGridDesc;

        unsafe
        {
            FlowContext.NvFlowContext* MyFlowContext = FlowContextD3D11.NvFlowCreateContextD3D11(FlowVersion, &DXContextDesc);
            FlowGrid.NvFlowGridDescDefaults(&TestGridDesc);

            Debug.Log($"| X HalfSize: {TestGridDesc.halfSize.x} | Y HalfSize: {TestGridDesc.halfSize.y} | Z HalfSize: {TestGridDesc.halfSize.z} |");

            //test_grid_location.x = transform.position.x;
            //test_grid_location.y = transform.position.y;
            //test_grid_location.z = transform.position.z;

            //fixed (NvFlowGridDesc* pointer_of_grid_desc = &test_grid_desc) { my_flow_grid = NvFlowCreateGrid(my_flow_context, pointer_of_grid_desc); }

            Debug.Log("unsafe stuff done");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
