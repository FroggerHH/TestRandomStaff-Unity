using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TestRandomStaff
{
    public class LoadImage : MonoBehaviour
    {
        [SerializeField] private Image renderer;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private LoadingMode mode = LoadingMode.Valheim;
        [SerializeField] private int height = 128;
        [SerializeField] private int width = 128;

        public Sprite GetSprite() => renderer.sprite;

        private void Awake() => button.onClick.AddListener(LoadImageFromWEB);

        private void Start()
        {
            if (inputField.text.Length == 0) LoadImageFromWEB();
        }

        public void LoadImageFromWEB()
        {
            renderer.sprite = null;
            var url = mode == LoadingMode.Valheim
                ? $"https://valheim-modding.github.io/Jotunn/Documentation/images/items/{inputField.text}.png"
                : inputField.text;
            LoadImageFromWEB(url,
                sprite1 => renderer.sprite = sprite1);
        }

        public void LoadImageFromWEB(string url, Action<Sprite> callback)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out _)) return;
            StopAllCoroutines();
            StartCoroutine(_Internal_LoadImage(url, callback));
        }

        private IEnumerator _Internal_LoadImage(string url, Action<Sprite> callback)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (texture.width == 0 || texture.height == 0) yield break;
                texture.ResizeCustom(newWidth: width, newHeight: height);
                var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f));
                callback?.Invoke(sprite);
            }
        }
    } 


    internal enum LoadingMode
    {
        Valheim,
        Url
    }
}