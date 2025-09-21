using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectEnums;

public class WindowStackEntry {

	public WindowPanel windowPanel;
	public WindowLocation location;
	public object parameters;

}

public static class WindowStackEntryExtensions {
	public static WindowStackEntry Copy(this WindowStackEntry entryToCopy) {
		WindowStackEntry copy = new WindowStackEntry();
		copy.windowPanel = entryToCopy.windowPanel;
		copy.location = entryToCopy.location;
		copy.parameters = entryToCopy.parameters; // Not a deep copy, change to a deep copy, if data shouldn't change in other instances
		return copy;
	}

}
