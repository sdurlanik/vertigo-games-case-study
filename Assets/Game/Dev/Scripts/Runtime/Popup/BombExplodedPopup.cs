using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Core.Events;

namespace VertigoGamesCaseStudy.Runtime.UI.Popup
{
    public class BombExplodedPopup : PopupBase
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform topPanel;
        [SerializeField] private RectTransform cardPanel;
        [SerializeField] private RectTransform buttonPanel;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButton;

        [Header("Animation Settings")]
        [SerializeField] private float fadeDuration = .2f;
        [SerializeField] private float topPanelAnimationDuration = .4f;
        [SerializeField] private float cardPanelAnimationDuration = .6f;
        [SerializeField] private float buttonPanelAnimationDuration = .5f;
        protected override void OnEnable()
        {
            base.OnEnable();
            giveUpButton.onClick.AddListener(OnGiveUpButtonClicked);
            reviveButton.onClick.AddListener(OnReviveButtonClicked);
        }

        private void OnDisable()
        {
            giveUpButton.onClick.RemoveListener(OnGiveUpButtonClicked);
            reviveButton.onClick.RemoveListener(OnReviveButtonClicked);
        }

        private void OnValidate()
        {
            if (giveUpButton == null)
            {
                giveUpButton = transform.Find("card_panel_button/card_popup_button_giveup").GetComponent<Button>();
            }

            if (reviveButton == null)
            {
                reviveButton = transform.Find("card_panel_button/card_popup_button_revive").GetComponent<Button>();
            }
        }

        protected override void Enter()
        {
            base.Enter();
            AnimatePopupEnter();
        }
        
        protected override void Exit()
        {
            base.Exit();
            AnimatePopupExit();
        }

        private void AnimatePopupEnter()
        {
            var topPanelOriginalPosition = topPanel.anchoredPosition;
            var cardPanelOriginalPosition = cardPanel.anchoredPosition;

            var parentRectTransform = topPanel.parent as RectTransform;

            var offScreenTopY = parentRectTransform.rect.height / 2 + topPanel.rect.height / 2;
            var offScreenBottomY = - (parentRectTransform.rect.height / 2 + cardPanel.rect.height / 2);

            topPanel.anchoredPosition = new Vector2(topPanelOriginalPosition.x, offScreenTopY);
            cardPanel.anchoredPosition = new Vector2(cardPanelOriginalPosition.x, offScreenBottomY);

            buttonPanel.localScale = Vector3.zero;

            var popupSequence = DOTween.Sequence();

            popupSequence.Append(canvasGroup.DOFade(1, fadeDuration).From(0));
            popupSequence.Append(topPanel.DOAnchorPos(topPanelOriginalPosition, topPanelAnimationDuration).SetEase(Ease.OutCubic));
            popupSequence.Join(cardPanel.DOAnchorPos(cardPanelOriginalPosition, cardPanelAnimationDuration).SetEase(Ease.OutCubic));
            popupSequence.Append(buttonPanel.DOScale(Vector3.one, buttonPanelAnimationDuration).SetEase(Ease.OutBack));
            
            popupSequence.Play();
        }

        private void AnimatePopupExit()
        {
            canvasGroup.DOFade(0, fadeDuration).OnComplete((() => gameObject.SetActive(false)));
        }

        private void OnGiveUpButtonClicked()
        {
            Exit();
            
            var giveUpEvent = new GiveUpEvent();
            GameEventManager.Instance.Fire(giveUpEvent);
            
        }

        private void OnReviveButtonClicked()
        {
            Exit();
            
            var reviveEvent = new ReviveEvent();
            GameEventManager.Instance.Fire(reviveEvent);
        }
    }
}
