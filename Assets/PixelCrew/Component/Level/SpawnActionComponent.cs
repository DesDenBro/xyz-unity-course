using PixelCrew.Common.Tech;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnActionComponent : MonoBehaviour
    {
        [SerializeField] private SpawnComponent _spawner;

        public void SpawnAction(string sasName)
        {
            var obj = _spawner.Spawn();

            var saComp = obj?.GetComponent<SpriteAnimation>();
            if (saComp == null) return;

            obj.name = obj.name + "_spawn_" + sasName;
            saComp.SetStartSasName(sasName);

            // добавляем вектор отклонения относительно нахождения стартовой точки спавна из sas
            var sasTransform = saComp.GetSasTransform(sasName);
            if (sasTransform != null)
            {
                obj.transform.position += new Vector3(
                    sasTransform.localPosition.x * sasTransform.parent.lossyScale.x,
                    sasTransform.localPosition.y * sasTransform.parent.lossyScale.y,
                    sasTransform.localPosition.z * sasTransform.parent.lossyScale.z
                );
            }
        }
    }
}
