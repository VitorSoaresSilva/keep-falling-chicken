using System;
using UnityEngine;

namespace Utilities
{
    public static class Functions
    {
        /// <summary>
        /// Transform volume from linear to logarithmic
        /// </summary>
        public static float LogarithmicDbTransform(float volume)
        {
            volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
            return volume - 80;
        }


        public static string GetSaveFileName(Enums.SaveGames saveGames)
        {
            return saveGames + ".data_version2";
        }
    
    }
}

