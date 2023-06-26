using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private TileGenerator _generator;
        private bool _isPlayerClicking = false;
        private TileSpec _currentTile;
        private Sprite _generatedTile;

        public List<Tile> VisitedTiles;
        public bool IsPlayerClicking { get => _isPlayerClicking; set => _isPlayerClicking = value; }
        public TileSpec CurrentTile { get => _currentTile; }
        public Sprite GeneratedTile { get => _generatedTile; }


        public void AssignCurrentTile(Sprite tile, TileSpec spec)
        {
            _generatedTile = tile;
            _currentTile = spec;
        }
        public void AddTile(Tile tile)
        {
            VisitedTiles.Add(tile);
        }
        public void DivideTile()
        {
            if (_currentTile != TileSpec.five)
            {
                LowerTileSpec(_currentTile);
            }
            else
            {
                _isPlayerClicking = false;
            }
        }

        public void LowerTileSpec(TileSpec currentSpec)
        {
            switch (currentSpec)
            {
                case TileSpec.ten:
                    _generatedTile = GameManager.TileFive;
                    break;
                case TileSpec.twenty:
                    _generatedTile = GameManager.TileTen;
                    break;
                case TileSpec.fourty:
                    _generatedTile = GameManager.TileTwenty;
                    break;
                case TileSpec.eighty:
                    _generatedTile = GameManager.TileFourty;
                    break;
                default:
                    _generatedTile = GameManager.TileFive;
                    break;
            }
            --_currentTile;
        }

        public Sprite FindLowerTile(TileSpec spec)
        {
            switch (spec)
            {
                case TileSpec.ten:
                    return GameManager.TileFive;
                case TileSpec.twenty:
                    return GameManager.TileTen;
                case TileSpec.fourty:
                    return GameManager.TileTwenty;
                case TileSpec.eighty:
                    return GameManager.TileFourty;
                default:
                    return GameManager.TileFive;
            }
        }
    }
}
