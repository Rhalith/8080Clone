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
                List<Tile> tiles = new();
                _manager.VisitedTiles[^1].CheckTile(tiles);
                if (tiles.Count > 2)
                {
                    MergeTiles(tiles);
                }
                tiles.Clear();
                CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._aboveTile);
                CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._belowTile);
                CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._leftTile);
                CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._rightTile);
                _manager.IsPlayerClicking = false;
                _manager.GenerateNewTile();
                _manager.ClearTiles();
            }
        }
        public void MultiplyTile()
        {
            if (_tileSpec.Equals(TileSpec.max))
            {
                ExplodeTile();
                ResetTile();
                return;
            }
            _tileSpec++;
            switch (_tileSpec)
            {
                case TileSpec.ten:
                    _renderer.sprite = GameManager.TileTen;
                    break;
                case TileSpec.twenty:
                    _renderer.sprite = GameManager.TileTwenty;
                    break;
                case TileSpec.fourty:
                    _renderer.sprite = GameManager.TileFourty;
                    break;
                case TileSpec.eighty:
                    _renderer.sprite = GameManager.TileEighty;
                    break;
                case TileSpec.max:
                    _renderer.sprite = GameManager.TileMax;
                    break;
            }
        } 
        //TODO
        private void ExplodeTile()
        {

        }
        public void ResetTile()
        {
            _isPlayerClicked = false;
            _tileSpec = TileSpec.none;
            _renderer.sprite = GameManager.EmptyTile;
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
            int i = Random.Range(0, tiles.Count);
            tiles[i].MultiplyTile();
            tiles.Remove(tiles[i]);
            foreach (var item in tiles)
            {
                item.ResetTile();
            }
        }

        private void CheckAfterMerge(List<Tile> tiles, Tile tile)
        {
            if (tile == null) return;
            tile.CheckTile(tiles);
            if (tiles.Count > 2)
            {
                MergeTiles(tiles);
            }
            tiles.Clear();
        }
    }
}