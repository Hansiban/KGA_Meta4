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
    private List<IXRInteractable> interactables = new List<IXRInteractable>(); // �ε��� ������ ������ ���� List�� ����

    private void Start()
    {
        /* Mapping ��� ����
        1. Action.Enable() -> ����(Performed)
        2. �̺�Ʈ �߰�
         */
        // Action Ȱ��ȭ
        teleportModeActive.action.Enable();
        teleportModeCancel.action.Enable();
        // Action �̺�Ʈ �߰�, Key Custom ����
        teleportModeActive.action.performed += onTeleportActive; // ���� ��� event �ǹ�
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
        { // teleport Area, ������ ���� ��, ��𼭵� ��û�ϸ� ������ ��ġ�� GO
            Vector3 pos = new Vector3(hit.point.x, request.destinationPosition.y, hit.point.z);
            request.destinationPosition = pos; // request.destinationPosition : �ڷ���Ʈ �� ��ǥ ��ġ

            linerenderer.enabled = false;
        }
        else if (interactables[0].interactionLayers.Equals(teleportAnchor))
        { // teleport Anchor, ������ ���� ��, Ư�� ������ ��ġ���� ��û�ϸ� ������ ��ġ�� GO
            request.destinationPosition = hit.transform.GetChild(0).transform.position;
            linerenderer.enabled = false;
        }
        teleportationProvider.QueueTeleportRequest(request); // Queue�� Teleport ��û�� ����
        TurnOffTeleport();
    }

    private void onTeleportActive(InputAction.CallbackContext obj)
    {
        if (gridModeActive.action.phase != InputActionPhase.Performed || teleportModeCancel.action.phase != InputActionPhase.Performed)
        {
            isActive = true;
            ray.lineType = XRRayInteractor.LineType.ProjectileCurve;
            ray.interactionLayers = teleportArea;
            ray.interactionLayers |= teleportAnchor; // Layer Mix - ���̾� ������ �ϴ� ���
        }
    }

    private void onTeleportCancel(InputAction.CallbackContext obj)
    {
        TurnOffTeleport();
    }

    private void TurnOffTeleport()
    { // teleport ���ϰ� ���ִ� method
        isActive = false;
        ray.lineType = XRRayInteractor.LineType.StraightLine;
        ray.interactionLayers = interactionLayer;
    }
}