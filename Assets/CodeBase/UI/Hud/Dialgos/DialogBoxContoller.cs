using PixelCrew.Model.Data;
using PixelCrew.Utils;
using PixelCrew.GameObjects;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PixelCrew.Model.Definitions.Localization;

namespace PixelCrew.UI.Hud
{
    public class DialogBoxContoller : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space]
        [SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")]
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private bool _isShowing = false;
        private DialogData _dialogData;
        private int _currentSenteceIndex;
        private AudioSource _sfxSource;
        private Coroutine _typingCoroutine;

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }

        public void ShowDialog(DialogData dialog)
        {
            if (_isShowing) return;

            _isShowing = true;
            _dialogData = dialog;
            _currentSenteceIndex = 0;
            _text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetKeyVal(AnimationKeys.UI.DialogBox.IsOpen, true);
        }

        public void OnSkip()
        {
            if (_typingCoroutine == null) return;

            StopTypeAnimation();
            _text.text = GetText();
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sentence = GetText();
            foreach (var letter in sentence)
            {
                _text.text += letter;
                if (letter != ' ') _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }
        }

        private string GetText()
        {
            var key = _dialogData.Sentences[_currentSenteceIndex];
            return LocalizationManager.I.Localize(key);
        }

        private void StopTypeAnimation()
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }

            _typingCoroutine = null;
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSenteceIndex++;

            var isDialogCompleted = _currentSenteceIndex >= _dialogData.Sentences.Length;
            if (isDialogCompleted)
            {
                HideDialogBox();
            }
            else
            {
                OnStartDialogAnimation();
            }
        }
        private void HideDialogBox()
        {
            _animator.SetKeyVal(AnimationKeys.UI.DialogBox.IsOpen, false);
            _sfxSource.PlayOneShot(_close);
        }

        private void OnStartDialogAnimation()
        {
            _typingCoroutine = StartCoroutine(TypeDialogText());
        }

        private void OnCloseDialogAnimation()
        {
            _isShowing = false;

            if (_dialogData != null)
            {
                _dialogData.AfterDialogEvent?.Invoke();
            }
        }
    }
}
