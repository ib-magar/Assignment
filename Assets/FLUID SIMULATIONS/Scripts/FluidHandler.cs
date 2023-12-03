using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public enum FluidColor
{
    Blue,Green,Black,Orange,Purple
}
public class FluidHandler : MonoBehaviour
{

    public MeshRenderer FluidRenderer;
    public Material[] fluidMaterials;
    public void ChangeFluidColor(FluidColor c)
    {
        switch (c)
        {
            case FluidColor.Blue:
                FluidRenderer.material = fluidMaterials[0]; break;
            case FluidColor.Green:
                FluidRenderer.material = FluidRenderer.material = fluidMaterials[1]; break;
            case FluidColor.Black:
                FluidRenderer.material = fluidMaterials[2]; break;
            case FluidColor.Orange:
                FluidRenderer.material = fluidMaterials[3]; break;
            case FluidColor.Purple:
                FluidRenderer.material = fluidMaterials[4]; break;
            default: break;

        }

    }

}
