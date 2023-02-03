using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour
{

    // Reference to the sign mesh
    public MeshRenderer signMesh;

    public void ApplyMaterial(Material material)
    {
        signMesh.material = material;        
    }
    
    
}
