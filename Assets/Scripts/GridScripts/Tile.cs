using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
                _manager.VisitableTiles = FillVisitableList();
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
                    if (!_tileSpec.Equals(TileSpec.five))
                    {
                        _manager.IsPlayerExitedFromTile = true;
                        _manager.DivideTile();
                        _renderer.sprite = _manager.CurrentTileSprite;
                        --_tileSpec;
                    }
                    else if (_tileSpec.Equals(TileSpec.five))
                    {
                        _manager.IsPlayerClicking = false;
                        _renderer.sprite = _manager.CurrentTileSprite;
                    }
                }
            }
        }
        private void OnMouseEnter()
        {
            if (_manager.VisitableTiles.Contains(this))
            {
                if (_manager.IsPlayerClicking)
                { 
                    if (!_isPlayerClicked)
                    {
                        _manager.IsPlayerExitedFromTile = false;
                        if (!_manager.CurrentTileSpec.Equals(TileSpec.five))
                        {
                            _manager.VisitableTiles.Clear();
                            _manager.VisitableTiles = FillVisitableList();
                            _isPlayerClicked = true;
                            _manager.AddTile(this);
                            _renderer.sprite = _manager.CurrentTileSprite;
                            _tileSpec = _manager.CurrentTileSpec;
                        }
                        else if (_manager.PreviousTileSpec.Equals(TileSpec.ten))
                        {
                            _manager.DivideTile();
                            _manager.UseGeneratedTile();
                            _renderer.sprite = _manager.CurrentTileSprite;
                            _tileSpec = _manager.CurrentTileSpec;
                            _manager.AddTile(this);
                            _manager.VisitableTiles = FillVisitableList();
                            _isPlayerClicked = true;
                            _manager.IsPlayerClicking = false;
                        }
                    }
                }
            }
            else
            {
                _manager.IsPlayerClicking = false;
            }
        }
        private void OnMouseUp()
        {
            if (_isPlayerClicked)
            {
                if (_manager.VisitableTiles.SequenceEqual(_manager.VisitedTiles[^1].FillVisitableList()))
                {
                    if (_manager.IsPlayerExitedFromTile) 
                    {
                        _manager.VisitedTiles[^1].MultiplyTile();
                    } 
                }

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

                _manager.IsPlayerExitedFromTile = false;
                _manager.IsPlayerClicking = false;
                _manager.GenerateNewTile();
                _manager.ClearTiles();
                _manager.VisitableTiles.Clear();
            }
        }

        //TODO
        private void ExplodeTile()
        {

        }

        private List<Tile> FillVisitableList()
        {
            List<Tile> tiles = new();
            if(_rightTile != null)
                tiles.Add(_rightTile);
            if(_leftTile != null)
                tiles.Add(_leftTile);
            if (_belowTile != null)
                tiles.Add(_belowTile);
            if (_aboveTile != null)
                tiles.Add(_aboveTile);
            return tiles;
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
    }
}