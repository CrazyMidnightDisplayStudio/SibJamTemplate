using R3;
using System;
using System.Collections.Generic;
using Core.Services.EventBus;
using Game.Services.Events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Core.Audio;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public class ClickObjectForAction: MonoBehaviour
    {
        [SerializeField]public Button _btnPref;
        [SerializeField]public GameObject _menuPref;
        private GameObject _menu = null;
        private VerticalLayoutGroup _verticalLayoutGroup;
        private List<Button> buttons = new List<Button>();
        
        [SerializeField] WaypointCreator creatorPoint;
        private AudioService _audioService;
        [SerializeField] List<String> _audioClipNameList = new();
        [Inject]
        public void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }
        private void Update()
        {
            // Проверяем, было ли нажатие левой кнопки мыши
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                   Vector3 mouseScreenPosition = Input.mousePosition;
                   mouseScreenPosition.z = Camera.main.nearClipPlane + 2;
                   Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                   InitMenu();
                   ClearBtn();
                   var listActionObject = GetGameObjectsPoint(mouseWorldPosition);
                   if (listActionObject.Count == 0)
                   {
                        var pointTarget = creatorPoint.CreatePoint(mouseWorldPosition);
                        listActionObject = BaseCommandList(pointTarget);
                   }
                    if (listActionObject.Count > 0)
                    {
                        CreateBtnAndSubs(listActionObject);
                        BindMenu(mouseWorldPosition);
                    }
                }
            }
            if(Input.GetMouseButtonUp(1))
            {
                ClearBtn();
                if(_menu != null)_menu.gameObject.SetActive(false);
            }
        }
        
        //В случае отсуствия меню создаем его
        private void InitMenu()
        {
            if (_menu == null)
            {
                _menu = Instantiate(_menuPref);
                _verticalLayoutGroup = _menu.GetComponentInChildren<VerticalLayoutGroup>();
            }
        }
        //Заполняем меню кнопками и назначает позицию отобращения
        private void BindMenu(Vector3 position)
        {
            _menu.transform.position = position;
            foreach (Button button in buttons)
            {
                button.transform.SetParent(_verticalLayoutGroup.transform, false);
            }
            _menu.gameObject.SetActive(true);
        }
        
        //получаем список объектов на с которыми можно взаимодействовать
        public List<IInteractAction> GetGameObjectsPoint(Vector3 point)
        {
            List<IInteractAction> gameObjects = new List<IInteractAction>();
        
            // Получаем все объекты, пересекаемые лучом
            RaycastHit2D[] hits = Physics2D.RaycastAll(point, Vector2.zero);
        
            foreach (RaycastHit2D hit in hits)
            {
                // Получаем объект, по которому был произведен клик
                GameObject clickedObject = hit.collider.gameObject;
                var interact = clickedObject.GetComponent<IInteractAction>();
                if (interact != null) gameObjects.Add(interact);
            }
        
            return gameObjects;
        }
        
        private void CreateBtnAndSubs(List<IInteractAction> interactAcnions)
        {
            foreach (var interact in interactAcnions)
            {
                var dictionaryActions = interact.GetActions();
                foreach (var action in dictionaryActions)
                {
                    var btn = SubscribeBtn(CreateBtn(action.Key), action.Value);
                    buttons.Add(btn);
                }
            }
        }
        //Создаем подписку кнопки на переданный action, после нажатия на кнопку меню исчезает
        private Button SubscribeBtn(Button btn, Action action)
        {
            btn.OnClickAsObservable().Subscribe
                (
                    _ => {
                        action.Invoke();
                        CMDEventBus.Publish(new CurrentEvent(action.Method.Name));
                        _menu.gameObject.SetActive(false);
                        var range = UnityEngine.Random.Range(0, _audioClipNameList.Count);
                        _audioService.PlaySfx(_audioClipNameList[range]);
                    }
                ).AddTo(btn.gameObject);
            return btn;
        }
        //Создаем из префаба кнопку и записываем текст дейстьвия
        private Button CreateBtn(string text)
        {
            var btn = Instantiate(_btnPref);
            var tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;
            return btn;
        }
        //Удаляем кнопки после использования
        private void ClearBtn()
        {
            foreach (var item in buttons)
            {
                Destroy(item.transform.gameObject);
            }
            buttons.Clear();
        }
        
        private List<IInteractAction> BaseCommandList(Transform point)
        {
            IInteractAction actions = new ActionHumanMove(creatorPoint.GetHumanController(), point);
        
            return new() { actions };
        }
    }
}
