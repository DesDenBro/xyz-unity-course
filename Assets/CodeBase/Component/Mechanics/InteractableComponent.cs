using PixelCrew.Common;
using PixelCrew.Common.Tech;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderSource;
        [SerializeField] private OutlineCollection _outlineCollection;
        [SerializeField] private bool _isActive = true;
        [SerializeField] private bool _checksToActivate;
        [SerializeField] private UnityEventGameObject[] _onChecksFailActions;
        [SerializeField] private bool _isInInteract;
        [SerializeField] private InteractableMode _mode = InteractableMode.Click;
        [SerializeField] private InteractableIteration _iterations = InteractableIteration.Once;
        [SerializeField] private UnityEventGameObject[] _beforeActions;
        [SerializeField] private UnityEventGameObject[] _actions;
        [SerializeField] private UnityEventGameObject[] _afterActions;

        private OutlineSettings _outline;
        private ThingSpecification _thingSpecification;
        private GameObject _lastActivator;
        private SpriteRenderer _innerSpriteRenderer;
        private bool _interactionDone = false;
        private bool _triggerAfterActions = false;
        private bool _isInHighlight;


        public bool InteractIsPossible => _iterations == InteractableIteration.Multi || !_interactionDone;
        private bool _IsOutlineEnabled
        {
            get 
            {
                if (_outlineCollection != null) return _outlineCollection.IsEnabled;
                if (_outline != null) return _outline.IsEnabled;
                return false;
            }
        }


        private void OutlineEnable() 
        {
            if (_outlineCollection != null) _outlineCollection.Enable(); 
            else if (_outline != null)
            {
                _innerSpriteRenderer.sprite = _renderSource.sprite;
                _outline.Enable(); 
            } 
        }
        private void OutlineDisable() 
        {
            if (_outlineCollection != null) _outlineCollection.Disable(); 
            else if (_outline != null) 
            {
                _innerSpriteRenderer.sprite = Sprite.Create(null, new Rect(0, 0, 0, 0), Vector2.zero);
                _outline.Disable(); 
            }
        }


        private void Start()
        {
            _innerSpriteRenderer = GetComponent<SpriteRenderer>();
            _thingSpecification = GetComponent<ThingSpecification>();
            _outline = GetComponent<OutlineSettings>();

            OutlineDisable();
        }

        private void FixedUpdate()
        {
            if (_isInHighlight && _outlineCollection == null)
            {
                _innerSpriteRenderer.sprite = _renderSource.sprite;
            }

            if (!_isInHighlight)
            {
                if (_IsOutlineEnabled) OutlineDisable();
                if (_isInInteract) _isInInteract = false;
            }
            _isInHighlight = false;

            if (_isActive && _isInInteract)
            {
                foreach (var action in _actions)
                {
                    action?.Invoke(_lastActivator);
                }
                _triggerAfterActions = true;
            }

            if (!_isInInteract && _triggerAfterActions)
            {
                _triggerAfterActions = false;
                foreach (var afterAction in _afterActions)
                {
                    afterAction?.Invoke(_lastActivator);
                }
                _lastActivator = null;
                _interactionDone = true;
            }
        }

        public void Highlight()
        {
            if (!InteractIsPossible) return;

            _isInHighlight = true;
            if (!_isInInteract && !_IsOutlineEnabled)
            {
                OutlineEnable();
            }
            if (_isInInteract)
            {
                OutlineDisable();
            }
        }

        public void Interact(GameObject activator, bool isPressed)
        {
            if (!InteractIsPossible) return;

            if (_lastActivator != null && _lastActivator != activator) return;
            if (_lastActivator == null) _lastActivator = activator;

            CheckActive();

            switch (_mode)
            {
                case InteractableMode.Click:
                    ClickInteraction(activator, isPressed); break;
                case InteractableMode.Hold:
                    HoldInteraction(activator, isPressed); break;
                case InteractableMode.OnOffClick:
                    OnOffClickInteraction(activator, isPressed); break;
            }
        }
        private void ClickInteraction(GameObject activator, bool isPressed)
        {
            if (!_isActive) return;

            if (_isInInteract || (!_isInInteract && isPressed)) return;
            if (!_isInInteract && !isPressed) _isInInteract = true;

            if (_isActive && _isInInteract)
            {
                foreach (var beforeAction in _beforeActions)
                {
                    beforeAction?.Invoke(_lastActivator);
                }
                foreach (var action in _actions)
                {
                    action?.Invoke(_lastActivator);
                }
                _triggerAfterActions = true;
                _isInInteract = false;
            }
        }
        private void HoldInteraction(GameObject activator, bool isPressed)
        {
            if (!_isActive) return;

            if (!_isInInteract && isPressed)
            {
                foreach (var beforeAction in _beforeActions)
                {
                    beforeAction?.Invoke(_lastActivator);
                }
                _isInInteract = true;
                _triggerAfterActions = true;
            }
            if (_isInInteract && !isPressed) _isInInteract = false;
        }

        private void OnOffClickInteraction(GameObject activator, bool isPressed)
        {
            if (!_isActive) return;

            if (!_isInInteract && isPressed)
            {
                foreach (var beforeAction in _beforeActions)
                {
                    beforeAction?.Invoke(_lastActivator);
                }
                _isInInteract = true;
                _triggerAfterActions = true;
            } else if (_isInInteract && isPressed)
            {
                _isInInteract = false;
            }
        }

        private void CheckActive()
        {
            if (_isActive || !_checksToActivate || _lastActivator == null) return;

            var checksDone = true;
            var inventoryComp = _lastActivator.GetComponent<InventoryComponent>();
            if (inventoryComp == null || _thingSpecification == null) return;

            checksDone = checksDone && inventoryComp.CheckItemCountToEvent(InventoryItemName.Key, _thingSpecification.KeysAmount);
            checksDone = checksDone && inventoryComp.CheckItemCountToEvent(InventoryItemName.Money, _thingSpecification.CostAmount);

            if (checksDone)
            {
                _isActive = inventoryComp.ChangeInventoryItemCount(InventoryItemName.Key, _thingSpecification.KeysAmount) && inventoryComp.ChangeInventoryItemCount(InventoryItemName.Money, _thingSpecification.CostAmount);
            }
            else
            {
                foreach (var onChecksFailAction in _onChecksFailActions)
                {
                    onChecksFailAction?.Invoke(_lastActivator);
                }
            }
        }
    }

    public enum InteractableIteration : byte
    {
        Once = 0,
        Multi = 1
    }
    public enum InteractableMode : byte
    {
        Click = 0,
        Hold = 1,
        OnOffClick = 2 // как Hold, только держать все время не надо кнопку
    }
}
