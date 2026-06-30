
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NingaFruits
{
    public class SoundManager
    {
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, string returnString, int returnLength, IntPtr callback);

        // Slice sound channel voice pool
        private string[] sliceAliases;
        private int sliceCount;
        private int currentSliceIndex;
        private bool[] sliceLoaded;

        private bool gameOverLoaded;
        private bool bombLoaded;

        public SoundManager(string resourcesPath)
        {
            sliceCount = 4;
            sliceAliases = new string[sliceCount];
            sliceLoaded = new bool[sliceCount];
            currentSliceIndex = 0;

            // Initialize the voice pool channels for the slice sound
            for (int i = 0; i < sliceCount; i++)
            {
                sliceAliases[i] = "sliceAlias" + i;
                sliceLoaded[i] = TryLoad(Path.Combine(resourcesPath, "slice.mp3"), sliceAliases[i]);
            }

            gameOverLoaded = TryLoad(Path.Combine(resourcesPath, "gameover.mp3"),      "gameOverAlias");
            bombLoaded     = TryLoad(Path.Combine(resourcesPath, "bombexplosion.mp3"), "bombAlias");
        }

        // Opens one file under the given alias - returns true if successful
        private bool TryLoad(string path, string alias)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            // Close first in case a previous session left it open
            mciSendString("close " + alias, null, 0, IntPtr.Zero);
            int err = mciSendString("open \"" + path + "\" type mpegvideo alias " + alias, null, 0, IntPtr.Zero);
            return (err == 0);
        }

        // Play the slice sound by cycling through the voice pool
        public void PlaySlice()
        {
            if (sliceLoaded[currentSliceIndex])
            {
                // Send single "play alias from 0" command. Plays asynchronously without blocking.
                mciSendString("play " + sliceAliases[currentSliceIndex] + " from 0", null, 0, IntPtr.Zero);
                currentSliceIndex = (currentSliceIndex + 1) % sliceCount;
            }
        }

        // Play the game over sound
        public void PlayGameOver()
        {
            if (gameOverLoaded)
            {
                mciSendString("play gameOverAlias from 0", null, 0, IntPtr.Zero);
            }
        }

        // Play the bomb explosion sound
        public void PlayBombBlast()
        {
            if (bombLoaded)
            {
                mciSendString("play bombAlias from 0", null, 0, IntPtr.Zero);
            }
        }

        // Stop all sound playbacks
        public void StopAll()
        {
            for (int i = 0; i < sliceCount; i++)
            {
                if (sliceLoaded[i])
                {
                    mciSendString("stop " + sliceAliases[i], null, 0, IntPtr.Zero);
                }
            }
            if (gameOverLoaded)
            {
                mciSendString("stop gameOverAlias", null, 0, IntPtr.Zero);
            }
            if (bombLoaded)
            {
                mciSendString("stop bombAlias", null, 0, IntPtr.Zero);
            }
        }

        // Close all MCI handles to free system resources
        public void CloseAll()
        {
            for (int i = 0; i < sliceCount; i++)
            {
                if (sliceLoaded[i])
                {
                    mciSendString("close " + sliceAliases[i], null, 0, IntPtr.Zero);
                }
            }
            if (gameOverLoaded)
            {
                mciSendString("close gameOverAlias", null, 0, IntPtr.Zero);
            }
            if (bombLoaded)
            {
                mciSendString("close bombAlias", null, 0, IntPtr.Zero);
            }
        }
    }
}
