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

    //vr레이어랑 유니티 레이어는 다름
    [Header("Layer - teleport_area")]
    [SerializeField] private InteractionLayerMask teleport_area;//teleport_area 레이어 안에 어디든 찍으면 정해진 곳에 이동하게 만듬

    [Header("Layer - teleport_anchor")]
    [SerializeField] private InteractionLayerMask teleport_anchor;//teleport_anchor 레이어 안의 특정 오브젝트를 정확히 찍으면 정해진 곳에 이동하게 만듬

    [Header("Mapping")]
    [SerializeField] private InputActionProperty teleportMode_active;
    [SerializeField] private InputActionProperty teleportMode_cancle;
    [SerializeField] private InputActionProperty gripmode_active;
    [SerializeField] private InputActionProperty trigger;

    public bool is_rayactive = false;
    private bool is_active;
    private InteractionLayerMask interaction_layer;
    private List<IXRInteractable> interactables = new List<IXRInteractable>(); //레이어

    private void Start()
    {
        //Action.enable => 수행 했을때(Perfomed)이벤트 추가
        
        //action 활성화
        teleportMode_active.action.Enable();
        teleportMode_cancle.action.Enable();

        //action 이벤트 추가
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
            ray.interactionLayers |= teleport_anchor; //레이어 믹스 방법
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
