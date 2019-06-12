/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{

 
    public class DemoEvent : System.Object {
        string type;
    }

    public class MouseEvent : DemoEvent {
        public Vector2 position;
        public int mode;
    }

    public class DialogueEvent : DemoEvent {
       public string dialogue;
    }

    public class CommandEvent : DemoEvent {
        public string command;
    }


    public class InteractionContext
    {
        public List<Skill> players;
        public Texture texture;
        public InteractionContext() : base()
        {
           
            players = new List<Skill>();
        }
        public void RemoveAllPlayers() {
            players.RemoveAll(obj => true);
        }

        public virtual void reset() {
        }
        public virtual bool joinContext(Skill skill)
        {
            players.Add(skill);
            return true;
                
        }
        public void leaveContext(Skill player)
        {

        }

        public void BroadcastToSkills(string msg)
        {
            foreach (Skill skill in players)
            {
                skill.Notify(msg);
            }
        }

        //public virtual void HandleEvent(DemoEvent evt) {}
        public virtual void Update() {}
    }

    public class TurnBasedInteractionContext : InteractionContext
    {
        public string skillname = "";
        public int currentTurn = 0;
        public Actor currentActor;
        public Skill currentSkill;
        public int playerIdx = 0;
        public int playmode = 0;
        public TurnBasedInteractionContext() : base()
        {
            reset();
        }

        public override void Update()
        {
            base.Update();
            if(playmode == 1 && currentTurn < 9) {
                playerIdx = currentTurn % players.Count;
                var skill = players[playerIdx];

                if (skill != currentSkill)
                {
                    currentSkill = skill;
                    currentActor = currentSkill.owner;
                    Debug.Log("TEST AI CURRENT TURN: " + currentTurn);
                    InformTurn(currentSkill);
                }

            }

        }


        void InformTurn(Skill skill)
        {
            BroadcastToSkills("TURN: " + skill.skillid);
        }

        public override bool joinContext(Skill skill)
        {
            Debug.Log("JOINING: " + skill.skillid);
            var v = base.joinContext(skill);
            return v;

        }


        public override void reset()
        {
            base.reset();
            playmode = 0;
            currentTurn = 0;
            currentSkill = null;
            currentActor = null;
            BroadcastToSkills("RESET");
        }
    }


    public class TicTacToeGameContext : TurnBasedInteractionContext
    {
        public int gs = 0;
        int screen = 0;
        public int firstscreen = 0;
        public float acceleration = 1.0F;
        float _delay = 1.0F;
        public float delay {
            set {
                _delay = Mathf.Max(0.1F, value);
            }
            get {
                return _delay;
            }
        }

        public Texture source;
        public RenderTexture dest;

        Material screenMaterial;
        public List<InputDevice> inputs = new List<InputDevice>();
        List<OutputDevice> outputs = new List<OutputDevice>();

        public TicTacToeGameContext(): base() {
            skillname = "tictactoe";
            source = new Texture2D(512, 512);
            dest = Resources.Load<RenderTexture>("ScreenTex");
            screenMaterial = SystemManager.shared.screenMaterial;
            SystemManager.shared.debugTexture = dest;
            reset();
        }





        public override bool joinContext(Skill skill)
        {
            var v = base.joinContext(skill);
            var tttsk = (TicTacToeSkill)skill;
            tttsk.context = this;
            return v;
        }


        public bool AddInput(InputDevice device) {
            inputs.Add(device);
            return true;
        }

        public bool AddOutput(OutputDevice device) {
            outputs.Add(device);
            Renderer r = device.GetComponent<Renderer>();
            r.material = screenMaterial;
            return true;
        }

        void HandleIntroductionEvents(MouseEvent evt) {
            if (evt.mode == 1)
            {
                SetScreen(1);
            }
        }

        void HandleSelectionEvents(MouseEvent evt) {
            int l = GetLocation(evt.position);
            int v = 0;
            if (l > 2) { v = 1; }
            if (l > 5) { v = 2; }
            screenMaterial.SetInt("_Selector", v);
            if (evt.mode == 1)
            {
                //BROADCAST CHANGE IN PARTICIPANTS
                SystemManager.shared.SetPlayerNumber(v);
                SetScreen(2);
            }

        }

        int GetLocation(Vector2 pos) {
            int x = (int)(pos.x * 3.0F);
            int y = (int)(pos.y * 3.0F);
            int loc = y * 3 + x;
            return loc;
        }

        int GetState(int initialstate, int location) {
            int filled = (initialstate >> (location * 2)) & 0x1;
            return filled == 0 ? 0 : -1;
        }

        int GetOccupancy(int initialstate, int location) {
            int filled = (initialstate >> (location * 2)) & 0x1;
            if(filled == 0) {
                return -1;
            } else {
                int occupant = (initialstate >> (location * 2 + 1)) & 0x1;
                return occupant;
            }
        }

        int GenerateNewState(int initialstate, int location) {
            int box = 1 + (currentTurn % 2 == 0 ? 0 : 2);
            int entry = box << (location * 2);
            int newstate = initialstate | entry;
            return newstate;
        }

        void HandleGameEvents(MouseEvent evt) {
            //Debug.Log("HANDLING: " + evt.position);
            int ngs = -1;
            int l = GetLocation(evt.position);
            int s = GetState(gs, l);
            if(s > -1) {

                ngs = GenerateNewState(gs, l);
                screenMaterial.SetInt("_GS", ngs);
            } 

            if (evt.mode == 1)
            {
                if(ngs > -1) {
                    AudioManager.shared.PlayEffect(playerIdx == 0 ? "550" : "350", 0, 0.3F);
                    gs = ngs;
                    currentTurn += 1;
                }
            }

        }

        async void SetScreen(int mode) {
            screen = mode;
            screenMaterial?.SetFloat("_Screen", screen);
            if(mode == 2) {
                playmode = 1;
            }
            if(mode == 3) {
                await Task.Delay(TimeSpan.FromSeconds(delay * 3.0));
                reset();
            }

        }




        public async void HandleEvent(MouseEvent evt)
        {

            if (screen == 0)
            {
                HandleIntroductionEvents(evt);
            }
            else if (screen == 1)
            {
                HandleSelectionEvents(evt);
            }
            else if (screen == 2)
            {
                HandleGameEvents(evt);
            }
            else if (screen == 3) {
                if(evt.mode == 1) {
                    reset();
                }
            }

        }


        int[] winMasks = { 21, 1344, 86016, 4161, 16644, 66576, 65793, 4368 };

        bool hasPlayerWon(int player, int state)
        {

            for (int i = 0; i < 8; i++)
            {
                int currentMask = winMasks[i];
                bool isOccupied = (state & currentMask) == currentMask;
                //Debug.Log("WON CHECK: " + currentMask + " occ: " + isOccupied);
                if (isOccupied)
                {
                    int testbits = (currentMask);
                    
                    int valshift = state >> 1;
                    if (player == 1)
                    {
                        valshift = ~valshift;
                    }
                    bool c = ((valshift & currentMask) == currentMask);
                    Debug.Log("WON?: " + c + " player: " + player + " testbits: " + testbits + " valshift: " + valshift + " mask: " + currentMask + " state: " + state);
                    if (c) return true;
                }
            }
            return false;
        }

        bool isGameWon() {
            int currentState = gs;
            int val = playerIdx == 0 ? 1 : 0;
            bool hasWon = hasPlayerWon(val, currentState);
            return hasWon;
        }

        bool isGameCompleted()
        {
            return currentTurn == 9;
        }

        public override void Update()
        {
            base.Update();
            MouseEvent evt;
            foreach (InputDevice input in inputs) {
                if(input.hasNewEvent) {
                    evt = (MouseEvent)input.GetEvent();
                    HandleEvent(evt);
                    if(evt.mode == 1) {
                        CheckWinner();
                    }
                }

            }



        }

        public void Accelerate() {
            delay *= acceleration;
            foreach (TicTacToeSkill skill in players)
            {
                skill.waittime *= acceleration;
            }
        }

        public async void CheckWinner() {

            Debug.Log("WON CHECKING WINNER");
            if (isGameWon())
            {
                Debug.Log("GAME WON");
                screenMaterial.SetInt("_WinnerSelector", playerIdx);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                DeclareWinner(currentSkill);
                SetScreen(3);
                Accelerate();
            }
            if (isGameCompleted() == true && screen == 2)
            {

                screenMaterial.SetInt("_WinnerSelector", 2);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                DeclareWinner(null);
                Debug.Log("WON GAME OVER");
                SetScreen(3);
                Accelerate();
            }

        }

        public override void reset() {
            base.reset();
            gs = 0;
            SetScreen(firstscreen);
        }




        void DeclareWinner(Skill winner) {

            string msg = "DRAW";
            if(winner != null) {
                msg = "WINNER IS " + winner.skillid;
            }
            BroadcastToSkills(msg);
        }

      
    }


}
