/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;

namespace Application {
    public class InputDevice : MonoBehaviour
    {
        public DemoEvent evt;
        public bool hasNewEvent
        {
            get
            {
                return evt != null;
            }
        }
        public void AddEvent(DemoEvent evt) {
            this.evt = evt;
        }
        public DemoEvent GetEvent() {
            var evt = this.evt;
            this.evt = null;
            return evt;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class OutputDevice : MonoBehaviour {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

