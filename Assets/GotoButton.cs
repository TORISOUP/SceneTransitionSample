using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TransitionSample
{
    public class GotoButton : MonoBehaviour
    {
        [SerializeField] private string _nextScene;

        [SerializeField] private Button _button;

        private async void Start()
        {
            // シーン遷移が終わるまで待つ
            await TransitionManager.OnTransitionFinishedAsync();

            _button.OnClickAsObservable()
                .Subscribe(_ => { TransitionManager.StartTransition(_nextScene); });
        }
    }
}