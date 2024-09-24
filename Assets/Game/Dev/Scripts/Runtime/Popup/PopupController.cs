using UnityEngine;
using VertigoGamesCaseStudy.Core.Events;
using VertigoGamesCaseStudy.Runtime.UI;

namespace VertigoGamesCaseStudy.Runtime.UI.Popup
{
    public class PopupController : MonoBehaviour
    {
        //class isimlendirmesini tekrar degerlendir
        [SerializeField] private RewardPopup rewardPopup;
        [SerializeField] private BombExplodedPopup bombExplodedPopup;
        [SerializeField] private LeaveGamePopup leaveGamePopup;

        private void OnEnable()
        {
            GameEventManager.Instance.On<BombExplodedEvent>(OnBombExploded);
            GameEventManager.Instance.On<RouletteSpinEndEvent>(OnRouletteSpinEnd);
            GameEventManager.Instance.On<LeaveGameEvent>(OnLeaveGame);
        }
        private void OnDisable()
        {
            GameEventManager.Instance.Off<BombExplodedEvent>(OnBombExploded);
            GameEventManager.Instance.Off<RouletteSpinEndEvent>(OnRouletteSpinEnd);
            GameEventManager.Instance.Off<LeaveGameEvent>(OnLeaveGame);
        }

        

        private void OnBombExploded(BombExplodedEvent bombExplodedEvent)
        {
            bombExplodedPopup.gameObject.SetActive(true);
        }
        
        private void OnRouletteSpinEnd(RouletteSpinEndEvent rouletteSpinEndEvent)
        {
            rewardPopup.SetReward(rouletteSpinEndEvent.SlotItem, rouletteSpinEndEvent.RewardMultiplier);
            rewardPopup.gameObject.SetActive(true);
        }
        
        private void OnLeaveGame(LeaveGameEvent leaveGameEvent)
        {
            leaveGamePopup.gameObject.SetActive(true);
        }
    }
}
