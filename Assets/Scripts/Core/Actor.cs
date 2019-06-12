/* Copyright ©Seth Piezas
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application
{


    public class Actor : MonoBehaviour
    {
        public string actorid;
        public List<Skill> skills;

        // Use this for initialization
        void Start()
        {
            skills = new List<Skill>();
        }


        bool hasSkill(string skill)
        {
            foreach (Skill s in skills)
            {
                if (s.type == skill) { return true; }
            }

            return false;
        }

        public virtual void Notify(string msg) {

        }

    }








}