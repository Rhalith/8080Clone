using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class TileGenerator : MonoBehaviour
    {

        #region Other Components
        private Sprite _generatedTile;
        [SerializeField] private SpriteRenderer _tileRenderer;
        [SerializeField] private GridManager _gridManager;
        private TileSpec _currentTile;
        public Sprite GeneratedTile { get => _generatedTile; }
        public TileSpec CurrentTile { get => _currentTile; }
        #endregion
        void Start()
        {
            GenerateTile();
        }
        private void GenerateTile()
        {
            int i = Random.Range(0, 5);
            switch (i)
            {
                case 0:
                    _generatedTile = GameManager.TileFive;
                    _currentTile = TileSpec.five;
                    break;
                case 1:
                    _generatedTile = GameManager.TileTen;
                    _currentTile = TileSpec.ten;
                    break;
                case 2:
                    _generatedTile = GameManager.TileTwenty;
                    _currentTile = TileSpec.twenty;
                    break;
                case 3:
                    _generatedTile = GameManager.TileFourty;
                    _currentTile = TileSpec.fourty;
                    break;
                case 4:
                    _generatedTile = GameManager.TileEighty;
                    _currentTile = TileSpec.eighty;
                    break;
                default:
                    _generatedTile = GameManager.TileFive;
                    _currentTile = TileSpec.five;
                    break;
            }
            SpawnTile();
        }
        private void SpawnTile()
        {
            _tileRenderer.sprite = _generatedTile;
            //_tileRenderer.sprite = GameManager.TileEighty;
            _tileRenderer.enabled = true;
            _gridManager.AssignCurrentTile(_generatedTile, _currentTile);
            //_gridManager.AssignCurrentTile(GameManager.TileEighty, TileSpec.eighty);
        }

    }
}
