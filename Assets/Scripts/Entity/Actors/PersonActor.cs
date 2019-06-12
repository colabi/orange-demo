/* Copyright ©Seth Piezas
 */
using UnityEngine;
using System.Collections;

namespace Application {
    public class PersonActor : Actor
    {
        public static PersonActor shared;
        public Skill tic1;
        public Skill tic2;
        // Use this for initialization
        void Start()
        {
            PersonActor.shared = this;
            tic1 = SkillFactory.AcquireSkill("tictactoe", this);
            tic1.skillid = "persontic1";
            tic2 = SkillFactory.AcquireSkill("tictactoe", this);
            tic2.skillid = "persontic2";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
