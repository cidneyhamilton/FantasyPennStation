using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSignSet : MonoBehaviour
{
    // Defines the space between each sign
    public float SpaceBetweenSigns = 20;
    
    // The  number of signs to spawn for each set of signs
    public int NumberOfSignsPerInterval = 3;

    // The number of sets of signs
    public int NumberOfIntervals = 3;
    
    public float SpaceBetweenIntervals = 160;
    
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
        Vector3 position = Vector3.zero;

        for (int j = 0; j < NumberOfIntervals; j++)
        {            
            
            for (int i = 0; i < NumberOfSignsPerInterval; i++)
            {

                // Spawn a new prefab with defined spacing
                Signpost sign = Instantiate(SignPrefab, position, Quaternion.identity) as Signpost;            

                sign.transform.SetParent(transform, false);

                sign.ApplyMaterial(signMaterial);

                

                position.x += SpaceBetweenSigns;
                
                        
            }

            position.x += SpaceBetweenIntervals;            
        }
        
    }
    
    
    
    
}
