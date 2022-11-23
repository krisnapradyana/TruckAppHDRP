using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainControl
{
    public class UIControl : MonoBehaviour
    {
        [Header("UI Attributes")]
        [SerializeField] Vector2 _windowTargetScale;
        [SerializeField] Vector2 _lineTargetPosition;

        [SerializeField] RectTransform _tutorialUI;
        [SerializeField] RectTransform _underlineBorder;
        [SerializeField] RectTransform _descriptionWindow;
        [SerializeField] TMP_Text _titleText;
        [SerializeField] TMP_Text _descriptionText;

        [SerializeField] RectTransform _showPos;
        [SerializeField] RectTransform _hidePos;
        [SerializeField] RectTransform _tutorialShowPos;
        [SerializeField] RectTransform _tutorialHidePos;
        [SerializeField] RectTransform _360UIPos;

        [Header("Extra UI Attributes")]
        [SerializeField] RectTransform _360UI;

        public Button _backButton;
        public Button _exitButton;
        public TMP_Text _inspectText;

        //Tween container
        private Tween _tweenAnimation0;

        public IEnumerator ShowTutorialWithTimer()
        {
            yield return null;
            var tween = _tutorialUI.transform.DOMove(_tutorialShowPos.position, 1f);
            yield return tween.WaitForCompletion();
            yield return new WaitForSeconds(4f);
            _tutorialUI.transform.DOMove(_tutorialHidePos.position, 1f);
        }

        public void SetTexts(string targetTitle, string targetDescription)
        {
            _descriptionText.text = targetDescription;
            _titleText.text = targetTitle;
        }

        public void ToggleShowWindow(bool state)
        {
            //play scale window
            switch (state)
            {
                case true:
                    _tweenAnimation0?.Kill();
                    _tweenAnimation0 = _descriptionWindow.DOMove(_showPos.position, 1f);
                    _descriptionWindow.GetComponent<CanvasGroup>().DOFade(1f, .3f).SetDelay(.3f);
                    break;
                case false:
                    _tweenAnimation0?.Kill();
                    _tweenAnimation0 = _descriptionWindow.DOMove(_hidePos.position, 1f);
                    _descriptionWindow.GetComponent<CanvasGroup>().DOFade(0f, .3f);
                    break;
            }
        }

        public void Show360UI()
        {
            _360UI.DOMoveY(_360UIPos.position.y, .75f);
        }

        public void ToggleBack(bool state)
        {
            _backButton.gameObject.SetActive(state);
        }
    }
}
