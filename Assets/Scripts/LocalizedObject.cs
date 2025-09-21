using ProjectEnums;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using RoboRyanTron.SearchableEnum;
public class LocalizedObject : MonoBehaviour {

	[SearchableEnum]
	[SerializeField] protected LocID locID = default;

	protected static List<LocalizedObject> localizedObjects = new List<LocalizedObject>();
	protected bool shouldUpdate = true;

	protected virtual void Awake() {
		Setup();
	}
	protected virtual void OnDestroy() {
		if (localizedObjects.Contains(this))
			localizedObjects.Remove(this);
	}
	protected virtual void Start() {
		if (shouldUpdate)
			UpdateContent();
	}
	protected virtual void Update() {
		if (shouldUpdate)
			UpdateContent();
	}
	protected virtual void UpdateContent() {
		//Run base.UpdateContent() in the end of the overriden method   
		shouldUpdate = false;
	}
	protected virtual void Setup() {
		//Run base.Setup() in the end of the overriden method
		localizedObjects.Add(this);
	}

	public static void UpdateLocalization() {
		for (int i = 0; i < localizedObjects.Count; i++) {
			localizedObjects[i].UpdateContent();
		}
	}
	public void SetLocID(LocID locID) {
		this.locID = locID;
		shouldUpdate = true;
	}
}
