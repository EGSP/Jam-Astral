using System;
using System.Linq;
using Egsp.Core;
using Egsp.Extensions.Collections;
using Egsp.Extensions.Linq;
using UnityEngine;

namespace Game.Levels
{
    // Здесь определены методы для глобального доступа к информации об уровнях.
    public partial struct LevelInfo
    {
        /// <summary>
        /// Возвращает следующий уровень.
        /// Если текущая сцена не в списке уровней, то вернется первый уровень в списке.
        /// Если уровней в списке нет, то ничего не вернется.
        /// </summary>
        public static Option<LevelInfo> GetNextLevel()
        {
            var sceneAsLevel = GetCurrentLevel();
            // Сцена является уровнем.
            if (sceneAsLevel.IsSome)
            {
                if (LevelInfos.Count > 0)
                {
                    // Уровень находится в списке.
                    if (LevelListed(sceneAsLevel.Value.LevelName))
                    {
                        var node = LevelInfos.Find(sceneAsLevel.Value);
                        // Уровень не найден в списке уровней.
                        if (node == null)
                            throw new NullReferenceException();

                        var nextNode = node.Next;
                        if (nextNode == null)
                            return Option<LevelInfo>.None;

                        return nextNode.Value;
                    }
                }
                return Option<LevelInfo>.None;
            }
            else
            {
                if (LevelInfos.Count > 0)
                    return LevelInfos.First.Value;
            }
            return Option<LevelInfo>.None;
        }
        
        /// <summary>
        /// Получение текущего уровня на основе открытой сцены.
        /// Если сцена не является игровым уровнем, то return Option.None.
        /// </summary>
        public static Option<LevelInfo> GetCurrentLevel()
        {
            var scene = GameSceneManager.GetActiveScene();
            var levelInfo = LevelInfos.FirstOrDefault(x => x.LevelName == scene.name);
            if (levelInfo.IsDefault)
                return Option<LevelInfo>.None;
            return levelInfo;
        }

        /// <summary>
        /// Помечает текущий уровень как завершенный. Сохранения не происходит.
        /// </summary>
        public static void CompleteCurrentLevel()
        {
            var level = GetCurrentLevel();
            
            if (level.IsSome)
            {
                var levelInfo = level.Value;
                levelInfo.Completed = true;

                LinkedListExtensions.Apply(LevelInfos, x => x.LevelName == levelInfo.LevelName,
                    n => n.Value = levelInfo);
                
                Debug.Log($"Уровень {levelInfo.LevelName} помечен как завершенный!");
            }
            else
            {
                Debug.Log("Не удалось завершить уровень.");
            }
        }

        private static bool LevelListed(string name)
        {
            var coincidence = LinqExtensions.FirstOrNone(LevelInfos, x => x.LevelName == name);
            return coincidence.IsSome;
        }
    }
}