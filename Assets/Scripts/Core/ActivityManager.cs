/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Application {
    public class ActivityManager : MonoBehaviour
    {
        public static ActivityManager shared;
        public Dictionary<string, InteractionContext> contexts;

        // Use this for initialization
        void Awake()
        {
            ActivityManager.shared = this;


        }

        public void Configure() {
            contexts = new Dictionary<string, InteractionContext>();
            var tictactoegame = new TicTacToeGameContext();
            contexts["tictactoe"] = tictactoegame;
        }



        // Update is called once per frame
        void Update()
        {
            if(Time.frameCount % 2 == 0) {
                foreach (KeyValuePair<string, InteractionContext> pair in contexts)
                {
                    pair.Value.Update();
                }

            }

        }


    }

}
