using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetPointer : MonoBehaviour
{
	public static TargetPointer Instance;
	public Transform Target; // цель
	public RectTransform PointerUI; // объект Image UI
	public Sprite PointerIcon; // иконка когда цель в поле видимости
	public Sprite OutOfScreenIcon; // иконка когда цель за приделами экрана	
	public float InterfaceScale = 100; // масштаб интерфейса

	private Vector3 startPointerSize;
	private Camera mainCamera;

	public void Init(GameObject canvas)
	{
		Instance = this;
		PointerUI = canvas.GetComponent<UIHolder>().Target.GetComponent<RectTransform>();
		startPointerSize = PointerUI.sizeDelta;
		mainCamera = Camera.main;		
	}

	public void SetTarget(Transform newTarget)
	{
		Target = newTarget;
	}
	
	public void RemoveTarget()
	{
		Target = null;
	}
	
	private void LateUpdate()
	{
		if (!Target)
		{
			PointerUI.gameObject.SetActive(false);
			return;
		}
		PointerUI.gameObject.SetActive(true);

		var realPos = mainCamera.WorldToScreenPoint(Target.position); // получениее экранных координат объекта
		var rect = new Rect(0, 0, Screen.width, Screen.height);

		var outPos = realPos;
		float direction = 1;

		PointerUI.GetComponent<Image>().sprite = OutOfScreenIcon;

		if (!IsBehind(Target.position)) // если цель спереди
		{
			if (rect.Contains(realPos)) // и если цель в окне экрана
			{
				PointerUI.GetComponent<Image>().sprite = PointerIcon;
			}
		}
		else // если цель cзади
		{
			realPos = -realPos;
			outPos = new Vector3(realPos.x, 0, 0); // позиция иконки - снизу
			if (mainCamera.transform.position.y < Target.position.y)
			{
				direction = -1;
				outPos.y = Screen.height; // позиция иконки - сверху				
			}
		}
		// ограничиваем позицию областью экрана
		var offset = PointerUI.sizeDelta.x / 2;
		outPos.x = Mathf.Clamp(outPos.x, offset, Screen.width - offset);
		outPos.y = Mathf.Clamp(outPos.y, offset, Screen.height - offset);

		var pos = realPos - outPos; // направление к цели из PointerUI 

		RotatePointer(direction * pos);

		PointerUI.sizeDelta = new Vector2(startPointerSize.x / 100 * InterfaceScale, startPointerSize.y / 100 * InterfaceScale);
		PointerUI.anchoredPosition = outPos;
	}
	private bool IsBehind(Vector3 point) // true если point сзади камеры
	{		
		var forward = mainCamera.transform.TransformDirection(Vector3.forward);
		var toOther = point - mainCamera.transform.position;
		if (Vector3.Dot(forward, toOther) < 0) return true;
		return false;
	}
	private void RotatePointer(Vector2 direction) // поворачивает PointerUI в направление direction
	{		
		var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		PointerUI.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}