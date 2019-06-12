/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;


namespace Application
{



    public enum PlatformPosition
    {
        Up = 0,
        Down = 1
    }

    public class SceneRunner : System.Object
    {
        public string id = "";
        public TicTacToeGameContext game;
        public void configure()
        {
            //CONFIGURE ANIMATION
        }
        public virtual void start() { }
        public virtual void end() { }
        public SceneRunner()
        {
            Debug.Log("const scenerunner");
        }
    }


    public class ComputerScene : SceneRunner
    {
        public DemoPlatform platform = SystemManager.shared.platform;
        public Computer computer = SystemManager.shared.platform.computer;
        public PersonActor person = PersonActor.shared;
        public AIActor ai = AIActor.shared;
        public PlatformPosition platformPosition
        {
            get
            {
                if (platform.platform.transform.localPosition.y < 0.1)
                    return PlatformPosition.Down;
                else
                    return PlatformPosition.Up;
            }
            set
            {
                string animname = "Up";
                if (value == PlatformPosition.Down) animname = "Down";
                Debug.Log("ANIM: " + animname);
                var animation = platform.GetComponent<Animation>();
                animation[animname].speed = 0.5F;
                animation.Play(animname);
            }

        }
        public override void start()
        {
            base.start();
            game = (TicTacToeGameContext)ActivityManager.shared.contexts["tictactoe"];
            if (game != null) { game.reset(); }
        }
        public ComputerScene()
        {
            Debug.Log("const computer");
        }
    }

    public class AppleSceneIntroduction : ComputerScene
    {
        public AppleSceneIntroduction() : base()
        {

        }
        public override void start()
        {
            base.start();
            id = "introduction";

        }
    }
    public class AppleTwoPlayerScene : ComputerScene
    {
        public AppleTwoPlayerScene()
        {
            Debug.Log("const twopl");
        }
        public override void start()
        {
            base.start();
            id = "twoplayer";

            if (game != null)
            {
                game.RemoveAllPlayers();
                game.reset();
                game.delay = 1.0F;

                game.firstscreen = 2;
                game.joinContext(person.tic1);
                game.joinContext(person.tic2);

            }
            //IF NOT RAISED, RUN RAISE ANIMATION
        }
    }
    public class AppleSinglePlayerScene : ComputerScene
    {
        public override void start()
        {
            base.start();
            id = "singleplayer";
            if (game != null)
            {
                game.RemoveAllPlayers();
                game.reset();
                game.delay = 1.0F;

                game.firstscreen = 2;
                game.joinContext(person.tic1);
                game.joinContext(ai.tic2);
            }
            //IF NOT RAISED, RUN RAISE ANIMATION
        }
    }
    public class AppleZeroPlayerScene : ComputerScene
    {
        public override void start()
        {
            base.start();
            id = "zeroplayer";

            if (game != null)
            {
                game.RemoveAllPlayers();
                game.reset();
                game.delay = 1.0F;

                game.firstscreen = 2;
                game.acceleration = 0.7F;
                game.joinContext(ai.tic1);
                game.joinContext(ai.tic2);
            }
            Complete();
        }

        async void Complete()
        {
            if (platformPosition == PlatformPosition.Down)
            {
                Debug.Log("ANIM SHOULD RUN LIFT ANIM");
                platformPosition = PlatformPosition.Up;
            }

            await Task.Delay(TimeSpan.FromSeconds(10));
        }

    }



    public class SystemManager : MonoBehaviour
    {
        public static SystemManager shared;
        public DemoPlatform platform;
        public SceneRunner runner;
        public Camera mainCamera;
        public Texture debugTexture;
        public Material screenMaterial;
        // Use this for initialization


        IEnumerator Start()
        {

            //yield return UnityEngine.Application.RequestUserAuthorization(UserAuthorization.Microphone);
            //if (UnityEngine.Application.HasUserAuthorization(UserAuthorization.Microphone))
            //{
            //    Debug.Log("Myxed Unity Microphone permissions");
            //}

            shared = this;


            ActivityManager.shared.Configure();
            AudioManager.shared.Configure();

            Debug.Log("STARTING SYSTEM");
            yield return null;
        }

        // Update is called once per frame
        void Update()
        {
            CollectInput();
        }


        void CollectInput()
        {
            RaycastHit hit;
            int mode = 0;
#if UNITY_EDITOR
            Vector3 pos = Input.mousePosition;
            mode = Input.GetMouseButtonDown(0) ? 1 : 0;
#else
            Vector3 pos = new Vector3(Screen.width * 0.5F, Screen.height * 0.5F, 0.0F);
            mode = Input.GetMouseButtonDown(0) ? 1 : 0;

#endif


            Ray ray = mainCamera.ScreenPointToRay(pos);
          
            if (Physics.Raycast(ray,out hit))
            {

                InputDevice input = hit.transform.gameObject.GetComponent<InputDevice>();
                if(input != null && runner?.id != "zeroplayer") {
                    //Debug.Log("HIT AN INPUT DEVICE: " + mode);

                    MouseEvent me = new MouseEvent();
                    me.position = hit.textureCoord;
                    me.mode = mode;
                    if(input.evt != null) {
                        MouseEvent le = (MouseEvent)input.evt;
                        if (le.mode == 1 && me.mode == 0) return;
                    }

                    input.AddEvent(me);

                }
                if(hit.transform.gameObject.name == "projsource" || hit.transform.gameObject.name == "GroundMoving") {
                    if(mode == 1) {
                        var originalfirst = ((TicTacToeGameContext)runner.game).firstscreen;
                        ((TicTacToeGameContext)runner.game).firstscreen = 0;
                        runner.game.reset();
                        ((TicTacToeGameContext)runner.game).firstscreen = originalfirst;
                    }
                }
            }

        }


        public void RunFlow(string flow) {
            Debug.Log("runflow " + flow);
            if (runner != null && runner.id == flow) return;
            if(runner != null) {
                runner.end();
            }


            SceneRunner ns = null;
            if (flow == "introduction") {
                ns = new AppleSceneIntroduction();
            } else if (flow == "twoplayer") {
                ns = new AppleTwoPlayerScene();
            } else if (flow == "singleplayer") {
                ns = new AppleSinglePlayerScene();
            } else if (flow == "zeroplayer") {
                ns = new AppleZeroPlayerScene();
            }
            if (ns != null )
            {
                Debug.Log("Attempting run new scene: " + flow);
                runner = ns;
                runner.configure();
                runner.start();
            }

        }

        public void SetPlayerNumber(int humancount) {
            if (humancount == 2) RunFlow("twoplayer");
            if (humancount == 1) RunFlow("singleplayer");
            if (humancount == 0) RunFlow("zeroplayer");
        }



    }
}
 