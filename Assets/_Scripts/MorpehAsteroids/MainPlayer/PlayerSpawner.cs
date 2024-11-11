using Cysharp.Threading.Tasks;
using MorpehAsteroids.MainPlayer.Shooting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MorpehAsteroids.MainPlayer
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<AssetReference> _appearanceReferences;
        [SerializeField] private List<AssetReference> _projectileReferences;

        private GameObject _setAppearance;
        private AsyncOperationHandle _loadAppearanceHandle;

        private int _setProjectileIndex;
        private AsyncOperationHandle _providerLoadHandle;

        internal int AvailableProjectileCount => _projectileReferences.Count;

        public async void SetPlayerAppearance(int appearanceIndex, Transform playerProviderTransform) 
        {
            if (appearanceIndex > _appearanceReferences.Count)
                appearanceIndex = _appearanceReferences.Count - 1;

            if (appearanceIndex < 0)
                appearanceIndex = 0;

            _loadAppearanceHandle = 
               Addressables.LoadAssetAsync<GameObject>(_appearanceReferences[appearanceIndex]);

            await UniTask.WaitUntil(() => _loadAppearanceHandle.IsDone == true);

            AsyncOperationHandle instationationHandle = _appearanceReferences[appearanceIndex]
                .InstantiateAsync(playerProviderTransform);

            await UniTask.WaitUntil(() => instationationHandle.IsDone == true);
            _setAppearance = (GameObject)instationationHandle.Result;
        }

        public void Dispose()
        {
            Addressables.ReleaseInstance(_setAppearance);
            Addressables.Release(_loadAppearanceHandle);
        }

        internal void SetTargetProjectileType(int projectileIndex) 
        {
            if (projectileIndex > _projectileReferences.Count)
                projectileIndex = _projectileReferences.Count - 1;

            if (projectileIndex < 0)
                projectileIndex = 0;

            if (_providerLoadHandle.IsValid()) 
                Addressables.Release(_providerLoadHandle);

            _setProjectileIndex = projectileIndex;
            _providerLoadHandle = 
                Addressables.LoadAssetAsync<GameObject>(_projectileReferences[projectileIndex]);
        }

        internal async UniTask<ProjectileProvider> SpawnProjectile(Vector3 worldPosition, Quaternion rotation) 
        {
            if (_providerLoadHandle.IsDone == false)
                await UniTask.WaitUntil(() => _providerLoadHandle.IsDone == true);

            AsyncOperationHandle spawnHandle = _projectileReferences[_setProjectileIndex]
                .InstantiateAsync(worldPosition, rotation);
            await UniTask.WaitUntil(() => spawnHandle.IsDone == true);
            return ((GameObject)spawnHandle.Result).GetComponent<ProjectileProvider>();
        }
    }
}