using System.Collections.Generic;
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

        public TileSpec TileSpec { get => _tileSpec; set => _tileSpec = value; }
        #endregion
        private void OnMouseDown()
        {
            if (!_isPlayerClicked)
            {
                _manager.UseGeneratedTile();
                _renderer.sprite = _manager.CurrentTileSprite;
                _tileSpec = _manager.CurrentTileSpec;
                _manager.AddTile(this);
                _isPlayerClicked = true;
                _manager.IsPlayerClicking = true;
            }
        }
        private void OnMouseExit()
        {
            if (_manager.IsPlayerClicking)
            {
                if (_isPlayerClicked)
                {
                    _manager.DivideTile();
                    _renderer.sprite = _manager.CurrentTileSprite;
                    if (!_tileSpec.Equals(TileSpec.five)) --_tileSpec;
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
                        _renderer.sprite = _manager.CurrentTileSprite;
                        _tileSpec = _manager.CurrentTileSpec;
                    }
                }
            }
        }
        private void OnMouseUp()
        {
            if (_isPlayerClicked)
            {
                _manager.IsPlayerClicking = false;
                _manager.GenerateNewTile();
                List<Tile> tiles = new();
                CheckTile(tiles);
                if(tiles.Count > 2) 
                {
                    MergeTiles(tiles);
                }
            }
        }
        public void CheckTile(List<Tile> tiles)
        {
            if (TileSpec.Equals(TileSpec.none)) return;
            CheckNextTile(_aboveTile, tiles);
            CheckNextTile(_belowTile, tiles);
            CheckNextTile(_leftTile, tiles);
            CheckNextTile(_rightTile, tiles);
        }

        private void CheckNextTile(Tile tile, List<Tile> tiles)
        {
            if (tile != null)
            {
                if (tile.TileSpec == this.TileSpec)
                {
                    if (tiles.Contains(tile)) return;
                    tiles.Add(tile);
                    tile.CheckTile(tiles);
                }
            }
        }

        private void MergeTiles(List<Tile> tiles)
        {
            foreach (var item in tiles)
            {
                Debug.Log(item.name);
            }
        }
    }
}