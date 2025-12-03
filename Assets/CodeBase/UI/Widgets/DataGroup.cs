using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class PedefinedDataGroup<TDataType, TItemType> : DataGroup<TDataType, TItemType>
        where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        public PedefinedDataGroup(Transform container) : base(null, container)
        {
            var items = container.GetComponentsInChildren<TItemType>();
            CreatedItems.AddRange(items);
        }

        public override void SetData(IList<TDataType> data)
        {
            if (data.Count > CreatedItems.Count)
                throw new IndexOutOfRangeException();

            base.SetData(data);
        }
    }


    public class DataGroup<TDataType, TItemType> where TItemType : MonoBehaviour, IItemRenderer<TDataType>
    {
        protected readonly List<TItemType> CreatedItems = new List<TItemType>();
        private readonly TItemType _prefab;
        private readonly Transform _container;

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TDataType> data)
        {
            // create required items
            for (var i = CreatedItems.Count; i < data.Count(); i++)
            {
                var item = UnityEngine.Object.Instantiate(_prefab, _container);
                CreatedItems.Add(item);
            }

            // update data and activate
            for (var i = 0; i < data.Count; i++)
            {
                CreatedItems[i].SetData(data[i], i);
                CreatedItems[i].gameObject.SetActive(true);
            }

            // hide unused items
            for (var i = data.Count; i < CreatedItems.Count; i++)
            {
                CreatedItems[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemRenderer<in TDataType>
    {
        void SetData(TDataType data, int index);
    }
}