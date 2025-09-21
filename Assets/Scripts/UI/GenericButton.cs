using ProjectEnums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton {

	[System.NonSerialized] public LocID locID;
	[System.NonSerialized] public Action action;

	public GenericButton(LocID id, Action act) {
		locID = id;
		action = act;
	}

}

public class GenericButton<T> {

	[System.NonSerialized] public LocID locID;
	[System.NonSerialized] public Action<T> action;

	public GenericButton(LocID id, Action<T> act) {
		locID = id;
		action = act;
	}

}

