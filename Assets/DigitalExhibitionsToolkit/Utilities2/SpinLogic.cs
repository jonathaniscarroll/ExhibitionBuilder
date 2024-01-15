using UnityEngine;
using System.Collections;

public class SpinLogic : MonoBehaviour
{
	float f_lastX = 0.0f;
	float f_difX = 0.5f;
	float f_lastY = 0.0f;
	float f_difY = 0.5f;
	int i_direction = 1;
	public float verticalSpeed = 1f;
	public float horizontalSpeed = 1f;
	public FloatEvent OutputX;
	public FloatEvent OutputY;

	// Update is called once per frame
	void Update()
	{
		f_difX = Mathf.Abs(f_lastX - Input.GetAxis("Mouse X"));

		if (f_lastX < Input.GetAxis("Mouse X"))
		{
			i_direction = -1;
			transform.Rotate(Vector3.up, -f_difX * horizontalSpeed);
		}

		if (f_lastX > Input.GetAxis("Mouse X"))
		{
			i_direction = 1;
			transform.Rotate(Vector3.up, f_difX * horizontalSpeed);
		}

		f_lastX = -Input.GetAxis("Mouse X");
		OutputX.Invoke(f_difX);

		f_difY = Mathf.Abs(f_lastY - Input.GetAxis("Mouse Y"));

		if (f_lastY < Input.GetAxis("Mouse Y"))
		{
			i_direction = -1;
			transform.Rotate(Vector3.right, -f_difY * verticalSpeed);
		}

		if (f_lastY > Input.GetAxis("Mouse Y"))
		{
			i_direction = 1;
			transform.Rotate(Vector3.right, f_difY * verticalSpeed);
		}

		f_lastY = -Input.GetAxis("Mouse Y");
		OutputY.Invoke(f_difY);
	}
}