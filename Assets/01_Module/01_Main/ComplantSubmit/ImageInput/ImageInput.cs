using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageInput : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] Button _imageSelect;
    [SerializeField] GameObject _selectImagePanel;
    [SerializeField] Button _imageDeselect;
    [SerializeField] RawImage _image;

    public event Action<string> OnSelectImage;
    public event Action OnDeselectImage;

    public void Init()
    {
        _imageSelect.onClick.AddListener(OnImageSelect);
        _imageDeselect.onClick.AddListener(OnImageDeselect);
    }

    void OnImageSelect()
    {
        PickImage();
    }

    public void OnImageDeselect()
    {
        _image.texture = null;
        OnDeselectImage?.Invoke();
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, 300);
        _imageSelect.gameObject.SetActive(true);
        _selectImagePanel.SetActive(false);
    }

    void PickImage()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                _image.texture = texture;
                OnSelectImage?.Invoke(path);

                float ratio = (float)_image.texture.height / _image.texture.width;
                float y = _rectTransform.sizeDelta.x * ratio;
                ratio = (y - 40) / (_rectTransform.sizeDelta.x - 40);
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.x * ratio);
                _imageSelect.gameObject.SetActive(false);
                _selectImagePanel.SetActive(true);
            }
        });
    }
}
