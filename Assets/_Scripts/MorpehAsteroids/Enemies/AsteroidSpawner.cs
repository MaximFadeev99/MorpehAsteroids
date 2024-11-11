using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MorpehAsteroids.Enemies
{
    [Serializable]
    public class AsteroidSpawner
    {
        [SerializeField] private AssetReference _asteroidPrefabReference;

        private Vector3 _instantiationPosition;
        private AsyncOperationHandle _loadAsteroidHandle;

        internal int QueuedSpawns { get; private set; }

        internal event Action<AsteroidProvider> AsteroidSpawned;

        internal void Initialize() 
        {
            _instantiationPosition = new Vector3(-50f, -10f, -50f);
        }

        internal void Dispose() 
        {
            if(_loadAsteroidHandle.IsValid() == true)
                Addressables.Release(_loadAsteroidHandle);
        }

        internal async void SpawnAsteroids(int numberToSpawn, float spawnPeriod) 
        {
            float waitInterval = numberToSpawn == 1 ? 0f : spawnPeriod / numberToSpawn;
            QueuedSpawns += numberToSpawn;

            for (int i = 0; i < numberToSpawn; i++) 
            {
                if (_loadAsteroidHandle.IsValid() == false) 
                {
                    _loadAsteroidHandle = Addressables.LoadAssetAsync<GameObject>(_asteroidPrefabReference);
                    await UniTask.WaitUntil(() => _loadAsteroidHandle.IsDone == true);
                }

                AsyncOperationHandle instantiationHandle = _asteroidPrefabReference
                    .InstantiateAsync(_instantiationPosition, Quaternion.identity);
                await UniTask.WaitUntil(() => instantiationHandle.IsDone == true);

                AsteroidProvider spawnedAsteroid = ((GameObject)instantiationHandle.Result)
                    .GetComponent<AsteroidProvider>();
                QueuedSpawns--;
                AsteroidSpawned?.Invoke(spawnedAsteroid);
                await UniTask.WaitForSeconds(waitInterval);
            }
        }
    }
}