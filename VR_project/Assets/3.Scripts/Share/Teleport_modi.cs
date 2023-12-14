using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Teleport_modi : MonoBehaviour
{
    [SerializeField] private XRRayInteractor ray;
    [SerializeField] private XRInteractorLineVisual linerenderer;

    [SerializeField] private TeleportationProvider teleportation_provider;

    //vr���̾�� ����Ƽ ���̾�� �ٸ�
    [Header("Layer - teleport_area")]
    [SerializeField] private InteractionLayerMask teleport_area;//teleport_area ���̾� �ȿ� ���� ������ ������ ���� �̵��ϰ� ����

    [Header("Layer - teleport_anchor")]
    [SerializeField] private InteractionLayerMask teleport_anchor;//teleport_anchor ���̾� ���� Ư�� ������Ʈ�� ��Ȯ�� ������ ������ ���� �̵��ϰ� ����

    [Header("Mapping")]
    [SerializeField] private InputActionProperty teleportMode_active;
    [SerializeField] private InputActionProperty teleportMode_cancle;
    [SerializeField] private InputActionProperty gripmode_active;
    [SerializeField] private InputActionProperty trigger;

    public bool is_rayactive = false;
    private bool is_active;
    private InteractionLayerMask interaction_layer;
    private List<IXRInteractable> interactables = new List<IXRInteractable>(); //���̾�

    private void Start()
    {
        //Action.enable => ���� ������(Perfomed)�̺�Ʈ �߰�
        
        //action Ȱ��ȭ
        teleportMode_active.action.Enable();
        teleportMode_cancle.action.Enable();

        //action �̺�Ʈ �߰�
        teleportMode_active.action.performed += On_teleportactive;
        teleportMode_cancle.action.performed += On_teleportcancel;

        interaction_layer = ray.interactionLayers;
        linerenderer = ray.gameObject.GetComponent<XRInteractorLineVisual>();

        if (!is_rayactive)
        {
            linerenderer.enabled = is_rayactive;
        }

    }

    private void Update()
    {
        if (!is_active) return;
        if (trigger.action.IsPressed() || teleportMode_active.action.IsPressed())
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
            Turnoff_teleport();
            return;
        }
        ray.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        TeleportRequest request = new TeleportRequest();
        if (interactables[0].interactionLayers.Equals(teleport_area))
        {
            Vector3 pos = new Vector3(hit.point.x, request.destinationPosition.y, hit.point.z);
            request.destinationPosition = pos;

            linerenderer.enabled = false;
        }

        else if (interactables[0].interactionLayers.Equals(teleport_anchor))
        {
            request.destinationPosition = hit.transform.GetChild(0).transform.position;

            linerenderer.enabled = false;
        }

        teleportation_provider.QueueTeleportRequest(request);
        Turnoff_teleport();
    }

    private void On_teleportactive(InputAction.CallbackContext obj)
    {
        if (gripmode_active.action.phase != InputActionPhase.Performed || teleportMode_cancle.action.phase != InputActionPhase.Performed)
        {
            is_active = true;
            ray.lineType = XRRayInteractor.LineType.ProjectileCurve;
            ray.interactionLayers = teleport_area;
            ray.interactionLayers |= teleport_anchor; //���̾� �ͽ� ���
        }
    }

    private void On_teleportcancel(InputAction.CallbackContext obj)
    {
        Turnoff_teleport();
    }

    private void Turnoff_teleport()
    {
        is_active = false;
        ray.lineType = XRRayInteractor.LineType.StraightLine;
        ray.interactionLayers = interaction_layer;

    }
}
