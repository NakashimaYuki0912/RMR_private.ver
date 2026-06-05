// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.LogAssetBundleManager
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using System;
using System.Collections.Generic;
using UnityEngine;


namespace abcdcode_LOGLIKE_MOD
{

    public class LogAssetBundleManager : Singleton<LogAssetBundleManager>
    {
        public List<AssetBundle> bundles;
        public List<LogAssetBundleManager.GameObjectBundleCache> GObjList;

        public LogAssetBundleManager()
        {
            this.bundles = new List<AssetBundle>();
            this.GObjList = new List<LogAssetBundleManager.GameObjectBundleCache>();
        }

        public void SetBundles(List<AssetBundle> bundlelist) => this.bundles = bundlelist;

        public void AddBundle(AssetBundle bundle) => this.bundles.Add(bundle);

        public void AddBundle(List<AssetBundle> bundlelist)
        {
            this.bundles.AddRange(bundlelist);
        }

        public GameObject LoadEffectEachScale(
          Transform parent,
          Vector3 scale,
          Vector3 position,
          string name,
          string bundlename = "")
        {
            GameObject asset = Singleton<LogAssetBundleManager>.Instance.GetAsset(name, bundlename);
            asset.LocalEachScalingAll(scale.x, scale.y, scale.z);
            asset.transform.parent = parent;
            asset.SetLayerAll("Effect");
            asset.transform.localPosition = position;
            asset.SetActive(true);
            return asset;
        }

        public GameObject LoadEffect(
          Transform parent,
          Vector3 scale,
          Vector3 position,
          string name,
          string bundlename = "")
        {
            GameObject asset = Singleton<LogAssetBundleManager>.Instance.GetAsset(name, bundlename);
            asset.LocalScalingAll(scale.x, scale.y, scale.z);
            asset.transform.parent = parent;
            asset.SetLayerAll("Effect");
            asset.transform.localPosition = position;
            asset.SetActive(true);
            return asset;
        }

        public GameObject GetAsset(string name, string bundlename = "")
        {
            LogAssetBundleManager.GameObjectBundleCache objectBundleCache = this.GObjList.Find(x => x.objname == name && (bundlename == "" || x.BundleName == bundlename));
            if (objectBundleCache != null)
                return UnityEngine.Object.Instantiate<GameObject>(objectBundleCache.obj);
            foreach (AssetBundle bundle in this.bundles)
            {
                GameObject original = bundle.LoadAsset<GameObject>(name);
                if (original != null)
                {
                    this.GObjList.Add(new LogAssetBundleManager.GameObjectBundleCache()
                    {
                        BundleName = bundle.name,
                        objname = original.name,
                        obj = original
                    });
                    return UnityEngine.Object.Instantiate<GameObject>(original);
                }
            }
            return (GameObject)null;
        }

        public class GameObjectBundleCache
        {
            public string objname;
            public string BundleName;
            public GameObject obj;
        }
    }
}
