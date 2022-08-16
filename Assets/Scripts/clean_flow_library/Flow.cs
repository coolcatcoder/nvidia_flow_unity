using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow : MonoBehaviour
{
    public FlowGrid FlowGrid;
    //public FlowGridMaterial FlowGridMaterial;
    //public FlowShape FlowShape;
    //public FlowGridEmit FlowGridEmit;
    //public FlowGridEmitCustom FlowGridEmitCustom;
    //public FlowGridExport FlowGridExport;
    //public FlowGridImport FlowGridImport;
    //public FlowGridSummary FlowGridSummary;
    //public FlowRenderMaterial FlowRenderMaterial;
    //public FlowVolumeRender FlowVolumeRender;
    //public FlowVolumeShadow FlowVolumeShadow;
    //public FlowCrossSection FlowCrossSection;
    //public FlowGridProxy FlowGridProxy;
    public FlowDevice FlowDevice;
    //public FlowSDFGenerator FlowSDFGenerator;
    //public FlowParticleSurface FlowParticleSurface;
    public FlowContext FlowContext;
    public FlowContextD3D11 FlowContextD3D11;
    //public FlowContextD3D12 FlowContextD3D12;

    // Start is called before the first frame update
    void Start()
    {
        FlowGrid = GetComponent<FlowGrid>();
        //FlowGridMaterial = GetComponent<FlowGridMaterial>();
        //FlowShape = GetComponent<FlowShape>();
        //FlowGridEmit = GetComponent<FlowGridEmit>();
        //FlowGridEmitCustom = GetComponent<FlowGridEmitCustom>();
        //FlowGridExport = GetComponent<FlowGridExport>();
        //FlowGridImport = GetComponent<FlowGridImport>();
        //FlowGridSummary = GetComponent<FlowGridSummary>();
        //FlowRenderMaterial = GetComponent<FlowRenderMaterial>();
        //FlowVolumeRender = GetComponent<FlowVolumeRender>();
        //FlowVolumeShadow = GetComponent<FlowVolumeShadow>();
        //FlowCrossSection = GetComponent<FlowCrossSection>();
        //FlowGridProxy = GetComponent<FlowGridProxy>();
        FlowDevice = GetComponent<FlowDevice>();
        //FlowSDFGenerator = GetComponent<FlowSDFGenerator>();
        //FlowParticleSurface = GetComponent<FlowParticleSurface>();
        FlowContext = GetComponent<FlowContext>();
        FlowContextD3D11 = GetComponent<FlowContextD3D11>();
        //FlowContextD3D12 = GetComponent<FlowContextD3D12>();

    }
}
