using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float wayPointToTolerence = 1f;
        [SerializeField] float wayPointLifeTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        Vector3 enemyLocation;
        float timeSinceLastSawPlayer;
        float timeSinceArrivedWayPoint;
        int currentWayPointIndex = 0;
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health= GetComponent<Health>();
            mover= GetComponent<Mover>();
            enemyLocation = transform.position;
        }

        
        void Update()
        {
            if (health.IsDead())
            {
                return;
            }

            if(DistanceToPlayer() < chaseDistance && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {
                Vector3 nextPosition = enemyLocation;
                if (patrolPath != null)
                {
                    if (AtWayPoint())
                    {
                        timeSinceArrivedWayPoint= 0;
                        CycleWayPoint();
                    }
                    nextPosition = GetNextWayPoint();
                }
                if(timeSinceArrivedWayPoint > wayPointLifeTime)
                {
                    mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                }
            }
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWayPoint += Time.deltaTime;
        }

        private Vector3 GetNextWayPoint()
        {
            return patrolPath.GetWayPointPosition(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceWayPoint = Vector3.Distance(transform.position, GetNextWayPoint());
            return distanceWayPoint < wayPointToTolerence;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
