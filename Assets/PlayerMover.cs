using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    private SteamVR_TrackedController _controller;

    public float speed = 1;
    bool triggerIsPressed;

    public Transform playerRig;

    private void OnEnable() {
        _controller = GetComponent<SteamVR_TrackedController>();
        _controller.TriggerClicked += HandleTriggerClicked;
        _controller.TriggerUnclicked += HandleTriggerUnClicked;
    }

    private void OnDisable() {
        _controller.TriggerClicked -= HandleTriggerClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e) {
        // trigger clicked action
        triggerIsPressed = true;
    }

    private void HandleTriggerUnClicked(object sender, ClickedEventArgs e) {
        // trigger unclicked
        triggerIsPressed = false;
    }

    void Update() {
        if (!triggerIsPressed) return;

        Vector3 dir = transform.forward;

        playerRig.position += (playerRig.position + dir).normalized * speed * Time.deltaTime;
    }
}
