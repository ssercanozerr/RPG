using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange;
        Transform targetObject;
        private void Update()
        {
            if (targetObject == null)
            {
                return;
            }
            if (GetIsInRange() == false)
            {
                GetComponent<Mover>().MoveTo(targetObject.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetObject.position) < weaponRange;
        }

        public void Attack(CombatTarget target) 
        {
            GetComponent<ActionScheduler>().StartAction(this);
            targetObject = target.transform;
        }

        public void Cancel()
        {
            targetObject = null;
        }
    }
}
