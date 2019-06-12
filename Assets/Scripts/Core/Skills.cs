/* Copyright ©Seth Piezas
 */
using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Application
{
    public class SkillFactory : System.Object
    {
        public static Skill AcquireSkill(string skillname, Actor actor) {
            Skill skill = null;
            if(typeof(AIActor) == actor.GetType()) {
                if(skillname == "tictactoe") {
                    skill = new AITicTacToeSkill();
                }
            }
            if(typeof(PersonActor) == actor.GetType()) {
                if(skillname == "tictactoe") {
                    skill = new HumanTicTacToeSkill();
                }
            }
            if(skill != null) {
                skill.owner = actor;
            }
            return skill;
        }
    }



    public class Skill : System.Object
    {
        public Actor owner;
        public string skillid;
        public string category = "";
        public string type = "";

        public virtual void Notify(string msg) {
            owner?.Notify(msg);
        }
    }
    public class GameSkill : Skill { 
    }


  
    public class TicTacToeSkill : GameSkill
    {
        public TicTacToeGameContext context;
        float _waittime = 1.0F;
        float _waittime_min = 0.03F;

        public float waittime
        {
            set
            {
                _waittime = Mathf.Max(value, _waittime_min);
                Debug.Log("WAIT: " + _waittime);
            }
            get
            {
                return _waittime;
            }
        }
        public TicTacToeSkill()
        {
            type = "tictactoe";
        }


        public override void Notify(string msg)
        {
            base.Notify(msg);
            Debug.Log(msg);
        }
    }

    //SHOULD A HUMAN BE ALLOWED TO INTERACT WITH SYSTEM ONLY ON TURN?  MAYBE NOT.  MAYBE CHEATING A FEATURE.
    public class HumanTicTacToeSkill : TicTacToeSkill
    {
        public HumanTicTacToeSkill() : base()
        {

        }

        public override void Notify(string msg)
        {
            base.Notify(msg);
            if (msg.Contains("TURN:"))
            {
                Debug.Log("NOTIFY HUMAN UI");
            }
        }
    }

    public class AITicTacToeSkill : TicTacToeSkill {

        AITicTacToe ttt = new AITicTacToe();
        public int playerid = 1;

        public AITicTacToeSkill() : base() {
 
        }
        void reset() {

        }

        public override void Notify(string msg)
        {
            base.Notify(msg);
            if (msg.Contains("TURN:")) {
                var toks = msg.Split();
                var skillid = toks[1];
                if(skillid == this.skillid) {
                    Debug.Log("AITicTacToeSkill AI TAKING TURN: " + msg);
                    TakeTurn();
                }

            }
            if(msg.Contains("WINNER") || msg.Contains("DRAW")) {
                reset();
            }
            if(msg.Contains("RESET")) {
                reset();
            }

        }

        async void TakeTurn() {
            int gs = context.gs;
            int[,] originalboard = ConvertExternalToInteralBoardState(gs);
            int[,] newboard = originalboard.Clone() as int[,];
            ttt.aiMove(newboard, playerid);
            await Task.Delay(TimeSpan.FromSeconds(waittime));
            ConvertBoardToMove(originalboard, newboard);
           
        }

        /*
         * CONVERT THE NEW BOARD TO A MOUSE CLICK INTERACTION WITH GAME DEVICE OVER INPUT
         */
        bool ConvertBoardToMove(int[,] originalboard, int[,] board)
        {
            int newx = -1;
            int newy = -1;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if(originalboard[x,y] != board[x,y]) {
                        newx = x;
                        newy = y;
                    }
                }
            }
            //ADD EVENT ONLY TO ONE OF THE TOUCH INPUTS
            InputDevice input = context.inputs[0];
            if(input != null) {
                MouseEvent me = new MouseEvent();
                Debug.Log("AI GEN MOUSE newx: " + newx + " y: " + newy);
                float y = newy / 3.0F + 0.1666F;
                float x = newx / 3.0F + 0.1666F;
                me.position = new Vector2(y, x);
                me.mode = 1;
                Debug.Log("AI COMPUTER POS: " + me.position);
                input.AddEvent(me);
                return true;
            }
            return false;
        }

        /*
         * CONVERT THE COMPACT CONTEXT GAME STATE TO THE AI'S UNPACKED REPRESENTATION
         */
        int[,] ConvertExternalToInteralBoardState(int state)
        {
            int[,] board = new int[3, 3];
            for (int i = 0; i < 9; i++) {
                int cell = state >> (i * 2);
                bool occ = (cell & 0x01) == 1;
                if(occ) {
                    int x = i / 3;
                    int y = i % 3;
                    int player = (cell & 0x02) == 0 ? 1 : -1;
                    board[x, y] = player;
                }
            }
            return board;
        }
    }


}
