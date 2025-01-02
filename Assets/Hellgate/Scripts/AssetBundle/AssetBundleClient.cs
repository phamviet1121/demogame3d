﻿//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
//                  Hellgate Framework
// Copyright © Uniqtem Co., Ltd.
//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif

/// <summary>
/// Framework namespace. using Hellgate;
/// </summary>
namespace Hellgate
{
    public class AssetBundleClient
    {
#region AssetBundleRef

        private class AssetBundleRef
        {
            public AssetBundle assetBundle = null;
            public string url;
            public int version;

            public AssetBundleRef (string u, int v)
            {
                url = u;
                version = v;
            }
        }

#endregion

#region Delegate

        public delegate void FinishedDelegate (object obj);

#endregion

#region Static

        private static Dictionary<string, AssetBundleRef> dictionaryAssetBundleRef;

#endregion

        private float progress;

        /// <summary>
        /// Gets the progress. Assetbundle download progress.
        /// 0 to 1.
        /// </summary>
        /// <value>The progress.</value>
        public float Progress {
            get {
                return progress;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hellgate.AssetBundleClient"/> class.
        /// </summary>
        public AssetBundleClient ()
        {
            dictionaryAssetBundleRef = new Dictionary<string, AssetBundleRef> ();
        }

        /// <summary>
        /// Gets the asset bundle.
        /// </summary>
        /// <returns>The asset bundle.</returns>
        /// <param name="url">URL.</param>
        /// <param name="version">Version.</param>
        public AssetBundle GetAssetBundle (string url, int version)
        {
            string keyName = url + version.ToString ();
            AssetBundleRef assetBundleRef;
            if (dictionaryAssetBundleRef.TryGetValue (keyName, out assetBundleRef)) {
                return assetBundleRef.assetBundle;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Downloads the asset bundle.
        /// </summary>
        /// <returns>The asset bundle.</returns>
        /// <param name="url">URL.</param>
        /// <param name="version">Version.</param>
        /// <param name="finished">Finished.</param>
        public IEnumerator DownloadAssetBundle (string url, int version, FinishedDelegate finished)
        {
            string keyName = url + version.ToString ();
            if (dictionaryAssetBundleRef.ContainsKey (keyName)) {
                if (finished != null) {
                    finished (null);
                }

                yield return null;
            } else {
                while (!Caching.ready) {
                    yield return null;
                }

#if UNITY_5_4_OR_NEWER
                uint v = Convert.ToUInt32 (version);
                using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle (url, v, 0)) {
                    yield return www.Send ();

                    while (!www.isDone) {
                        progress = www.downloadProgress;
                        yield return null;
                    }

                    if (!www.isNetworkError) {
                        AssetBundleRef assetBundleRef = new AssetBundleRef (url, version);
                        assetBundleRef.assetBundle = DownloadHandlerAssetBundle.GetContent (www);
                        dictionaryAssetBundleRef.Add (keyName, assetBundleRef);

                        yield return null;
                    }

                    if (finished != null) {
                        finished (www);
                    }

                    progress = 1f;
                }
#else
                using (WWW www = WWW.LoadFromCacheOrDownload (url, version)) {
                    while (!www.isDone) {
                        progress = www.progress;
                        yield return null;
                    }

                    if (www.error == null) {
                        AssetBundleRef assetBundleRef = new AssetBundleRef (url, version);
                        assetBundleRef.assetBundle = www.assetBundle;
                        dictionaryAssetBundleRef.Add (keyName, assetBundleRef);

                        yield return null;
                    }

                    if (finished != null) {
                        finished (www);
                    }

                    progress = 1f;
                    www.Dispose ();
                }
#endif
            }
        }

        /// <summary>
        /// Unload the specified url, version and unloadAllLoadedObjects.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="version">Version.</param>
        /// <param name="unloadAllLoadedObjects">If set to <c>true</c> unload all loaded objects.</param>
        public void Unload (string url, int version, bool unloadAllLoadedObjects)
        {
            string keyName = url + version.ToString ();
            AssetBundleRef assetBundleRef;
            if (dictionaryAssetBundleRef.TryGetValue (keyName, out assetBundleRef)) {
                assetBundleRef.assetBundle.Unload (unloadAllLoadedObjects);
                assetBundleRef.assetBundle = null;
                dictionaryAssetBundleRef.Remove (keyName);
            }
        }

        /// <summary>
        /// Alls the unload.
        /// </summary>
        /// <param name="unloadAllLoadedObjects">If set to <c>true</c> unload all loaded objects.</param>
        public void AllUnload (bool unloadAllLoadedObjects)
        {
            foreach (KeyValuePair<string, AssetBundleRef> kVP in dictionaryAssetBundleRef) {
                kVP.Value.assetBundle.Unload (unloadAllLoadedObjects);
                kVP.Value.assetBundle = null;
            }

            dictionaryAssetBundleRef = new Dictionary<string, AssetBundleRef> ();
        }
    }
}
