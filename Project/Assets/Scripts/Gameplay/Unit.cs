using TurnBased.Data;
using UnityEngine;
using DG.Tweening;
using System;

namespace TurnBased.Gameplay
{
    public class Unit : MonoBehaviour
    {
        public Vector2Int AttackRange
        {
            get => m_attackRange;
        }    

        public Vector2Int MoveRange
        {
            get => m_moveRange;
        }

        public Vector2Int CurrentCellPosition { get; set; }
        public int OwningPlayer { get; set; }
        int m_maxHealth;
        int m_damage;
        int m_currentHealth;
        Vector2Int m_attackRange;
        Vector2Int m_moveRange;

        public void SetInitialState(UnitSetup setup)
        {
            this.m_maxHealth = setup.Health;
            this.m_currentHealth = setup.Health;
            this.m_attackRange = setup.AttackRange;
            this.m_moveRange = setup.MoveRange;
            this.m_damage = setup.Damage;
        }

        public void MoveUnitFromTo(Cell currentCell, Cell targetCell, Action onComplete)
        {
            this.transform.DOMove(targetCell.transform.position, 1).OnComplete(() => OnMovementComplete(targetCell, onComplete));
            var forwardLook = targetCell.transform.position - this.transform.position;
            this.transform.DORotateQuaternion(Quaternion.LookRotation(forwardLook), 1);
            currentCell.OccupyingUnit = null;
        }

        public void AttackCell(Cell targetCell) => targetCell.TakeDamage(m_damage);
        public void TakeDamage(int damage)      => this.m_currentHealth -= damage;

        void OnMovementComplete(Cell targetCell, Action onComplete)
        {
            Debug.Log("Move Complete");
            this.CurrentCellPosition = targetCell.Position;
            targetCell.OccupyingUnit = this;
            onComplete.Invoke();
        }
    }
}
