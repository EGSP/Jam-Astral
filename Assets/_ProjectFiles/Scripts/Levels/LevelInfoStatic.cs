using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Egsp.Core;
using Egsp.Extensions.Collections;
using Egsp.Files;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Levels
{
    // STATIC METHODS
    public partial struct LevelInfo
    {
        private static LinkedList<LevelInfo> _levelInfos;
        
        /// <summary>
        /// Данные об уровнях.
        /// </summary>
        public static LinkedList<LevelInfo> LevelInfos
        {
            get => _levelInfos;
            private set => _levelInfos = value;
        }
        
        private static bool AlreadyLoaded { get; set; }
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticConstructor()
        {
            LevelInfos = new LinkedList<LevelInfo>();
            ReloadLevelInfos();
        }

        public static void LoadLevelInfosToExisting(LevelsSource source, LoadMode loadMode = LoadMode.Overwrite)
        {
            var promise = LoadLevelInfos(source);

            promise.GetResult(x =>
            {
                Debug.Log($"Levels loaded: {x.Count}");
                if (loadMode == LoadMode.Add)
                    LevelInfos.Join(x);
                else
                    LevelInfos = x;
            });
        }

        /// <summary>
        /// Загрузка уровней из источника.
        /// </summary>
        public static Promise<LinkedList<LevelInfo>> LoadLevelInfos(LevelsSource source)
        {
            var task = source.LoadLevels();
            var promise = new Promise<LinkedList<LevelInfo>>();
            task.ContinueWith(x => promise.Result = x.Result);

            return promise;
        }

        /// <summary>
        /// Перезагрузка уровней. Уровни будут загружены из стандартных ассетов.
        /// </summary>
        public static void ReloadLevelInfos(bool forceReload = false)
        {
            if (AlreadyLoaded && !forceReload)
                return;

            LoadLevelInfosToExisting(new LevelsSource.LevelsFromAddressables());

            AlreadyLoaded = true;
        }

        /// <summary>
        /// Сохраняет данные об уровнях. Данные будут сохранены в отдельных файлах прогресса.
        /// </summary>
        public static void SaveLevelInfos(IEnumerable<LevelInfo> levelInfos)
        {
            Storage.Global.SaveObjectsByFiles("Levels", levelInfos,
                x => x.LevelName);
        }
        
        public enum LoadMode
        {
            Add,
            Overwrite
        }
        
        public abstract class LevelsSource
        {
            public abstract Task<LinkedList<LevelInfo>> LoadLevels();

            public class LevelsFromDirectory : LevelsSource
            {
                private readonly string _directory;

                public LevelsFromDirectory(string directory)
                {
                    _directory = directory;
                }
                
                public override async Task<LinkedList<LevelInfo>> LoadLevels()
                {
                    var linkedList = Storage.Global.LoadObjectsFromDirectory<LevelInfo>(_directory);
                    return linkedList;
                }
            }

            public class LevelsFromAddressables : LevelsSource
            {
                private LinkedList<LevelInfo> _levelInfos = new LinkedList<LevelInfo>();
                
                public override async Task<LinkedList<LevelInfo>> LoadLevels()
                {
                    var ao =
                        Addressables.LoadAssetsAsync<TextAsset>("level_meta", OnTextLoaded);

                    await ao.Task;

                    var ordered = _levelInfos.OrderBy(x => x.OrderId);
                    var orderedList = new LinkedList<LevelInfo>();
                    foreach (var levelInfo in ordered)
                    {
                        orderedList.AddLast(levelInfo);
                    }

                    _levelInfos.Clear();
                    return orderedList;
                }

                public void OnTextLoaded(TextAsset asset)
                {
                    var levelInfo = Storage.DefaultSerializer.Deserialize<LevelInfo>(asset.bytes);

                    if (levelInfo.IsSome)
                        _levelInfos.AddLast(levelInfo.Value);
                }
            }
        }
    }
}