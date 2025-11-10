using UnityEngine;

namespace TowerDefence
{
    /// <summary>
    /// Defines the different sound effects and music tracks available in the game.
    /// </summary>
    public enum Sound
    {
        BGM,
        Arrow,
        ArrowHit,
        EnemyDeath,
        TowerBuilt,
        EnemyBreakthrough,
        Win,
        Lose
    }

    /// <summary>
    /// Provides extension methods for the Sound enum, allowing for clean calls like Sound.BGM.Play().
    /// </summary>
    public static class SoundExtensions
    {
        /// <summary>
        /// The extension method that plays the given sound.
        /// </summary>
        /// <param name="sound">The 'this' keyword makes this an extension method.</param>
        public static void Play(this Sound sound)
        {
            SoundPlayer.Instance.PlaySound(sound);
        }
    }
}
