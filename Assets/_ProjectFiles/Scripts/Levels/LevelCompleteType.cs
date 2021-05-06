namespace Game.Levels
{
    /// <summary>
    /// Каким способом завершен уровень.
    /// </summary>
    public enum LevelCompleteType
    {
        /// <summary>
        /// Прохождение завершено внутри игры, т.е. по игровым правилам.
        /// </summary>
        Ingame,
        /// <summary>
        /// Прохождение завершено внешним вызовом, т.е. без игрового процесса.
        /// </summary>
        External
    }
}