/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS.Runtime
{
    public interface IPlayerController
    {
        float GetWalkSpeed();

        void SetWalkSpeed(float value);

        float GetWalkStepLength();

        void SetWalkStepLength(float value);

        float GetRunSpeed();

        void SetRunSpeed(float value);

        float GetRunStepLength();

        void SetRunStepLength(float value);

        float GetSprintSpeed();

        void SetSprintSpeed(float value);

        float GetSprintStepLength();

        void SetSprintStepLength(float value);

        float GetStepInterval();

        void SetStepInterval(float value);

        FPController.SprintDirection GetSprintDirection();

        void SetSprintDirection(FPController.SprintDirection value);

        float GetJumpForce();

        void SetJumpForce(float value);

        bool GetSimulateJumpDirectionImpulse();

        void SetSimulateJumpDirectionImpulse(bool value);

        float GetJumpDirectionImpulse();

        void SetJumpDirectionImpulse(float value);

        bool GetAirControl();

        void SetAirControl(bool value);

        float GetAirControlSpeed();

        void SetAirControlSpeed(float value);

        float GetStickToGroundForce();

        void SetStickToGroundForce(float value);

        float GetGravityMultiplier();

        void SetGravityMultiplier(float value);

        FootstepProperties GetFootstepProperties();

        void SetFootstepProperties(FootstepProperties value);

        Camera GetFPCamera();

        void SetFPCamera(Camera value);

        FPCameraLook GetCameraLook();

        CameraFOVKick GetFovKickSystem();

        UCurveControlledBob GetCurveControlledBobSystem();

        ULerpControlledBob GetJumpBobSystem();

        FPCrouch GetCrouchSystem();

        FPGrab GetGrabSystem();

        CameraTilt GetCameraTilt();

        FPSimpleClimb GetClimbSystem();

        CharacterController GetCharacterController();

        bool FovKickIsEnabled();

        void SetFovKickEnabled(bool value);

        bool HeadBobIsEnabled();

        void SetHeadBobEnabled(bool value);

        bool IsIdle();

        bool IsJumping();

        bool IsWalking();

        bool IsRunning();

        bool IsSprinting();

        bool IsGrounded();
    }
}