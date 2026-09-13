using UnityEngine;

public class ScreenRock : MonoBehaviour
{
    [SerializeField] Transform _loadingImage;
    [SerializeField] float _speed;
    float _rotate;
    float Rotate
    {
        get
        {
            _rotate += Time.deltaTime * _speed;
            _rotate %= 360;
            return _rotate;
        }
    }

    void OnEnable()
    {
        _rotate = 0;
    }

    void Update()
    {
        _loadingImage.rotation = Quaternion.Euler(0, 0, Rotate);
    }
}
