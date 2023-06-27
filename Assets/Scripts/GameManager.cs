using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        #region Tile Sprites
        [Header("Tile Sprites")]
        [SerializeField] private Sprite _emptyTile;
        [SerializeField] private Sprite _tileFive;
        [SerializeField] private Sprite _tileTen;
        [SerializeField] private Sprite _tileTwenty;
        [SerializeField] private Sprite _tileFourty;
        [SerializeField] private Sprite _tileEighty;
        [SerializeField] private Sprite _tileMax;
        #endregion

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);
        }
        public static Sprite EmptyTile
        {
            get
            {
                if (_instance == null) return null;
                return _instance._emptyTile;
            }
        }
        public static Sprite TileFive
        {
            get
            {
                if (_instance == null) return null;
                return _instance._tileFive;
            }
        }

        public static Sprite TileTen
        {
            get
            {
                if (_instance == null) return null;
                return _instance._tileTen;
            }
        }
        public static Sprite TileTwenty
        {
            get
            {
                if (_instance == null) return null;
                return _instance._tileTwenty;
            }
        }
        public static Sprite TileFourty
        {
            get
            {
                if (_instance == null) return null;
                return _instance._tileFourty;
            }
        }
        public static Sprite TileEighty
        {
            get
            {
                if (_instance == null) return null;
                return _instance._tileEighty;
            }
        }
        public static Sprite TileMax
        {
            get
            {
                if (_instance == null) return null;
                return _instance._tileMax;
            }
        }
    }


}