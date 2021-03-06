﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	[SerializeField] public Animator animator;

    private int levelToLoad;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1)) {
            fadeToLevel(0);
        }
	}

    public void fadeToLevel(int level) {
        levelToLoad = level;
        animator.SetTrigger("FadeOut");
    }

    public void onFadeComplete() {
        SceneManager.LoadScene(levelToLoad);
    }
}
