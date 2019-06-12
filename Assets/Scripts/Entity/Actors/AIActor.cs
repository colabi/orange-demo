/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;

namespace Application {

    public class AIActor : Actor
    {
        public static AIActor shared;
        public Skill tic1;
        public Skill tic2;

        void Start()
        {
            AIActor.shared = this;
            tic1 = SkillFactory.AcquireSkill("tictactoe", this);
            tic1.skillid = "aitic1";
            var ttt = (AITicTacToeSkill)tic1;
            ttt.playerid = 1;
            tic2 = SkillFactory.AcquireSkill("tictactoe", this);
            tic2.skillid = "aitic2";
            ttt = (AITicTacToeSkill)tic2;
            ttt.playerid = -1;

        }

    }

   
}
