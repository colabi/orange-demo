/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;

namespace Application {
    public class DemoPlatform : MonoBehaviour
    {
        public GameObject platform;
        public Computer computer;
        public AirProjector projector;
        // Use this for initialization
        void Start()
        {
            SystemManager.shared.platform = this;
            SystemManager.shared.RunFlow("twoplayer");
            var game = (TicTacToeGameContext)ActivityManager.shared.contexts["tictactoe"];
            game.AddInput(computer.touchoverlay);
            game.AddInput(projector.touchoverlay);

            game.AddOutput(computer.monitor);
            game.AddOutput(projector.monitor);

            AppleMode = false;
        }

        public bool AppleMode {
            set {
                if(value == true) {
                    computer.gameObject.active = true;
                    projector.gameObject.active = false;
                } else {
                    computer.gameObject.active = false;
                    projector.gameObject.active = true;
                }

            }
            get {
                return computer.enabled;
            }


        }
    }

}
