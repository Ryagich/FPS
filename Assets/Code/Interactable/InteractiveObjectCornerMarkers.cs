using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractiveObjectCornerMarkers : MonoBehaviour
{
    [SerializeField, Tooltip("The box colliders of the interactive object.")]
    private BoxCollider[] _boxColliders = new BoxCollider[0];

    [SerializeField,
     Tooltip(
         "The prefab to use for the corner objects. 8 instances of this will be instantiated and placed at the corners of the box.")]
    private GameObject _cornerObject = null;

    private Transform[] corners = null;

    protected void Start()
    {
        // Allocate transform array
        var boxCount = 0;
        for (var i = 0; i < _boxColliders.Length; ++i)
        {
            if (_boxColliders[i] != null)
                ++boxCount;
        }

        corners = new Transform[boxCount * 8];

        // Iterate through & add corners
        var itr = 0;
        for (var i = 0; i < _boxColliders.Length; ++i)
        {
            var box = _boxColliders[i];
            if (box == null)
                continue;

            var startIndex = itr * 8;
            for (var j = 0; j < 8; ++j)
            {
                corners[startIndex + j] = Instantiate(_cornerObject).transform;
                corners[startIndex + j].SetParent(box.transform, false);
            }

            corners[startIndex + 0].localPosition = new Vector3(
                box.center.x + box.size.x * 0.5f,
                box.center.y + box.size.y * 0.5f,
                box.center.z + box.size.z * 0.5f
            );
            corners[startIndex + 0].localRotation = Quaternion.Euler(new Vector3(90f, 180f, 0f));

            corners[startIndex + 1].localPosition = new Vector3(
                box.center.x - box.size.x * 0.5f,
                box.center.y + box.size.y * 0.5f,
                box.center.z + box.size.z * 0.5f
            );
            corners[startIndex + 1].localRotation = Quaternion.Euler(new Vector3(90f, 90f, 0f));

            corners[startIndex + 2].localPosition = new Vector3(
                box.center.x - box.size.x * 0.5f,
                box.center.y - box.size.y * 0.5f,
                box.center.z + box.size.z * 0.5f
            );
            corners[startIndex + 2].localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

            corners[startIndex + 3].localPosition = new Vector3(
                box.center.x + box.size.x * 0.5f,
                box.center.y - box.size.y * 0.5f,
                box.center.z + box.size.z * 0.5f
            );
            corners[startIndex + 3].localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            corners[startIndex + 4].localPosition = new Vector3(
                box.center.x + box.size.x * 0.5f,
                box.center.y + box.size.y * 0.5f,
                box.center.z - box.size.z * 0.5f
            );
            corners[startIndex + 4].localRotation = Quaternion.Euler(new Vector3(90f, -90f, 0f));

            corners[startIndex + 5].localPosition = new Vector3(
                box.center.x - box.size.x * 0.5f,
                box.center.y + box.size.y * 0.5f,
                box.center.z - box.size.z * 0.5f
            );
            corners[startIndex + 5].localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

            corners[startIndex + 6].localPosition = new Vector3(
                box.center.x - box.size.x * 0.5f,
                box.center.y - box.size.y * 0.5f,
                box.center.z - box.size.z * 0.5f
            );
            corners[startIndex + 6].localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            corners[startIndex + 7].localPosition = new Vector3(
                box.center.x + box.size.x * 0.5f,
                box.center.y - box.size.y * 0.5f,
                box.center.z - box.size.z * 0.5f
            );
            corners[startIndex + 7].localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));

            // Fix scale
            // for (int j = 0; j < 8; ++j)
            // {
            //     //m_Corners[startIndex + j].SetParent(t, true);
            //     var s = corners[startIndex + j].lossyScale;
            //     corners[startIndex + j].localScale = new Vector3(
            //         Mathf.Abs(1f / s.x), Mathf.Abs(1f / s.y), Mathf.Abs(1f / s.z)
            //     );
            // }

            ++itr;
        }

        Hide();
    }

    [Button("Show")]
    public void Show()
    {
        for (var i = 0; i < corners.Length; ++i)
            corners[i].gameObject.SetActive(true);
    }

    [Button("Hide")]
    public void Hide()
    {
        for (var i = 0; i < corners.Length; ++i)
            corners[i].gameObject.SetActive(false);
    }
}