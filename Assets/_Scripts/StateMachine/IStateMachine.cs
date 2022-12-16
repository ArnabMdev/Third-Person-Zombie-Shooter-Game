using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stateless;

namespace com.Arnab.ZombieAppocalypseShooter
{
	public interface IStateMachine
	{
        public void InitializeStates();
        public void InitializeStateMachine();
    } 
}
