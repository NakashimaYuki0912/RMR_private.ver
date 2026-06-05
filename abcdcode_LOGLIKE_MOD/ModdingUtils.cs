// Decompiled with JetBrains decompiler
// Type: abcdcode_LOGLIKE_MOD.ModdingUtils
// Assembly: abcdcode_LOGLIKE_MOD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BD775C4-C5BF-4699-81F7-FB98B2E922E2
// Assembly location: C:\Users\Usuário\Desktop\Projects\LoR Modding\spaghetti\RogueLike Mod Reborn\dependencies\abcdcode_LOGLIKE_MOD.dll

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;


namespace abcdcode_LOGLIKE_MOD
{

    public class ModdingUtils
    {
        public static List<V> FindAll_DicValueAsKey<K, V>(Dictionary<K, V> dic, Predicate<K> predicate)
        {
            List<K> all = dic.Keys.ToList<K>().FindAll(predicate);
            List<V> allDicValueAsKey = new List<V>();
            foreach (K key in all)
                allDicValueAsKey.Add(dic[key]);
            return allDicValueAsKey;
        }

        public static Texture2D MixTexture(Texture2D bottom, Texture2D top, int startx, int starty)
        {
            RenderTexture dest1 = new RenderTexture(bottom.width, bottom.height, 24);
            RenderTexture.active = dest1;
            Graphics.Blit((Texture)bottom, dest1);
            Texture2D texture2D = new Texture2D(bottom.width, bottom.height);
            texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float)bottom.width, (float)bottom.height), 0, 0);
            RenderTexture dest2 = new RenderTexture(top.width, top.height, 24);
            RenderTexture.active = dest2;
            Graphics.Blit((Texture)top, dest2);
            texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float)top.width, (float)top.height), startx, starty);
            texture2D.Apply();
            return texture2D;
        }

        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            RenderTexture dest = new RenderTexture(targetWidth, targetHeight, 24);
            RenderTexture.active = dest;
            Graphics.Blit((Texture)source, dest);
            Texture2D texture2D = new Texture2D(targetWidth, targetHeight);
            texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float)targetWidth, (float)targetHeight), 0, 0);
            texture2D.Apply();
            return texture2D;
        }

        public static void SuffleList<T>(List<T> value)
        {
            int count = value.Count;
            while (count > 1)
            {
                --count;
                int index = UnityEngine.Random.Range(0, count + 1);
                T obj = value[index];
                value[index] = value[count];
                value[count] = obj;
            }
        }

        public static T GetFieldValue<T>(FieldInfo info, object instance) => (T)info.GetValue(instance);

        public static T GetFieldValue<T>(string info, object instance)
        {
            return ModdingUtils.GetFieldValue<T>(ModdingUtils.GetField(info, instance), instance);
        }

        public static FieldInfo GetField(string name, object instance)
        {
            return instance.GetType().GetField(name, AccessTools.all);
        }

        public static TKey PickoneKeyInDic<TKey, TValue>(Dictionary<TKey, TValue> dic)
        {
            List<KeyValuePair<TKey, TValue>> list = dic.ToList<KeyValuePair<TKey, TValue>>();
            return list[UnityEngine.Random.Range(0, list.Count)].Key;
        }

        public static InputField CreateInputField(
          Transform parent,
          string ImageName,
          Vector2 position,
          TextAnchor tanchor,
          int fsize,
          Color tcolor,
          Font font)
        {
            GameObject gameObject = ModdingUtils.CreateImage(parent, ImageName, new Vector2(1f, 1f), position).gameObject;
            Text text = ModdingUtils.CreateText(gameObject.transform, new Vector2(0.0f, 0.0f), fsize, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), tanchor, tcolor, font);
            text.text = "";
            InputField inputField = gameObject.AddComponent<InputField>();
            inputField.targetGraphic = (Graphic)gameObject.GetComponent<Image>();
            inputField.textComponent = text;
            return inputField;
        }

        public static Button AddButton(Image target)
        {
            Button button = target.gameObject.AddComponent<Button>();
            button.targetGraphic = (Graphic)target;
            return button;
        }

        public static Image CreateImage(
          Transform parent,
          Sprite sprite,
          Vector2 scale,
          Vector2 position)
        {
            return ModdingUtils.CreateImage(parent, sprite, scale, position, new Vector2((float)sprite.texture.width, (float)sprite.texture.height));
        }

        public static Image CreateImage(
          Transform parent,
          string sprite,
          Vector2 scale,
          Vector2 position)
        {
            return ModdingUtils.CreateImage(parent, LogLikeMod.ArtWorks[sprite], scale, position);
        }

        public static Image CreateImage(
          Transform parent,
          Sprite sprite,
          Vector2 scale,
          Vector2 position,
          Vector2 sizeDelta)
        {
            GameObject gameObject = new GameObject("Image");
            Image image = gameObject.AddComponent<Image>();
            image.transform.SetParent(parent);
            image.sprite = sprite;
            image.rectTransform.sizeDelta = sizeDelta;
            gameObject.SetActive(true);
            gameObject.transform.localScale = (Vector3)scale;
            gameObject.transform.localPosition = (Vector3)position;
            return image;
        }

        public static Image CreateImage(
          Transform parent,
          string ImageName,
          Vector2 scale,
          Vector2 position,
          Vector2 sizeDelta)
        {
            return ModdingUtils.CreateImage(parent, LogLikeMod.ArtWorks[ImageName], scale, position, sizeDelta);
        }

        public static Text CreateText(
          Transform target,
          Vector2 position,
          int fsize,
          Vector2 anchormin,
          Vector2 anchormax,
          Vector2 anchorposition,
          TextAnchor anchor,
          Color tcolor,
          Font font)
        {
            GameObject gameObject = new GameObject("Text");
            Text text = gameObject.AddComponent<Text>();
            gameObject.transform.SetParent(target);
            text.rectTransform.sizeDelta = Vector2.zero;
            text.rectTransform.anchorMin = anchormin;
            text.rectTransform.anchorMax = anchormax;
            text.rectTransform.anchoredPosition = anchorposition;
            text.text = " ";
            text.font = font;
            text.fontSize = fsize;
            text.color = tcolor;
            text.alignment = anchor;
            gameObject.transform.localScale = new Vector3(1f, 1f);
            gameObject.transform.localPosition = (Vector3)position;
            gameObject.SetActive(true);
            return text;
        }

        public static TextMeshProUGUI CreateText_TMP(
          Transform target,
          Vector2 position,
          int fsize,
          Vector2 anchormin,
          Vector2 anchormax,
          Vector2 anchorposition,
          TextAlignmentOptions anchor,
          Color tcolor,
          TMP_FontAsset font)
        {
            GameObject gameObject = new GameObject("Text");
            TextMeshProUGUI textTmp = gameObject.AddComponent<TextMeshProUGUI>();
            gameObject.transform.SetParent(target);
            textTmp.rectTransform.sizeDelta = Vector2.zero;
            textTmp.rectTransform.anchorMin = anchormin;
            textTmp.rectTransform.anchorMax = anchormax;
            textTmp.rectTransform.anchoredPosition = anchorposition;
            textTmp.text = " ";
            textTmp.font = font;
            textTmp.fontSize = (float)fsize;
            textTmp.color = tcolor;
            textTmp.alignment = anchor;
            gameObject.transform.localScale = new Vector3(1f, 1f);
            gameObject.transform.localPosition = (Vector3)position;
            gameObject.SetActive(true);
            return textTmp;
        }

        public static Text CreateText(Transform target)
        {
            return ModdingUtils.CreateText(target, new Vector2(0.0f, 0.0f), 10, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), new Vector2(0.0f, 0.0f), TextAnchor.UpperLeft, Color.black, UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf"));
        }

        public static UILogCustomSelectable CreateLogSelectable(
          Transform parent,
          Sprite Image,
          Vector2 scale,
          Vector2 position,
          Vector2 sizedelta)
        {
            Image image = ModdingUtils.CreateImage(parent, Image, scale, position, sizedelta);
            UILogCustomSelectable logSelectable = image.gameObject.AddComponent<UILogCustomSelectable>();
            logSelectable.targetGraphic = (Graphic)image;
            return logSelectable;
        }

        public static UILogCustomSelectable CreateLogSelectable(
          Transform parent,
          string Imagepath,
          Vector2 scale,
          Vector2 position,
          Vector2 sizedelta)
        {
            Image image = ModdingUtils.CreateImage(parent, Imagepath, scale, position, sizedelta);
            UILogCustomSelectable logSelectable = image.gameObject.AddComponent<UILogCustomSelectable>();
            logSelectable.targetGraphic = (Graphic)image;
            return logSelectable;
        }

        public static UILogCustomSelectable CreateLogSelectable(
          Transform parent,
          Sprite Image,
          Vector2 scale,
          Vector2 position)
        {
            Image image = ModdingUtils.CreateImage(parent, Image, scale, position);
            UILogCustomSelectable logSelectable = image.gameObject.AddComponent<UILogCustomSelectable>();
            logSelectable.targetGraphic = (Graphic)image;
            return logSelectable;
        }

        public static UILogCustomSelectable CreateLogSelectable(
          Transform parent,
          string Imagepath,
          Vector2 scale,
          Vector2 position)
        {
            Image image = ModdingUtils.CreateImage(parent, Imagepath, scale, position);
            UILogCustomSelectable logSelectable = image.gameObject.AddComponent<UILogCustomSelectable>();
            logSelectable.targetGraphic = (Graphic)image;
            return logSelectable;
        }

        public static UICustomSelectable CreateSelectable(
          Transform parent,
          string Imagepath,
          Vector2 scale,
          Vector2 position,
          Vector2 sizedelta)
        {
            Image image = ModdingUtils.CreateImage(parent, Imagepath, scale, position, sizedelta);
            UICustomSelectable selectable = image.gameObject.AddComponent<UICustomSelectable>();
            selectable.targetGraphic = (Graphic)image;
            return selectable;
        }

        public static Button CreateButton(
          Transform parent,
          string Imagepath,
          Vector2 scale,
          Vector2 position,
          Vector2 sizedelta)
        {
            Image image = ModdingUtils.CreateImage(parent, Imagepath, scale, position, sizedelta);
            Button button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = (Graphic)image;
            return button;
        }

        public static Button CreateButton(
          Transform parent,
          string Imagepath,
          Vector2 scale,
          Vector2 position)
        {
            Image image = ModdingUtils.CreateImage(parent, Imagepath, scale, position);
            Button button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = (Graphic)image;
            return button;
        }

        public static Button CreateButton(Transform parent, string Imagepath)
        {
            return ModdingUtils.CreateButton(parent, Imagepath, new Vector2(1f, 1f), new Vector2(0.0f, 0.0f));
        }

        public static void SpriteTrace(string path, Sprite sprite)
        {
            string contents = sprite.name + Environment.NewLine + sprite.rect.ToString() + Environment.NewLine + sprite.border.ToString() + Environment.NewLine + sprite.pivot.ToString() + Environment.NewLine;
            File.WriteAllText(path, contents);
        }
    }
}
