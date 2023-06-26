using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private TileGenerator _generator;
        private bool _isPlayerClicking = false;
        private TileSpec _currentTileSpec;
        private Sprite _currentTileSprite;

        public List<Tile> VisitedTiles;
        public bool IsPlayerClicking { get => _isPlayerClicking; set => _isPlayerClicking = value; }
        public TileSpec CurrentTileSpec { get => _currentTileSpec; }
        public Sprite CurrentTileSprite { get => _currentTileSprite; }


        public void AssignCurrentTile(Sprite tile, TileSpec spec)
        {
            _currentTileSprite = tile;
            _currentTileSpec = spec;
        }
        public void AddTile(Tile tile)
        {
            VisitedTiles.Add(tile);
        }
        public void ClearTiles()
        {
            VisitedTiles.Clear();
        }
        public void UseGeneratedTile()
        {
            _generator.UseGeneratedTile();
        }
        public void GenerateNewTile()
        {
            _generator.GenerateTile();
        }
        public void DivideTile()
        {
            if (_currentTileSpec != TileSpec.five)
            {
                LowerTileSpec(_currentTileSpec);
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
                    _currentTileSprite = GameManager.TileFive;
                    break;
                case TileSpec.twenty:
                    _currentTileSprite = GameManager.TileTen;
                    break;
                case TileSpec.fourty:
                    _currentTileSprite = GameManager.TileTwenty;
                    break;
                case TileSpec.eighty:
                    _currentTileSprite = GameManager.TileFourty;
                    break;
                default:
                    _currentTileSprite = GameManager.TileFive;
                    break;
            }
            if (!_currentTileSpec.Equals(TileSpec.five)) --_currentTileSpec;
        }
    }
}
