using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class Tile : MonoBehaviour
    {
        #region Neighbours
        [Header("Neigbours")]
        [SerializeField] private Tile _aboveTile;
        [SerializeField] private Tile _belowTile;
        [SerializeField] private Tile _leftTile;
        [SerializeField] private Tile _rightTile;
        #endregion

        #region Other Components
        [Header("Components")]
        [SerializeField] private GameObject _rainbowObject;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private GridManager _manager;
        #endregion

        #region Variables
        private TileSpec _tileSpec = TileSpec.none;
        private bool _isPlayerClicked = false;
        #endregion

        #region Mouse Functions
        private void OnMouseDown()
        {
            ClickTile();
        }
        private void OnMouseExit()
        {
            ExitTile();
        }
        private void OnMouseEnter()
        {
            if (_manager.VisitableTiles.Contains(this))
            {
                EnterTile();
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
                if (_manager.VisitableTiles.SequenceEqual(_manager.VisitedTiles[^1].GetVisitableList()))
                {
                    if (_manager.IsPlayerExitedFromTile)
                    {
                        _manager.VisitedTiles[^1].MultiplyTile();
                    }
                }
                MergeTiles();
                ResetManager();
            }
        }
        #endregion

        private void AddScore()
        {
            _manager.AddScore(_tileSpec);
        }

        private void MultiplyTile()
        {
            if (_tileSpec.Equals(TileSpec.max))
            {
                ExplodeTile();
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
                    _renderer.sprite = GameManager.EmptyTile;
                    _rainbowObject.GetComponent<Animator>().enabled = true;
                    _rainbowObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    break;
            }
        }

        private void ExplodeTile()
        {
            _rainbowObject.GetComponent<Animator>().SetBool("isExploded", true);
            if (_rightTile != null)
            {
                _rightTile.ResetTile();
                if (_rightTile._aboveTile != null)
                {
                    _rightTile._aboveTile.ResetTile();
                }
                if (_rightTile._belowTile != null)
                {
                    _rightTile._belowTile.ResetTile();
                }
            }

            if (_leftTile != null)
            {
                _leftTile.ResetTile();
                if (_leftTile._aboveTile != null)
                {
                    _leftTile._aboveTile.ResetTile();
                }
                if (_leftTile._belowTile != null)
                {
                    _leftTile._belowTile.ResetTile();
                }
            }
            if (_belowTile != null)
                _belowTile.ResetTile();
            if (_aboveTile != null)
                _aboveTile.ResetTile();
            _manager.AddScore(TileSpec.none);
        }

        public void ResetTile()
        {
            _isPlayerClicked = false;
            _tileSpec = TileSpec.none;
            _renderer.sprite = GameManager.EmptyTile;
            _rainbowObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            _rainbowObject.GetComponent<SpriteRenderer>().GetComponent<Animator>().SetBool("isExploded", false);
            _rainbowObject.GetComponent<SpriteRenderer>().GetComponent<Animator>().enabled = false;
        }

        private void ClickTile()
        {
            if (!_isPlayerClicked)
            {
                _isPlayerClicked = true;
                _manager.IsPlayerClicking = true;
                _manager.VisitedTiles.Add(this);
                _manager.VisitableTiles = GetVisitableList();
                _manager.PickGeneratedTile();
                _renderer.sprite = _manager.CurrentTileSprite;
                _tileSpec = _manager.CurrentTileSpec;
            }
        }

        private void ExitTile()
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

        private void EnterTile()
        {
            if (_manager.IsPlayerClicking)
            {
                if (!_isPlayerClicked)
                {
                    _manager.IsPlayerExitedFromTile = false;
                    if (!_manager.CurrentTileSpec.Equals(TileSpec.five))
                    {
                        _isPlayerClicked = true;
                        _renderer.sprite = _manager.CurrentTileSprite;
                        _tileSpec = _manager.CurrentTileSpec;
                        _manager.VisitableTiles.Clear();
                        _manager.VisitableTiles = GetVisitableList();
                        _manager.VisitedTiles.Add(this);

                    }
                    else if (_manager.PreviousTileSpec.Equals(TileSpec.ten))
                    {
                        _manager.DivideTile();
                        _renderer.sprite = _manager.CurrentTileSprite;
                        _tileSpec = _manager.CurrentTileSpec;
                        _isPlayerClicked = true;
                        _manager.VisitedTiles.Add(this);
                        _manager.VisitableTiles = GetVisitableList();
                        _manager.IsPlayerClicking = false;
                    }
                }
            }
        }

        private void CheckTile(List<Tile> tiles)
        {
            if (_tileSpec.Equals(TileSpec.none)) return;
            CheckNeighbors(tiles);
        }

        private void CheckNeighbors(List<Tile> tiles)
        {
            List<Tile> neighbors = new()
            {
                _aboveTile,
                _belowTile,
                _leftTile,
                _rightTile
            };
            foreach (var tile in neighbors)
            {
                if (tile != null)
                {
                    if (tile._tileSpec == _tileSpec)
                    {
                        if (tiles.Contains(tile)) return;
                        tiles.Add(tile);
                        tile.CheckTile(tiles);
                    }
                }
            }

        }

        private void StartMerge(List<Tile> tiles)
        {
            int i = Random.Range(0, tiles.Count);
            tiles[i].MultiplyTile();
            tiles[i].AddScore();
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
                StartMerge(tiles);
            }
            tiles.Clear();
        }

        private void MergeTiles()
        {
            List<Tile> tiles = new();
            _manager.VisitedTiles[^1].CheckTile(tiles);
            if (tiles.Count > 2)
            {
                StartMerge(tiles);
            }
            tiles.Clear();
            CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._aboveTile);
            CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._belowTile);
            CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._leftTile);
            CheckAfterMerge(tiles, _manager.VisitedTiles[^1]._rightTile);
        }

        private void ResetManager()
        {
            _manager.IsPlayerExitedFromTile = false;
            _manager.IsPlayerClicking = false;
            _manager.GenerateNewTile();
            _manager.VisitedTiles.Clear();
            _manager.VisitableTiles.Clear();
        }

        private List<Tile> GetVisitableList()
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

    }
}