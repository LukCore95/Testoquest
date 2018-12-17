using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    [SerializeField]
	private Vector3 rotatePower = new Vector3(0, 100, 0);

    private Vector3 angle;

	void Start()
	{
		angle = transform.eulerAngles;
	}

	void Update()
	{
		angle += rotatePower * Time.deltaTime;
		transform.eulerAngles = angle;
	}

}
