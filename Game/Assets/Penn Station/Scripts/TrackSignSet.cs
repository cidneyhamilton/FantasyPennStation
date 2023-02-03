using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSignSet : MonoBehaviour
{
    // Defines the space between each sign
    public float SpaceBetween = 20;
    
    // The  number of signs to spawn for each set of signs
    public float NumberOfSigns = 3;

    public Signpost SignPrefab;

    // The material to apply to the sign
    public Material signMaterial;    

    void Awake() 
    {
        SpawnSigns();        
    }

    // TODO: Invoke this from an editor script to show in scene view
    protected void SpawnSigns()
    {
        for (int i = 0; i < NumberOfSigns; i++)
        {
            // Spawn a new prefab with defined spacing
            Vector3 position = Vector3.zero;
            position.x += i * SpaceBetween;
                        
            Signpost sign = Instantiate(SignPrefab, position, Quaternion.identity) as Signpost;            

            sign.transform.SetParent(transform, false);

            sign.ApplyMaterial(signMaterial);
            
            
        }
        
    }
    
    
    
    
}
