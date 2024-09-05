using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class InverseMask : Image
{
    public override Material materialForRendering
    {
        get { 
            Material material = new Material(base.materialForRendering);
            material.SetFloat("_StencilComp", (float)CompareFunction.NotEqual);
            return material;
        }
    }

}
