/* ================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;
using UnrealFPS.Runtime;
using UnrealFPS.UI;

namespace UnrealFPS.Editor
{
    [CustomEditor(typeof(FPController))]
    [CanEditMultipleObjects]
    public class FPControllerEditor : UEditor<FPController>
    {
        internal new static class ContentProperties
        {
            public readonly static GUIContent CameraFoldout = new GUIContent("Camera Properies", "Camera and subsystem camera properties.");
            public readonly static GUIContent MovementFoldout = new GUIContent("Movement Properies");
            public readonly static GUIContent FootStepProperties = new GUIContent("Footstep Properties");
            public readonly static GUIContent CrouchSystem = new GUIContent("Crouch System");
            public readonly static GUIContent GrabSystem = new GUIContent("Grab System");
            public readonly static GUIContent TiltbSystem = new GUIContent("Tilt System");
            public readonly static GUIContent ClimbSystem = new GUIContent("Climb System");
            public readonly static GUIContent PickableItemSystem = new GUIContent("Pickable Item System");

            public readonly static GUIContent PlayerCamera = new GUIContent("Player Camera", "Player first person camera instance.");
            public readonly static GUIContent SensitivityByX = new GUIContent("Sensitivity X", "Sensitivity of mouse or joystick by X angle.");
            public readonly static GUIContent SensitivityByY = new GUIContent("Sensitivity Y", "Sensitivity of mouse or joystick by Y angle.");
            public readonly static GUIContent MinimumAngleByX = new GUIContent("Minimum X", "Minimum angle by X axis, when rotating the camera.");
            public readonly static GUIContent MaximumAngleByX = new GUIContent("Maximum X", "Maximum angle by X axis, when rotating the camera.");
            public readonly static GUIContent MinimumAngleByY = new GUIContent("Minimum Y", "Minimum angle by Y axis, when rotating the camera.");
            public readonly static GUIContent MaximumAngleByY = new GUIContent("Maximum Y", "Maximum angle by Y axis, when rotating the camera.");
            public readonly static GUIContent SmoothTime = new GUIContent("Smooth value", "Smoothing value while rotating camera.\nThe smaller the value, the stronger the smoothing.");
            public readonly static GUIContent Smooth = new GUIContent("Smooth Rotation", "Smoothing camera roation.");
            public readonly static GUIContent ClampVerticalRotation = new GUIContent("Vertical Rotation", "Clamp Vertical Rotation");
            public readonly static GUIContent ClampHorizontalRotation = new GUIContent("Horizontal Rotation", "Clamp Horizontal Rotation");
        }

        private bool movementFoldout;
        private bool cameraFoldout;
        private bool fovFoldout;
        private bool hdFoldout;
        private bool footstepPropertiesFoldout;
        private bool grabFoldout;
        private bool crouchFoldout;
        private bool tiltFoldout;
        private bool climbstepPropertiesFoldout;
        private bool climbFoldout;
        private bool pickableItemFoldout;

        public override string HeaderName()
        {
            return "First Person Controller";
        }

        public override void BaseGUI()
        {
            #region [Camera]
            BeginBox();
            IncreaseIndentLevel();
            cameraFoldout = EditorGUILayout.Foldout(cameraFoldout, ContentProperties.CameraFoldout, true);
            if (cameraFoldout)
            {
                if (instance.GetFPCamera() != null)
                {
                    instance.SetFPCamera((Camera)EditorGUILayout.ObjectField(ContentProperties.PlayerCamera, instance.GetFPCamera(), typeof(Camera), true));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    instance.SetFPCamera((Camera)EditorGUILayout.ObjectField(ContentProperties.PlayerCamera, instance.GetFPCamera(), typeof(Camera), true));
                    if (SearchButton())
                    {
                        Camera camera = UEditorInternal.FindFPCamera(instance.transform);
                        if (camera != null)
                        {
                            instance.SetFPCamera(camera);
                        }
                        else
                        {
                            UDisplayDialogs.Message("Searching", "Camera not found, try find it manually.");
                        }
                    }
                    GUILayout.EndHorizontal();
                    UEditorHelpBoxMessages.CameraError();
                }
                instance.GetCameraLook().SetSensitivityByX(EditorGUILayout.Slider(ContentProperties.SensitivityByX, instance.GetCameraLook().GetSensitivityByX(), 0.0f, 50.0f));
                instance.GetCameraLook().SetSensitivityByY(EditorGUILayout.Slider(ContentProperties.SensitivityByY, instance.GetCameraLook().GetSensitivityByY(), 0.0f, 50.0f));
                instance.GetCameraLook().SetMinimumAngleByX(EditorGUILayout.Slider(ContentProperties.MinimumAngleByX, instance.GetCameraLook().GetMinimumAngleByX(), -360.0f, 360.0f));
                instance.GetCameraLook().SetMaximumAngleByX(EditorGUILayout.Slider(ContentProperties.MaximumAngleByX, instance.GetCameraLook().GetMaximumAngleByX(), -360.0f, 360.0f));
                instance.GetCameraLook().SetMinimumAngleByY(EditorGUILayout.Slider(ContentProperties.MinimumAngleByY, instance.GetCameraLook().GetMinimumAngleByY(), -360.0f, 360.0f));
                instance.GetCameraLook().SetMaximumAngleByY(EditorGUILayout.Slider(ContentProperties.MaximumAngleByY, instance.GetCameraLook().GetMaximumAngleByY(), -360.0f, 360.0f));

                float smoothTime = instance.GetCameraLook().GetSmoothTime();
                bool smooth = instance.GetCameraLook().GetSmooth();
                HiddenFloatField(ContentProperties.SmoothTime, ContentProperties.Smooth, ref smoothTime, ref smooth);
                if (smoothTime == 0)
                {
                    smooth = false;
                    smoothTime = 15;
                }
                instance.GetCameraLook().SetSmooth(smooth);
                instance.GetCameraLook().SetSmoothTime(Mathf.Abs(smoothTime));

                instance.GetCameraLook().ClampVerticalRotation(EditorGUILayout.Toggle(ContentProperties.ClampVerticalRotation, instance.GetCameraLook().VerticalRotationClamped()));
                instance.GetCameraLook().ClampHorizontalRotation(EditorGUILayout.Toggle(ContentProperties.ClampHorizontalRotation, instance.GetCameraLook().HorizontalRotationClamped()));

                GUILayout.Space(10);

                GUILayout.Label("Effects", UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(5);
                BeginSubBox();
                EditorGUI.BeginDisabledGroup(!instance.FovKickIsEnabled());
                fovFoldout = EditorGUILayout.Foldout(fovFoldout, "Field Of View Kick System", true);
                if (fovFoldout)
                {
                    instance.GetFovKickSystem().SetFovIncrease(EditorGUILayout.Slider("Increase", instance.GetFovKickSystem().GetFovIncrease(), 0.0f, 50.0f));
                    instance.GetFovKickSystem().SetOriginalFov(EditorGUILayout.Slider("Original FOV", instance.GetFovKickSystem().GetOriginalFov(), 0.0f, 179.0f));
                    instance.GetFovKickSystem().SetIncreaseSpeed(EditorGUILayout.Slider("Increase Speed", instance.GetFovKickSystem().GetIncreaseSpeed(), 0.0f, 50.0f));
                    instance.GetFovKickSystem().SetDecreaseSpeed(EditorGUILayout.Slider("Decrease Speed", instance.GetFovKickSystem().GetDecreaseSpeed(), 0.0f, 50.0f));
                }
                string fovSystemToggleName = instance.FovKickIsEnabled() ? "Field Of View Kick Enabled" : "Field Of View Kick Disabled";
                EditorGUI.EndDisabledGroup();
                if (fovFoldout && !instance.FovKickIsEnabled())
                {
                    Rect notificationBackgroungRect = GUILayoutUtility.GetLastRect();
                    Rect notificationTextRect = GUILayoutUtility.GetLastRect();

                    notificationBackgroungRect.y -= 57;
                    notificationBackgroungRect.height = 75f;

                    notificationTextRect.y -= 45;
                    notificationTextRect.height = 50;

                    Notification("Field Of View Kick System Disabled", notificationBackgroungRect, notificationTextRect);
                }
                instance.SetFovKickEnabled(EditorGUILayout.Toggle(fovSystemToggleName, instance.FovKickIsEnabled()));
                EndSubBox();

                BeginSubBox();
                EditorGUI.BeginDisabledGroup(!instance.HeadBobIsEnabled());
                hdFoldout = EditorGUILayout.Foldout(hdFoldout, "Head Bob System", true);
                if (hdFoldout)
                {
                    instance.GetCurveControlledBobSystem().SetHorizontalBobRange(EditorGUILayout.Slider("Horizontal Range", instance.GetCurveControlledBobSystem().GetHorizontalBobRange(), 0.0f, 3.0f));
                    instance.GetCurveControlledBobSystem().SetVerticalBobRange(EditorGUILayout.Slider("Vertical Range", instance.GetCurveControlledBobSystem().GetVerticalBobRange(), 0.0f, 3.0f));
                    instance.GetCurveControlledBobSystem().SetVerticaltoHorizontalRatio(EditorGUILayout.Slider("Verticalto/Horizontal Ratio", instance.GetCurveControlledBobSystem().GetVerticaltoHorizontalRatio(), 0.0f, 10.0f));
                    instance.GetCurveControlledBobSystem().SetBobBaseInterval(EditorGUILayout.Slider("Interval", instance.GetCurveControlledBobSystem().GetBobBaseInterval(), 0.0f, 10.0f));
                    instance.GetCurveControlledBobSystem().SetBobCurve(EditorGUILayout.CurveField("Bob Curve", instance.GetCurveControlledBobSystem().GetBobCurve()));
                }
                string hdSystemToggleName = instance.HeadBobIsEnabled() ? "Head Bob Enabled" : "Head Bob Disabled";
                EditorGUI.EndDisabledGroup();
                if (hdFoldout && !instance.HeadBobIsEnabled())
                {
                    Rect notificationBackgroungRect = GUILayoutUtility.GetLastRect();
                    Rect notificationTextRect = GUILayoutUtility.GetLastRect();

                    notificationBackgroungRect.y -= 75;
                    notificationBackgroungRect.height = 93.5f;

                    notificationTextRect.y -= 58.5f;
                    notificationTextRect.height = 60;

                    Notification("Head Bob System Disabled", notificationBackgroungRect, notificationTextRect);
                }
                instance.SetHeadBobEnabled(EditorGUILayout.Toggle(hdSystemToggleName, instance.HeadBobIsEnabled()));
                EndSubBox();
            }
            EndBox();
            #endregion

            #region [Movement]
            BeginBox();
            movementFoldout = EditorGUILayout.Foldout(movementFoldout, ContentProperties.MovementFoldout, true);
            if (movementFoldout)
            {
                GUILayout.Label("Speed", UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(5);
                instance.SetWalkSpeed(EditorGUILayout.FloatField("Walk Speed", instance.GetWalkSpeed()));
                instance.SetRunSpeed(EditorGUILayout.FloatField("Run Speed", instance.GetRunSpeed()));
                instance.SetSprintSpeed(EditorGUILayout.FloatField("Sprint Speed", instance.GetSprintSpeed()));

                GUILayout.Space(10);
                GUILayout.Label("Step Properties", UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(5);
                instance.SetStepInterval(EditorGUILayout.FloatField("Step Interval", instance.GetStepInterval()));
                instance.SetWalkStepLength(EditorGUILayout.FloatField("Walk Lenghten", instance.GetWalkStepLength()));
                instance.SetRunStepLength(EditorGUILayout.FloatField("Run Lenghten", instance.GetRunStepLength()));
                instance.SetSprintStepLength(EditorGUILayout.FloatField("Sprint Lenghten", instance.GetSprintStepLength()));
                GUILayout.Space(10);

                GUILayout.Label("Jump", UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(5);
                instance.SetJumpForce(EditorGUILayout.FloatField("Jump Force", instance.GetJumpForce()));

                float airControlSpeed = instance.GetAirControlSpeed();
                bool airControl = instance.GetAirControl();
                HiddenFloatField("Air Control Speed", "Simulate Air Control", ref airControlSpeed, ref airControl);
                instance.SetAirControl(airControl);
                instance.SetAirControlSpeed(airControlSpeed);

                float jumpDirectionImpulse = instance.GetJumpDirectionImpulse();
                bool simulateJumpDirectionImpulse = instance.GetSimulateJumpDirectionImpulse();
                HiddenFloatField("Jump Direction Impulse", "Simulate Direction Impulse", ref jumpDirectionImpulse, ref simulateJumpDirectionImpulse);
                instance.SetSimulateJumpDirectionImpulse(simulateJumpDirectionImpulse);
                instance.SetJumpDirectionImpulse(jumpDirectionImpulse);

                instance.GetJumpBobSystem().SetBobAmount(EditorGUILayout.Slider("Bob Amount", instance.GetJumpBobSystem().GetBobAmount(), 0.0f, 10.0f));
                instance.GetJumpBobSystem().SetBobDuration(EditorGUILayout.Slider("Bob Duration", instance.GetJumpBobSystem().GetBobDuration(), 0.0f, 10.0f));
                GUILayout.Space(10);

                GUILayout.Label("Other", UEditorStyles.SectionHeaderLabel);
                GUILayout.Space(5);
                instance.SetSprintDirection((FPController.SprintDirection)EditorGUILayout.EnumPopup("Sprint Direction", instance.GetSprintDirection()));
                instance.SetStickToGroundForce(EditorGUILayout.FloatField("Stick To Ground Force", instance.GetStickToGroundForce()));
                instance.SetGravityMultiplier(EditorGUILayout.FloatField("Gravity Multiplier", instance.GetGravityMultiplier()));
            }
            EndBox();
            #endregion

            #region [FootStep Surface Properties]
            BeginBox();
            footstepPropertiesFoldout = EditorGUILayout.Foldout(footstepPropertiesFoldout, ContentProperties.FootStepProperties, true);
            if (footstepPropertiesFoldout)
            {
                instance.SetFootstepProperties((FootstepProperties)EditorGUILayout.ObjectField("Properties", instance.GetFootstepProperties(), typeof(FootstepProperties), true));
                if (instance.GetFootstepProperties() == null)
                {
                    UEditorHelpBoxMessages.Tip("Footstep sounds will not be played.", "For create Footstep Properties asset press right mouse button on Project window and select Create > Unreal FPS > Player > Footstep Properties.", true);
                }
            }
            EndBox();
            #endregion

            #region [Crouch System]
            BeginBox();
            crouchFoldout = EditorGUILayout.Foldout(crouchFoldout, ContentProperties.CrouchSystem, true);
            if (crouchFoldout)
            {
                instance.GetCrouchSystem().SetCrouchType((FPCrouch.CrouchType)EditorGUILayout.EnumPopup("Crouch Type", instance.GetCrouchSystem().GetCrouchType()));
                instance.GetCrouchSystem().SetSpeed(EditorGUILayout.FloatField("Speed", instance.GetCrouchSystem().GetSpeed()));
                instance.GetCrouchSystem().SetCrouchHeight(EditorGUILayout.FloatField("Height", instance.GetCrouchSystem().GetCrouchHeight()));
                instance.GetCrouchSystem().SetSmooth(EditorGUILayout.FloatField("Smooth", instance.GetCrouchSystem().GetSmooth()));
            }
            EndBox();
            #endregion

            #region [Grab System]
            BeginBox();
            grabFoldout = EditorGUILayout.Foldout(grabFoldout, ContentProperties.GrabSystem, true);
            if (grabFoldout)
            {
                instance.GetGrabSystem().SetGrabRange(EditorGUILayout.FloatField("Grab Range", instance.GetGrabSystem().GetGrabRange()));
                instance.GetGrabSystem().SetGrabDistance(EditorGUILayout.FloatField("Grab Distance", instance.GetGrabSystem().GetGrabDistance()));
                instance.GetGrabSystem().SetHeightOffset(EditorGUILayout.FloatField("Height Offset", instance.GetGrabSystem().GetHeightOffset()));
                instance.GetGrabSystem().SetSpring(EditorGUILayout.FloatField("Spring", instance.GetGrabSystem().GetSpring()));
                instance.GetGrabSystem().SetDrag(EditorGUILayout.FloatField("Drag", instance.GetGrabSystem().GetDrag()));
                instance.GetGrabSystem().SetDamper(EditorGUILayout.FloatField("Damper", instance.GetGrabSystem().GetDamper()));
                instance.GetGrabSystem().SetAngularDrag(EditorGUILayout.FloatField("Angular Drag", instance.GetGrabSystem().GetAngularDrag()));
                instance.GetGrabSystem().SetDistance(EditorGUILayout.FloatField("Distance", instance.GetGrabSystem().GetDistance()));
            }
            EndBox();
            #endregion

            #region [Tilts System]
            BeginBox();
            tiltFoldout = EditorGUILayout.Foldout(tiltFoldout, ContentProperties.TiltbSystem, true);
            if (tiltFoldout)
            {
                instance.GetCameraTilt().SetAngle(EditorGUILayout.Slider("Angle", instance.GetCameraTilt().GetAngle(), 0, 180.0f));
                instance.GetCameraTilt().SetOutputRange(EditorGUILayout.FloatField("Output Range", instance.GetCameraTilt().GetOutputRange()));
                instance.GetCameraTilt().SetRotateSpeed(EditorGUILayout.FloatField("Rotate Speed", instance.GetCameraTilt().GetRotateSpeed()));
                instance.GetCameraTilt().SetOutputSpeed(EditorGUILayout.FloatField("Output Speed", instance.GetCameraTilt().GetOutputSpeed()));
            }
            EndBox();
            #endregion

            #region [Climb System]
            BeginBox();
            climbFoldout = EditorGUILayout.Foldout(climbFoldout, ContentProperties.ClimbSystem, true);
            if (climbFoldout)
            {
                GUILayout.Label("Note: Climb System in Beta!", UEditorStyles.CenteredGrayBoldAndItalicLabel);
                GUILayout.Space(7);
                instance.GetClimbSystem().SetClimbStyle((FPSimpleClimb.ClimbStyle)EditorGUILayout.EnumPopup("Climb Style", instance.GetClimbSystem().GetClimbStart()));
                if (instance.GetClimbSystem().GetClimbStart() == FPSimpleClimb.ClimbStyle.ByKey)
                {
                    instance.GetClimbSystem().SetStartClimbKey((KeyCode)EditorGUILayout.EnumPopup("Start Climb Key", instance.GetClimbSystem().GetStartClimbKey()));
                }
                instance.GetClimbSystem().SetSpeed(EditorGUILayout.FloatField("Speed", instance.GetClimbSystem().GetSpeed()));
                instance.GetClimbSystem().SetDownThreshold(EditorGUILayout.Slider("Down Threshold", instance.GetClimbSystem().GetDownThreshold(), -1.0f, 1.0f));
                instance.GetClimbSystem().FreezLateralMove(EditorGUILayout.Toggle("Freez Lateral Move", instance.GetClimbSystem().FreezLateralMove()));
                EditorGUI.BeginDisabledGroup(instance.GetClimbSystem().GetClimbStepProperties() == null);
                instance.GetClimbSystem().SetPlaySoundCycle(EditorGUILayout.Slider("Play Sound Speed", instance.GetClimbSystem().GetPlaySoundCycle(), 0.0f, 10.0f));
                EditorGUI.EndDisabledGroup();
                instance.GetClimbSystem().SetClimbStepProperties((ClimbstepProperties)EditorGUILayout.ObjectField("Climbstep Properties", instance.GetClimbSystem().GetClimbStepProperties(), typeof(ClimbstepProperties), true));
                if (instance.GetClimbSystem().GetClimbStepProperties() == null)
                {
                    UEditorHelpBoxMessages.Tip("Climb step sounds will not be played.", "or create Climb Step Properties asset press right mouse button on Project window and select Create > Unreal FPS > Player > Climb Step Properties.", true);
                }
            }
            EndBox();
            #endregion

            #region [Pickable Item System]
            BeginBox();
            pickableItemFoldout = EditorGUILayout.Foldout(pickableItemFoldout, ContentProperties.PickableItemSystem, true);
            if (pickableItemFoldout)
            {
                instance.GetPickupItemSystem().SetHUD(ObjectField<HUDManager>("HUD Manager", instance.GetPickupItemSystem().GetHUD(), true));
                if (instance.GetPickupItemSystem().GetHUD() == null)
                {
                    UEditorHelpBoxMessages.Message("HUD Manager is empty!", MessageType.Warning, true);
                }
            }
            DecreaseIndentLevel();
            EndBox();
            #endregion
        }
    }
}