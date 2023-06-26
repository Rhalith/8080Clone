using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class Tile : MonoBehaviour
    {
        #region Other Tiles
        [Header("Other Tiles")]
        [SerializeField] private Tile _aboveTile;
        [SerializeField] private Tile _belowTile;
        [SerializeField] private Tile _leftTile;
        [SerializeField] private Tile _rightTile;
        #endregion

        #region Other Components
        [Header("Components")]
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private GridManager _manager;
        #endregion

        #region Variables
        private TileSpec _tileSpec;
        private bool _isPlayerClicked = false;
        #endregion
        // Start is called before the first frame update
        void Start()
        {

        }
        private void OnMouseDown()
        {
            _renderer.sprite = _manager.GeneratedTile;
            _tileSpec = _manager.CurrentTile;
            _manager.AddTile(this);
            _isPlayerClicked = true;
            _manager.IsPlayerClicking = true;
        }
        private void OnMouseExit()
        {
            if (_manager.IsPlayerClicking)
            {
                if (_isPlayerClicked)
                {
                    _manager.DivideTile();
                    _renderer.sprite = _manager.GeneratedTile;
                    --_tileSpec;
                }
            }
        }
        private void OnMouseEnter()
        {
            if (!_manager.VisitedTiles.Contains(this))
            {
                if (_manager.IsPlayerClicking)
                {
                    if (!_isPlayerClicked)
                    {
                        _isPlayerClicked = true;
                        _manager.AddTile(this);
                        _renderer.sprite = _manager.GeneratedTile;
                        _tileSpec = _manager.CurrentTile;
                    }
                }
            }
        }
        private void OnMouseUp()
        {
            _manager.IsPlayerClicking = false;
        }
    }
}