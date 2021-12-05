using System;
using UnityEngine;

namespace TurnBased.Gameplay
{
    /// <summary>
    /// Controls the color and data for cell in <see cref="Position"/>.
    /// </summary>
    public class Cell : MonoBehaviour
    {
        public CellHighlightMode CurrentHighlightMode
        {
            get;
            set;
        }

        public event Action<Cell> CellSelected;

        public Vector2Int Position      { get; set; }
        public Unit       OccupyingUnit { get; set; }

        [SerializeField] private MeshRenderer meshRenderer;

        public void ChangeHighlight(CellHighlightMode cellHighlightMode)
        {
            switch (cellHighlightMode)
            {
                case CellHighlightMode.NotHighlighted:
                    this.meshRenderer.material.color = Color.white;
                    break;
                case CellHighlightMode.CanChooseIt:
                    this.meshRenderer.material.color = Color.yellow;
                    break;
                case CellHighlightMode.CanMoveTo:
                    this.meshRenderer.material.color = Color.green;
                    break;
                case CellHighlightMode.CanAttack:
                    this.meshRenderer.material.color = Color.red;
                    break;
            }

            this.CurrentHighlightMode = cellHighlightMode;
        }

        public void OnCellPressed() => this.CellSelected?.Invoke(this);
    }

    public enum CellHighlightMode
    {
        NotHighlighted, // lame white
        CanChooseIt,    // yellow?
        CanMoveTo,
        CanAttack
    }
}
