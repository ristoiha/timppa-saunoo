using ProjectEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.GPUSort;
using static UnityEngine.Video.VideoPlayer;

public class UIEventManager : MonoBehaviour {

	public static UIEventManager instance;

	private Dictionary<CustomCommand, Action<List<WindowStackEntry>, CommandVariable>> customCommandHandlers = new Dictionary<CustomCommand, Action<List<WindowStackEntry>, CommandVariable>>();
	private Dictionary<Command, Action<List<WindowStackEntry>, CommandVariable>> commandHandlers = new Dictionary<Command, Action<List<WindowStackEntry>, CommandVariable>>();
	private Dictionary<WindowPanelAction, Action<WindowPanel, WindowPanel>> windowPanelHandlers = new Dictionary<WindowPanelAction, Action<WindowPanel, WindowPanel>>();
	private Dictionary<ContentPanelAction, Action<CommandVariable, Transform>> contentPanelHandlers = new Dictionary<ContentPanelAction, Action<CommandVariable, Transform>>();


	private void Awake() {
		if (instance == null) {
			instance = this;
			RegisterHandlersCustomCommand();
			RegisterHandlersCommand();
			RegisterHandlersWindowPanel();
			RegisterHandlersContentPanelAsset();
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		if (instance == this) {
			instance = null;
		}
	}

	private void RegisterHandlersCustomCommand() {
		//customCommandHandlers.Add(CustomCommand.OpenLiftPanel, (windowStack, commandVariable) => {
		//	ExhibitAsset LiftPackageAsset = (ExhibitAsset)commandVariable.scriptableObjectValue;
		//	LocIDVariable.SetRuntimeValue(LocID.Value_LastLiftPackage, LiftPackageAsset.locID);
		//	WindowManager.instance.CloseWindow(WindowPanel.LiftPackageSelectionPanel);
		//	WindowManager.instance.OpenWindow(WindowPanel.LiftSelectionPanel, commandVariable.scriptableObjectValue, windowStack);
		//});

	}

	private void RegisterHandlersWindowPanel() {
		windowPanelHandlers.Add(WindowPanelAction.OpenWindow, (windowPanel, callerPanel) => {
			WindowBase callerWindowBase = WindowManager.instance.GetWindow(callerPanel);
			WindowManager.instance.OpenWindow(windowPanel, null, callerWindowBase.windowStack);
		});

		windowPanelHandlers.Add(WindowPanelAction.CloseWindow, (windowPanel, callerPanel) => {
			WindowManager.instance.CloseWindow(windowPanel);
		});

		windowPanelHandlers.Add(WindowPanelAction.SwitchToWindow, (windowPanel, callerPanel) => {
			WindowBase callerWindowBase = WindowManager.instance.GetWindow(callerPanel);
			WindowManager.instance.CloseWindow(callerPanel);
			WindowManager.instance.OpenWindow(windowPanel, null, callerWindowBase.windowStack);
		});

		windowPanelHandlers.Add(WindowPanelAction.ToggleWindow, (windowPanel, callerPanel) => {
			if (WindowManager.instance.WindowIsOpen(windowPanel) == true) {
				WindowManager.instance.CloseWindow(windowPanel);
			}
			else {
				WindowBase callerWindowBase = WindowManager.instance.GetWindow(callerPanel);
				WindowManager.instance.OpenWindow(windowPanel, null, callerWindowBase.windowStack);
			}
		});

		windowPanelHandlers.Add(WindowPanelAction.UpdateWindow, (windowPanel, callerPanel) => {
			WindowManager.instance.UpdateWindow(windowPanel);
		});
	}

	private void RegisterHandlersContentPanelAsset() {
		contentPanelHandlers.Add(ContentPanelAction.OpenWindow, (commandVariable, callerTransform) => {
			GenericContentWindow genericContentWindow = new GenericContentWindow(commandVariable.contentPanelAsset.style, commandVariable.contentPanelAsset);
			WindowBase windowBase = callerTransform.GetComponentInParent<WindowBase>();
			WindowManager.instance.OpenWindow(WindowPanel.GenericContentPanel, genericContentWindow, windowBase.windowStack);
		});

		contentPanelHandlers.Add(ContentPanelAction.CloseWindow, (commandVariable, callerTransform) => {
			WindowManager.instance.CloseWindow(WindowPanel.GenericContentPanel, commandVariable.contentPanelAsset);
		});

		contentPanelHandlers.Add(ContentPanelAction.SwitchToWindow, (commandVariable, callerTransform) => {
			WindowBase windowBase = callerTransform.GetComponentInParent<WindowBase>();
			WindowManager.instance.CloseWindow(windowBase.window, windowBase.windowStack);
			contentPanelHandlers[ContentPanelAction.OpenWindow](commandVariable, callerTransform);
		});

	}

	private void RegisterHandlersCommand() {
		commandHandlers.Add(Command.SaveChanges, (windowStack, commandVariable) => {
			UserProfile.SaveCurrent();
		});

		commandHandlers.Add(Command.RevertChanges, (windowStack, commandVariable) => {
			GenericContentPanel contentPanel = (GenericContentPanel)WindowManager.instance.GetWindow(WindowPanel.GenericContentPanel, windowStack);
			if (contentPanel != null) {
				VariableContent.ResetValues();
			}
		});

		commandHandlers.Add(Command.SwitchToPreviousWindow, (windowStack, commandVariable) => {
			WindowStackEntry currentWindow = windowStack[windowStack.Count - 1];
			WindowManager.instance.CloseWindow(currentWindow.windowPanel, currentWindow.parameters);
			windowStack.RemoveAt(windowStack.Count - 1);
			if (windowStack.Count > 0) {
				WindowStackEntry switchWindow = windowStack[windowStack.Count - 1]; // Get panel to switch to
				windowStack.RemoveAt(windowStack.Count - 1); // remove previous panel from stack
				WindowManager.instance.OpenWindow(switchWindow.windowPanel, switchWindow.parameters, windowStack); // Open previous panel (that adds back to stack)
			}
		});

		commandHandlers.Add(Command.ExitGame, (windowStack, commandVariable) => {
#if !UNITY_EDITOR
			Application.Quit();
#endif
		});

		commandHandlers.Add(Command.StartGame, (windowStack, commandVariable) => {
			SceneLoader.instance.LoadScene(SceneID.Gameplay);
		});

		commandHandlers.Add(Command.ShowNotification, (windowStack, commandVariable) => {
			GenericWindow window = new GenericWindow(WindowStyle.NotificationLarge, commandVariable.locIDValue);
			window.AddButton(LocID.Ok, delegate () {
				WindowManager.instance.CloseWindow(WindowPanel.GenericMessagePanel);
				WindowManager.instance.OpenWindow(windowStack[windowStack.Count - 1].windowPanel, windowStack[windowStack.Count - 1].parameters, windowStack);
			});
			WindowManager.instance.CloseWindow(windowStack[windowStack.Count - 1].windowPanel);
			WindowManager.instance.OpenWindow(WindowPanel.GenericMessagePanel, window, windowStack);
		});

		commandHandlers.Add(Command.LoadScene, (windowStack, commandVariable) => {
			SceneLoader.instance.LoadScene(commandVariable.sceneIDValue);
		});
	}

	public void TriggerEvent(CommandVariable commandVariable, Transform callerTransform) {
		TouchIdentifier.EnableDoubleClickProtection();
		if (commandVariable.buttonActionType == ButtonActionType.Command) {
			WindowBase callerWindowBase = callerTransform.GetComponentInParent<WindowBase>();
			commandHandlers[commandVariable.command](callerWindowBase.windowStack, commandVariable);
		}
		else if (commandVariable.buttonActionType == ButtonActionType.WindowPanel) {
			WindowPanel callerPanel = callerTransform.GetComponentInParent<WindowBase>().window;
			windowPanelHandlers[commandVariable.windowPanelAction](commandVariable.windowPanelValue, callerPanel);
		}
		else if (commandVariable.buttonActionType == ButtonActionType.SetVariable || commandVariable.buttonActionType == ButtonActionType.SetRuntimeVariable || commandVariable.buttonActionType == ButtonActionType.SetSceneVariable) {
			VariableTarget target = VariableTarget.SceneValue;
			if (commandVariable.buttonActionType == ButtonActionType.SetRuntimeVariable) { target = VariableTarget.RuntimeValue; }
			else if (commandVariable.buttonActionType == ButtonActionType.SetSceneVariable) { target = VariableTarget.SceneValue; }
			else if (commandVariable.buttonActionType == ButtonActionType.SetVariable) { target = VariableTarget.PermanentValue; }

			if (commandVariable.variableType == VariableType.Float) {
				FloatVariable.SetValue(commandVariable.variable, commandVariable.floatValue, target);
			}
			else if (commandVariable.variableType == VariableType.Int) {
				IntVariable.SetValue(commandVariable.variable, commandVariable.intValue, target);
			}
			else if (commandVariable.variableType == VariableType.String) {
				StringVariable.SetValue(commandVariable.variable, commandVariable.stringValue, target);
			}
			else if (commandVariable.variableType == VariableType.Bool) {
				BoolVariable.SetValue(commandVariable.variable, commandVariable.boolValue, target);
			}
			else if (commandVariable.variableType == VariableType.LocID) {
				LocIDVariable.SetValue(commandVariable.variable, commandVariable.locIDValue, target);
			}
		}
		else if (commandVariable.buttonActionType == ButtonActionType.ContentPanel) {
			contentPanelHandlers[commandVariable.contentPanelAction](commandVariable, callerTransform);
		}
		else if (commandVariable.buttonActionType == ButtonActionType.CustomCommand) {
			WindowBase callerWindowBase = callerTransform.GetComponentInParent<WindowBase>();
			customCommandHandlers[commandVariable.customCommand](callerWindowBase.windowStack, commandVariable);
		}
	}


}