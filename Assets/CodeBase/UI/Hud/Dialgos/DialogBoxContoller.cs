using PixelCrew.Model.Data;
using PixelCrew.Utils;
using PixelCrew.GameObjects;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

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
            _dialogData = dialog;
            _currentSenteceIndex = 0;
            _text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetKeyVal(AnimationKeys.UI.DialogBox.IsOpen, true);
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sentence = _dialogData.Sentences[_currentSenteceIndex];
            foreach (var letter in sentence)
            {
                _text.text += letter;
                if (letter != ' ') _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }
        }
        public void OnSkip()
        {
            if (_typingCoroutine == null) return;

            StopTypeAnimation();
            _text.text = _dialogData.Sentences[_currentSenteceIndex];
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

        }


        [SerializeField] private DialogData _testData;
        public void Test()
        {
            ShowDialog(_testData);
        }
    }
}
