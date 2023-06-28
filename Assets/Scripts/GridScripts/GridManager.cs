using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class GridManager : MonoBehaviour
    {
        #region Other Components
        [SerializeField] private TileGenerator _generator;
        [SerializeField] private Score _score;
        #endregion

        #region Variables
        private TileSpec _previousTileSpec;
        private TileSpec _currentTileSpec;
        private Sprite _currentTileSprite;
        private List<Tile> _visitedTiles = new();
        private List<Tile> _visitableTiles = new();
        private bool _isPlayerExitedFromTile = false;
        private bool _isPlayerClicking = false;
        #endregion

        #region Getters&Setters
        public bool IsPlayerClicking { get => _isPlayerClicking; set => _isPlayerClicking = value; }
        public TileSpec CurrentTileSpec { get => _currentTileSpec; }
        public Sprite CurrentTileSprite { get => _currentTileSprite; }
        public bool IsPlayerExitedFromTile { get => _isPlayerExitedFromTile; set => _isPlayerExitedFromTile = value; }
        public TileSpec PreviousTileSpec { get => _previousTileSpec; }
        public List<Tile> VisitedTiles { get => _visitedTiles; set => _visitedTiles = value; }
        public List<Tile> VisitableTiles { get => _visitableTiles; set => _visitableTiles = value; }
        #endregion

        public void AssignCurrentTile(Sprite tile, TileSpec spec)
        {
            _currentTileSprite = tile;
            _currentTileSpec = spec;
        }
        public void PickGeneratedTile()
        {
            _generator.DisableGeneratedTile();
        }
        public void GenerateNewTile()
        {
            _generator.GenerateTile();
        }
        public void DivideTile()
        {
            _previousTileSpec = _currentTileSpec;
            if (_currentTileSpec != TileSpec.five)
            {
                LowerTileSpec(_currentTileSpec);
            }
            else
            {
                _isPlayerClicking = false;
            }
        }
        public void AddScore(TileSpec tileSpec)
        {
            switch (tileSpec)
            {
                case TileSpec.ten:
                    _score.AddScore(10);
                    break;
                case TileSpec.twenty:
                    _score.AddScore(20);
                    break;
                case TileSpec.fourty:
                    _score.AddScore(40);
                    break;
                case TileSpec.eighty:
                    _score.AddScore(80);
                    break;
                case TileSpec.max:
                    _score.AddScore(160);
                    break;
                default:
                    _score.AddScore(320);
                    break;
            }
        }
        private void LowerTileSpec(TileSpec currentSpec)
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
