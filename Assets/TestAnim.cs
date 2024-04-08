using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SkeletonAnimation>().AnimationState.Event += (entry, e) =>
        {
            
            Debug.Log(e.Balance);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
