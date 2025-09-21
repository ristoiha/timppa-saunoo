using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstances : MonoBehaviour
{

	public GameObject gem;
	public Color color;
	public Material material;
    // Start is called before the first frame update
    void Start()
    {
        gem = this.gameObject;
		material = gem.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
		material.color = color;
    }
}
