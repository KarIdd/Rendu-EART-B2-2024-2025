using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;


namespace HappyHarvest
{
    /// <summary>
    /// Manage everything related to the terrain where crop are planted. Hold the content of cells with the states of
    /// crop in those cells. Handle also switching tiles and the like where tilling and watering happens.
    /// </summary>
    public class TerrainManager : MonoBehaviour
    {
        public Grid Grid;
        
        private void Awake()
        {
            GameManager.Instance.Terrain = this;
        }
    }
}
