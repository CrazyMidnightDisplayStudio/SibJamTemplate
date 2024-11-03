using System.Collections.ObjectModel;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
using System;
using System.Collections.Generic;
namespace Assets.Scripts.Gameplay.ActionMenu
{
    public class BinderActionMenu : MonoBehaviour
    {
        [SerializeField] private Button _tmpNamePref;
        [SerializeField] private VerticalLayoutGroup  _container;

        public void BindTwo()
        {
            //foreach (var data in list)
            //{
            //    var view = Instantiate(_tmpNamePref);
            //    var tmp = view.GetComponentInChildren<TextMeshProUGUI>();
            //    tmp.text = data.name;
            //    var typeClass = data.first;
            //    var meth = data.second;
            //    Type type = data.type;
            //    view.OnClickAsObservable()
            //        .Subscribe(
            //        _ => {
            //            meth.Invoke(typeClass, null);
            //            Debug.Log(data.name);
            //            Debug.Log(meth.Attributes.ToString());
            //        }
            //            ).AddTo(this);
            //    view.transform.SetParent(_container.transform, false);
            //}
        }
    }
}
