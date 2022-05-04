using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : Module
{
   
   public string state1 {get;set;}

   public string state2 {get;set;}

   public int _state = 1;
   public int state {
      get {
         return _state;
      }
      set {
         if (_state != value) {
            _state = value;
            Init();
         }
      }
   }

   public override void Init () {
      base.Init();
      if (state == 1) {
         ModuleParser.Parse(gameObject,state2,false);
         Debug.Log("state1 = " + state1);
         ModuleParser.Parse(gameObject,state1);
      } else {
         ModuleParser.Parse(gameObject,state1,false);
         Debug.Log("state2 = " + state2);
         ModuleParser.Parse(gameObject,state2);
      }
   }
}
