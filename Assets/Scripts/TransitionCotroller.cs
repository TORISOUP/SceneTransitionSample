using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TransitionSample
{
    public class TransitionCotroller : MonoBehaviour
    {
        [SerializeField] private Image coverImage;
        [SerializeField] private float transitonSeconds;

        private readonly BoolReactiveProperty isTransferring = new BoolReactiveProperty(false);

        public IObservable<Unit> OnTransitionFinished
        {
            get
            {
                // シーン遷移をしていないなら、即イベント発行
                if (!isTransferring.Value) return Observable.Return(Unit.Default);

                // シーン遷移中なら終わったときにイベント発行
                return isTransferring.FirstOrDefault(x => !x).AsUnitObservable();
            }
        }

        /// <summary>
        /// シーン遷移を開始する
        /// </summary>
        public void TransitionStart(string nextSceneName)
        {
            if (isTransferring.Value) return; //すでにシーン遷移中なら何もしない
            isTransferring.Value = true;
            StartCoroutine(TransitionCoroutine(nextSceneName));
        }

        private IEnumerator TransitionCoroutine(string nextSceneName)
        {
            var time = transitonSeconds;

            // 画面のクリックイベントをTransitionCanvasでブロックする
            coverImage.raycastTarget = true;

            // 画面を徐々に白くする
            while (time > 0)
            {
                time -= Time.deltaTime;
                coverImage.color = OverrideColorAlpha(coverImage.color, 1.0f - time / transitonSeconds);
                yield return null;
            }

            // 完全に白くする
            coverImage.color = OverrideColorAlpha(coverImage.color, 1.0f);

            // 画面が隠し終わったらシーン遷移する
            yield return SceneManager.LoadSceneAsync(nextSceneName);

            // 画面を徐々に戻す
            time = transitonSeconds;
            while (time > 0)
            {
                time -= Time.deltaTime;
                coverImage.color = OverrideColorAlpha(coverImage.color, time / transitonSeconds);
                yield return null;
            }

            // クリックイベントのブロック解除
            coverImage.raycastTarget = false;
            coverImage.color = OverrideColorAlpha(coverImage.color, 0.0f);

            // シーン遷移完了
            isTransferring.Value = false;
        }

        private Color OverrideColorAlpha(Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
    }
}