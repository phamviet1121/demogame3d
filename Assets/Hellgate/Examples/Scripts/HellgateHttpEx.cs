//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
//                  Hellgate Framework
// Copyright © Uniqtem Co., Ltd.
//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hellgate;

namespace HellgeteEx
{
    public class HellgateHttpData
    {
        string[] sprite = null;

        public string[] _Sprite {
            get {
                return sprite;
            }
        }
    }

    public class HellgateHttpEx : HellgateSceneControllerEx
    {
        [SerializeField]
        private GameObject title;
        [SerializeField]
        private GameObject sprite;
        [SerializeField]
        private GameObject button;
        private List<Sprite> sprites;
        private string url;

        public static void GoHttp ()
        {
            List<HttpData> https = new List<HttpData> ();
            https.Add (new HttpData ("info", "json"));

            LoadingJobData data = new LoadingJobData ();
            data.https = https;
            data.finishedDelegate = delegate(List<object> obj, LoadingJobController job) {
#if UNITY_5_4_OR_NEWER
                UnityEngine.Networking.UnityWebRequest www = Util.GetListObject<UnityEngine.Networking.UnityWebRequest> (obj);
                HellgateHttpData sprites = JsonUtil.FromJson<HellgateHttpData> (www.downloadHandler.text);
#else
                WWW www = Util.GetListObject<WWW> (obj);
                HellgateHttpData sprites = JsonUtil.FromJson<HellgateHttpData> (www.text);
#endif

                List<AssetBundleData> assetbundles = new List<AssetBundleData> ();
                for (int i = 0; i < sprites._Sprite.Length; i++) {
                    assetbundles.Add (new AssetBundleData ("hellgatehttp", sprites._Sprite [i], typeof(Sprite)));
                }

                job.nextSceneName = "HellgateHttp";
                job.LoadAssetBundle (assetbundles);
            };

            data.lEvent = delegate(LoadingJobStatus status, LoadingJobController job) {
                if (status == LoadingJobStatus.HttpTimeover) { // Time over.
                    SceneManager.Instance.Close ();
                    SceneManager.Instance.PopUp ("Replay ?", PopUpType.YesAndNo, delegate(PopUpYNType type) {
                        if (type == PopUpYNType.Yes) {
                            SceneManager.Instance.LoadingJob (data);
                        }
                    });
                } else if (status == LoadingJobStatus.HttpError) {
                    SceneManager.Instance.Close ();
                    SceneManager.Instance.PopUp ("Network Error.", PopUpType.Ok, delegate(PopUpYNType type) {
                        SceneManager.Instance.Reboot ();
                    });
                }
            };

            data.PutExtra ("title", "Http");
            SceneManager.Instance.LoadingJob (data);
        }

        public override void OnSet (object data)
        {
            base.OnSet (data);

            List<object> objs = data as List<object>;
            Dictionary<string, object> intent = Util.GetListObject<Dictionary<string, object>> (objs);
            sprites = Util.GetListObjects<Sprite> (objs);

            SetLabelTextValue (title, intent ["title"].ToString ());

            url = "";

            StartCoroutine (ViewAsset ());
            button.SetActive (false);
        }

        public override void OnKeyBack ()
        {
            base.Quit ("Exit ?");
        }

        private IEnumerator ViewAsset ()
        {
            System.Random ran = new System.Random ();
            int index = ran.Next (0, sprites.Count);
            Set2DSpriteValue (sprite, sprites [index]);

            yield return new WaitForSeconds (0.5f);
            StartCoroutine (ViewAsset ());
        }

        public void OnClickRequest ()
        {
            HttpData http = new HttpData ("test", "json");

#if UNITY_5_4_OR_NEWER
            http.finishedDelegate = delegate(UnityEngine.Networking.UnityWebRequest www) {
                if (www == null) { // time over
                } else if (www.isNetworkError) { // error
                } else {
                    HellgateHttpDataEx data = JsonUtil.FromJson<HellgateHttpDataEx> (www.downloadHandler.text);
                    url = data.Url;

                    button.SetActive (true);
                }
            };
#else
            http.finishedDelegate = delegate (WWW www) {
                if (www == null) { // time over
                } else if (www.error != null) { // error
                } else {
                    HellgateHttpDataEx data = JsonUtil.FromJson<HellgateHttpDataEx> (www.text);
                    url = data.Url;

                    button.SetActive (true);
                }
            };
#endif

            HttpManager.Instance.Get (http);
        }

        public void OnClickOpenURL ()
        {
            if (url == "") {
                return;
            }

            Application.OpenURL (url);
        }
    }
}
