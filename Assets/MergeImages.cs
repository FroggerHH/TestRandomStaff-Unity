using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TestRandomStaff
{
    public class MergeImages : MonoBehaviour
    {
        [SerializeField] private Image renderer;
        [SerializeField] private Button button;
        [SerializeField] private LoadImage[] images = Array.Empty<LoadImage>();
        [SerializeField] private int height = 128;
        [SerializeField] private int width = 128;

        private void Awake()
        {
            button.onClick.AddListener(Merge);
            images = transform.root.GetComponentsInChildren<LoadImage>();
        }

        public async void Merge()
        {
            Sprite sprite = null;
            if (images == null || images.Length == 0) return;
            Resources.UnloadUnusedAssets();
            var texture = new Texture2D(width, height);
            for (int x = 0; x < texture.width; x++)
            for (int y = 0; y < texture.height; y++)
                texture.SetPixel(x, y, Color.clear);

            texture.Apply();
            sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f));
            sprite.name = "Merged state 0";
            renderer.sprite = sprite;

            for (int i = 0; i < images.Length; i++)
            {
                if(images[i] == null || images[i].GetSprite() == null) continue;
                var textureI = images[i].GetSprite().texture.ResizeCustom(width, height);
                for (int x = 0; x < textureI.width; x++)
                for (int y = 0; y < textureI.height; y++)
                {
                    var newPixel = textureI.GetPixel(x, y);

                    if (newPixel.a != 0) texture.SetPixel(x, y, newPixel);
                }

                texture.Apply();
                sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f));
                sprite.name = $"Merged state {i + 1}";
                renderer.sprite = sprite;
                //await Task.Delay(2000);
            }

            texture.Apply();
            sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0.5f, 0.5f));
            sprite.name = "Done merged image";
            renderer.sprite = sprite;
        }
    }
}





























//https://catherineasquithgallery.com/uploads/posts/2023-01/1674279261_catherineasquithgallery-com-p-serii-fon-meditsina-foto-2.jpg
//https://celes.club/pictures/uploads/posts/2023-06/1687588175_celes-club-p-versachi-risunok-uzor-risunok-vkontakte-82.png
//https://www.clipartmax.com/png/full/416-4169199_scalloped-square-clip-art-frames-free-download.png
//MaceSilver
