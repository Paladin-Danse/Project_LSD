/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;
using UnrealFPS.Utility;

namespace UnrealFPS.Runtime
{
    public static class SurfaceHelper
    {  
        /// <summary>
        /// If surface terrain return contact point terrain texture,
        /// else return physic material.
        /// </summary>
        public static Object GetSurfaceAuto(Collider collider, Vector3 contactPos)
        {
            Terrain terrain = collider.GetComponent<Terrain>();
            PhysicMaterial physicMaterial = collider.GetComponent<Collider>().sharedMaterial;
            if (terrain != null)
            {
                TerrainTextureDetector terrainTextureDetector = new TerrainTextureDetector(terrain.terrainData);
                Texture2D texture = terrainTextureDetector.GetActiveTexture(contactPos);
                return texture;
            }
            return physicMaterial;
        }

        /// <summary>
        /// Return surface physic material.
        /// </summary>
        public static PhysicMaterial GetSurfaceMaterial(Collider collider, Vector3 contactPos)
        {
            return collider.GetComponent<Collider>().sharedMaterial;
        }

        /// <summary>
        /// Return contact point terrain texture.
        /// </summary>
        public static Texture2D GetSurfaceTexture(Collider collider, Vector3 contactPos)
        {
            Terrain terrain = collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                TerrainTextureDetector terrainTextureDetector = new TerrainTextureDetector(terrain.terrainData);
                Texture2D texture = terrainTextureDetector.GetActiveTexture(contactPos);
                return texture;
            }
            return null;
        }
    }
}