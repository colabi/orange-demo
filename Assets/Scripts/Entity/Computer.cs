/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Application
{
   

    public class Computer : MonoBehaviour
    {
        public CRTMonitor monitor;
        public TouchScreenInput touchoverlay;
        public GameObject screen;
        private void Start()
        {
            screen.GetComponent<Renderer>().material = SystemManager.shared.screenMaterial;
        }

    }

}