using Assets.Scripts.GridScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TileAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Start()
        {
            _animator.speed = 2.0f;
        }
        public void StartRainbow()
        {
            SetExplode(0);
        }
        public void StartExplode()
        {
            SetExplode(1);
        }

        public void ResetTile()
        {
            gameObject.GetComponentInParent<Tile>().ResetTile();
        }

        private void SetExplode(int i)
        {
            _animator.SetBool("isExploded", i != 0);
        }

    }
}