using System;
using UniRx;
using UnityEngine;

namespace TransitionSample
{
    public static class TransitionManager
    {
        /// <summary>
        /// TransitionCotrollerが存在しないなら生成する
        /// </summary>
        private static Lazy<TransitionCotroller> controller = new Lazy<TransitionCotroller>(() =>
        {
            var r = Resources.Load("TransitionCotroller");
            var o = UnityEngine.Object.Instantiate(r) as GameObject;
            UnityEngine.Object.DontDestroyOnLoad(o);
            return o.GetComponent<TransitionCotroller>();
        });

        private static TransitionCotroller Controller => controller.Value;

        /// <summary>
        /// 次のシーンへ遷移する
        /// </summary>
        public static void StartTransition(string nextSceneName)
        {
            Controller.TransitionStart(nextSceneName);
        }

        /// <summary>
        /// シーン遷移完了を通知する
        /// </summary>
        public static IObservable<Unit> OnTransitionFinishedAsync()
        {
            return Controller.OnTransitionFinished;
        }
    }
}