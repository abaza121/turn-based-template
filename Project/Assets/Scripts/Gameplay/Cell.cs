using System;
using UnityEngine;

namespace TurnBased.Gameplay
{
    /// <summary>
    /// Controls the color and data for cell in <see cref="Position"/>.
    /// </summary>
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// Current Highlight mode of the cell in grid renderer.
        /// </summary>
        public CellHighlightMode CurrentHighlightMode
        {
            get;
            set;
        }

        /// <summary>
        /// Invoked when the cell is selected via input.
        /// </summary>
        public event Action<Cell> CellSelected;

        /// <summary>
        /// Position of the cell in the grid.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// Current occupying unit of the cell.
        /// </summary>
        public Unit       OccupyingUnit { get; set; }

        [SerializeField] private MeshRenderer meshRenderer;

        /// <summary>
        /// Changes the highlight mode to given mode.
        /// </summary>
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

        /// <summary>
        /// Serialized event listener, invokes the event for the cell.
        /// </summary>
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
