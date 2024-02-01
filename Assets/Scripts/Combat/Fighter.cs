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
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage = 10f;
        Transform targetObject;
        float timeSinceLastAttack;
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

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
                AttackMethod();
                GetComponent<Mover>().Cancel();
            }
        }

        private void AttackMethod()
        {
            if (timeBetweenAttacks < timeSinceLastAttack)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        void Hit()
        {
            Health health = targetObject.GetComponent<Health>();
            health.TakeDamage(weaponDamage);
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
