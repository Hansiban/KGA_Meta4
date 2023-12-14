using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportModi : MonoBehaviour
{
    [SerializeField] private XRRayInteractor ray;
    [SerializeField] private XRInteractorLineVisual linerenderer;

    [SerializeField] private TeleportationProvider teleportationProvider;

    [Header("Layer - Teleport Area")] // Unity Layer != VR Layer
    [SerializeField] private InteractionLayerMask teleportArea;

    [Header("Layer - Teleport Anchor")]
    [SerializeField] private InteractionLayerMask teleportAnchor;

    [SerializeField] private InputActionProperty teleportModeActive;
    [SerializeField] private InputActionProperty teleportModeCancel;
    [SerializeField] private InputActionProperty gridModeActive;
    [SerializeField] private InputActionProperty trigger;

    public bool isRayActive = false;
    private bool isActive;
    private InteractionLayerMask interactionLayer;
    private List<IXRInteractable> interactables = new List<IXRInteractable>(); // 부딪힐 공간을 나누기 위해 List로 선언

    private void Start()
    {
        /* Mapping 사용 순서
        1. Action.Enable() -> 수행(Performed)
        2. 이벤트 추가
         */
        // Action 활성화
        teleportModeActive.action.Enable();
        teleportModeCancel.action.Enable();
        // Action 이벤트 추가, Key Custom 가능
        teleportModeActive.action.performed += onTeleportActive; // 번개 모양 event 의미
        teleportModeCancel.action.performed += onTeleportCancel;

        interactionLayer = ray.interactionLayers;
        linerenderer = ray.gameObject.GetComponent<XRInteractorLineVisual>();

        if (!isRayActive)
        {
            linerenderer.enabled = isRayActive;
        }
    }

    private void Update()
    {
        if (!isActive) return;
        if (trigger.action.IsPressed() || teleportModeActive.action.IsPressed())
        {
            if (!linerenderer.enabled)
            {
                linerenderer.enabled = true;
            }
            return;
        }
        ray.GetValidTargets(interactables);
        if (interactables.Count <= 0)
        {
            TurnOffTeleport();
            return;
        }
        ray.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        TeleportRequest request = new TeleportRequest();
        if (interactables[0].interactionLayers.Equals(teleportArea))
        { // teleport Area, 정해진 구역 안, 어디서든 요청하면 지정한 위치로 GO
            Vector3 pos = new Vector3(hit.point.x, request.destinationPosition.y, hit.point.z);
            request.destinationPosition = pos; // request.destinationPosition : 텔레포트 시 목표 위치

            linerenderer.enabled = false;
        }
        else if (interactables[0].interactionLayers.Equals(teleportAnchor))
        { // teleport Anchor, 정해진 구역 안, 특정 포지션 위치에서 요청하면 정해진 위치로 GO
            request.destinationPosition = hit.transform.GetChild(0).transform.position;
            linerenderer.enabled = false;
        }
        teleportationProvider.QueueTeleportRequest(request); // Queue로 Teleport 요청을 받음
        TurnOffTeleport();
    }

    private void onTeleportActive(InputAction.CallbackContext obj)
    {
        if (gridModeActive.action.phase != InputActionPhase.Performed || teleportModeCancel.action.phase != InputActionPhase.Performed)
        {
            isActive = true;
            ray.lineType = XRRayInteractor.LineType.ProjectileCurve;
            ray.interactionLayers = teleportArea;
            ray.interactionLayers |= teleportAnchor; // Layer Mix - 레이어 여러개 하는 방법
        }
    }

    private void onTeleportCancel(InputAction.CallbackContext obj)
    {
        TurnOffTeleport();
    }

    private void TurnOffTeleport()
    { // teleport 안하게 꺼주는 method
        isActive = false;
        ray.lineType = XRRayInteractor.LineType.StraightLine;
        ray.interactionLayers = interactionLayer;
    }
}