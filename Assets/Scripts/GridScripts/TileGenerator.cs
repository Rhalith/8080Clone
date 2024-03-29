using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GridScripts
{
    public class TileGenerator : MonoBehaviour
    {

        #region Other Components
        [SerializeField] private SpriteRenderer _tileRenderer;
        [SerializeField] private GridManager _gridManager;
        #endregion

        #region Variables
        private Sprite _generatedTileSprite;
        private TileSpec _generatedTileSpec;
        #endregion

        #region Getters&Setters
        public Sprite GeneratedTileSprite { get => _generatedTileSprite; }
        public TileSpec GeneratedTileSpec { get => _generatedTileSpec; }
        #endregion

        void Start()
        {
            GenerateTile();
        }

        public void GenerateTile()
        {
            int i = Random.Range(0, 5);
            switch (i)
            {
                case 0:
                    _generatedTileSprite = GameManager.TileFive;
                    _generatedTileSpec = TileSpec.five;
                    break;
                case 1:
                    _generatedTileSprite = GameManager.TileTen;
                    _generatedTileSpec = TileSpec.ten;
                    break;
                case 2:
                    _generatedTileSprite = GameManager.TileTwenty;
                    _generatedTileSpec = TileSpec.twenty;
                    break;
                case 3:
                    _generatedTileSprite = GameManager.TileFourty;
                    _generatedTileSpec = TileSpec.fourty;
                    break;
                case 4:
                    _generatedTileSprite = GameManager.TileEighty;
                    _generatedTileSpec = TileSpec.eighty;
                    break;
                default:
                    _generatedTileSprite = GameManager.TileFive;
                    _generatedTileSpec = TileSpec.five;
                    break;
            }
            SpawnTile();
        }

        public void DisableGeneratedTile()
        {
            _tileRenderer.enabled = false;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        private void SpawnTile()
        {
            StartCoroutine(nameof(GrowSpawnedTile));
            _tileRenderer.sprite = _generatedTileSprite;
            _tileRenderer.enabled = true;
            _gridManager.AssignCurrentTile(_generatedTileSprite, _generatedTileSpec);
        }

        private IEnumerator GrowSpawnedTile()
        {
            while (gameObject.transform.localScale.x < 29.9f)
            {
                yield return new WaitForSeconds(0.005f);
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x+0.5f, gameObject.transform.localScale.y+0.5f, 1);
            }
            gameObject.transform.localScale = new Vector3(30, 30, 1);
        }
    }
}
