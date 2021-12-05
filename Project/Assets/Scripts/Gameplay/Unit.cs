using TurnBased.Data;
using UnityEngine;
using DG.Tweening;
using System;

namespace TurnBased.Gameplay
{
    /// <summary>
    /// Controls Unit runtime data, current health and available moves.
    /// </summary>
    public class Unit : MonoBehaviour
    {
        public event Action HealthChanged;
        public event Action UnitDead;

        public Vector2Int AttackRange
        {
            get => m_attackRange;
        }    

        public Vector2Int Energy
        {
            get => m_energy;
        }

        public float HealthPercent
        {
            get => Mathf.Clamp01((float)m_currentHealth / m_maxHealth);
        }

        public bool UnitAttacked
        {
            get => this.unitAttacked;
        }

        public Vector2Int CurrentCellPosition { get; set; }
        public int OwningPlayer { get; set; }
        int m_maxHealth;
        int m_damage;
        int m_currentHealth;
        Vector2Int m_attackRange;
        Vector2Int m_moveRange;
        Vector2Int m_energy;
        bool unitAttacked;

        public void SetInitialState(UnitSetup setup)
        {
            this.m_maxHealth = setup.Health;
            this.m_currentHealth = setup.Health;
            this.m_attackRange = setup.AttackRange;
            this.m_moveRange = setup.MoveRange;
            this.m_damage = setup.Damage;
        }

        public void ResetEnergy()
        {
            this.unitAttacked = false;
            this.m_energy = this.m_moveRange;
        }

        public void MoveUnitFromTo(Cell currentCell, Cell targetCell, Action onComplete)
        {
            this.transform.DOMove(targetCell.transform.position, 1).OnComplete(() => OnMovementComplete(targetCell, onComplete));
            var forwardLook = targetCell.transform.position - this.transform.position;

            this.TakeEnergy(currentCell.Position - targetCell.Position);
            this.transform.DORotateQuaternion(Quaternion.LookRotation(forwardLook), 1);
            currentCell.OccupyingUnit = null;
        }

        public void AttackUnit(Unit targetUnit)
        {
            targetUnit.TakeDamage(m_damage);
            m_energy = Vector2Int.zero;
            this.unitAttacked = true;
        }

        public void TakeDamage(int damage)
        {
            this.m_currentHealth -= damage;
            this.HealthChanged?.Invoke();
            if(this.m_currentHealth <= 0) UnitDead?.Invoke();
        }

        void TakeEnergy(Vector2Int delta)
        {
            delta.x = Mathf.Abs(delta.x);
            delta.y = Mathf.Abs(delta.y);
            this.m_energy   = this.m_energy - delta;
            this.m_energy.x = Mathf.Max(this.m_energy.x, 0);
            this.m_energy.y = Mathf.Max(this.m_energy.y, 0);
        }

        void OnMovementComplete(Cell targetCell, Action onComplete)
        {
            Debug.Log("Move Complete");
            this.CurrentCellPosition = targetCell.Position;
            targetCell.OccupyingUnit = this;
            onComplete.Invoke();
        }
    }
}
