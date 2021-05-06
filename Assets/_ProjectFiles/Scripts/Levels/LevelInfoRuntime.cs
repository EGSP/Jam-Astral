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
        public static WeakEvent<(LevelInfo, LevelCompleteType)> OnLevelComplete { get; private set; }
            = new WeakEvent<(LevelInfo, LevelCompleteType)>();

        /// <summary>
        /// Вызывается в том случае, если уровень уже был помечен как завершенный, но был пройден снова.
        /// </summary>
        public static WeakEvent<LevelInfo> OnLevelRecomplete { get; private set; }
            = new WeakEvent<LevelInfo>();

        /// <summary>
        /// Возвращает следующий уровень.
        /// Если текущая сцена не в списке уровней, то вернется первый уровень в списке.
        /// Если уровней в списке нет, то ничего не вернется.
        /// </summary>
        public static Option<LevelInfo> GetNextLevel()
        {
            var sceneAsLevel = GetCurrentLevel().Item2;
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
        public static ValueTuple<string,Option<LevelInfo>> GetCurrentLevel()
        {
            var scene = GameSceneManager.GetActiveScene();
            var levelInfo = LevelInfos.FirstOrDefault(x => x.LevelName == scene.name);
            if (levelInfo.IsDefault)
                return ("Текущая активная сцена не является уровнем.",Option<LevelInfo>.None);
            return (String.Empty, levelInfo);
        }

        /// <summary>
        /// Помечает текущий уровень в системе контроля как завершенный в игровом процесса.
        /// </summary>
        public static void CompleteCurrentLevel()
        {
            var tuple = GetCurrentLevel();
            var level = tuple.Item2;
            
            if (level.IsSome)
            {
                if (level.Value.Completed)
                {
                    RecompleteLevel(level.Value);
                    return;
                }
            
                var levelInfo = level.Value;
                CompleteLevel(levelInfo, LevelCompleteType.Ingame);
            }
            else
            {
                Debug.Log($"Не удалось завершить уровень по причине: {tuple.Item1}");
            }
        }

        /// <summary>
        /// Помечает уровень в системе контроля как завершенный.
        /// </summary>
        public static void CompleteLevel(LevelInfo levelInfo, LevelCompleteType completeType)
        {
            if (levelInfo.Completed == true)
            {
                RecompleteLevel(levelInfo);
                return;
            }
            
            levelInfo.Completed = true;

            LinkedListExtensions.Apply(LevelInfos, x => x.LevelName == levelInfo.LevelName,
                n => n.Value = levelInfo);
            
            // Сохраняем прогресс.
            SaveLevelInfosProgress(_levelInfos);

            // Оповещаем о завершении уровня.
            OnLevelComplete.RaiseOnce((levelInfo, completeType));
            Debug.Log($"Уровень {levelInfo.LevelName} помечен как завершенный!");
        }

        private static void RecompleteLevel(LevelInfo levelInfo)
        {
            LogAlreadyCompleted();
            OnLevelRecomplete.RaiseOnce(levelInfo);
        }
        
        private static void LogAlreadyCompleted() => Debug.Log("Уровень уже помечен как завершенный.");

        private static bool LevelListed(string name)
        {
            var coincidence = LinqExtensions.FirstOrNone(LevelInfos, x => x.LevelName == name);
            return coincidence.IsSome;
        }
    }
}