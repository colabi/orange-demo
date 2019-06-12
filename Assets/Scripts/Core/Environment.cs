using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Environment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var envMgr = GetComponent<AREnvironmentProbeManager>();
        envMgr.AddEnvironmentProbe(new Pose(), Vector3.one, Vector3.one);
    }


}
