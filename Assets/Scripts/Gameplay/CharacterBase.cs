using ExitGames.Client.Photon;
using ProjectEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBase : MonoBehaviour {

	public Animator animator;
	public GameObject currentCharacter;
	private Renderer meshRenderer;

	private void Awake() {
		animator = currentCharacter.GetComponent<Animator>();
		meshRenderer = currentCharacter.GetComponentInChildren<Renderer>();
	}

}
