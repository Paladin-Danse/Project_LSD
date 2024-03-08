/* ================================================================
   ---------------------------------------------------
   Project   :    #0001
   Publisher :    #0002
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnrealFPS.Editor
{
    public static class UEditorInternal
    {
        /// <summary>
        /// Find first person player with Player tag in scene.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag(TNC.PLAYER);
            if (player != null)
                return player.transform;
            return null;
        }

        /// <summary>
        /// Find first person camera with FPCamera tag in player instance childs.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Camera FindFPCamera(Transform transform)
        {
            Transform player = FindPlayer();
            if (player == null)
            {
                return null;
            }

            Camera[] cameras = player.GetComponentsInChildren<Camera>(true);
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].tag == TNC.CAMERA)
                {
                    return cameras[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Find first person camera with FPWeaponLayer tag in player instance childs.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Camera FindFPWeaponLayer(Transform transform)
        {
            Transform player = FindPlayer();
            if (player == null)
            {
                return null;
            }

            Camera[] cameras = player.GetComponentsInChildren<Camera>(true);
            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].tag == TNC.CAMERA_LAYER)
                {
                    return cameras[i];
                }
            }
            return null;
        }

        public static T FindComponent<T>(Transform transform) where T : Component
        {
            if (transform == null)
                return null;

            T t = transform.GetComponentInChildren<T>();
            if (t != null)
                return t;
            return null;
        }

        public static T AddComponent<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
                component = gameObject.AddComponent<T>();
            return component;
        }

        public static Component AddComponent(Component component, GameObject gameObject)
        {
            System.Type _type = component.GetType();
            component = gameObject.GetComponent(_type);
            if (component == null)
                component = gameObject.AddComponent(_type);
            return component;
        }

        /// <summary>
        /// Find muzzle flash in weapon instance childs.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static ParticleSystem FindMuzzleFlash(Transform transform)
        {
            ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i].name.Contains("Muzzle"))
                {
                    return particleSystems[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Find cartridge ejection in weapon instance childs.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static ParticleSystem FindCartridgeEjection(Transform transform)
        {
            ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i].name.Contains("Cartridge"))
                {
                    return particleSystems[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Find fire/attack point in camera instance childs.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform FindFirePoint(Transform transform)
        {
            Transform[] transforms = transform.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name.Contains("Fire Point") ||
                    transforms[i].name.Contains("FirePoint") ||
                    transforms[i].name.Contains("Attack Point") ||
                    transforms[i].name.Contains("AttackPoint"))
                {
                    return transforms[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Move component with type T to top in inspector.
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void MoveComponentTop<T>(Transform target) where T : Component
        {
            T targetComponent = target.GetComponent<T>();
            Component[] components = target.GetComponents<Component>();
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == targetComponent)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(targetComponent);
            }
        }

        /// <summary>
        /// Move component with type T to bottom in inspector.
        /// </summary>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void MoveComponentBottom<T>(Transform target) where T : Component
        {
            T targetComponent = target.GetComponent<T>();
            Component[] components = target.GetComponents<Component>();
            int length = components.Length;
            int index = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == targetComponent)
                {
                    index = i;
                }
            }
            int some = length - index;
            for (int i = 0; i < some; i++)
            {
                UnityEditorInternal.ComponentUtility.MoveComponentDown(targetComponent);
            }
        }

        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static List<Object> FindAssetsByType(System.Type type)
        {
            List<Object> assets = new List<Object>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        /// <summary>
        /// Return all animation clips from transform animator.
        /// If animator not found return null.
        /// </summary>
        /// <returns></returns>
        public static AnimationClip[] GetAllClips(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.runtimeAnimatorController.animationClips;
        }

        /// <summary>
        /// Return all animator parameter names.
        /// If animator not found return null.
        /// </summary>
        /// <returns></returns>
        public static string[] GetAnimatorParameterNames(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.parameters.Select(n => n.name).ToArray();
        }

        /// <summary>
        /// Return all animator parameters.
        /// If animator not found return null.
        /// </summary>
        /// <returns></returns>
        public static UnityEngine.AnimatorControllerParameter[] GetAnimatorParameters(Animator animator)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
                return null;

            return animator.parameters;
        }

        /// <summary>
        /// Verify project inputs and return missing require Unreal FPS inputs. 
        /// </summary>
        /// <param name="type">[Axes], [Buttons], [All]</param>
        /// <returns>Axes / Buttons / All</returns>
        public static string[] GetMissingInput(string type)
        {
            List<string> missingAxes = new List<string>();
            List<string> incAxes = new List<string>();
            switch (type)
            {
                case "Axes":
                    incAxes.AddRange(INC.GetAxes());
                    break;
                case "Buttons":
                    incAxes.AddRange(INC.GetButtons());
                    break;
                case "All":
                    incAxes.AddRange(INC.GetAxes());
                    incAxes.AddRange(INC.GetButtons());
                    break;
                default:
                    return null;
            }

            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset") [0];
            SerializedObject obj = new SerializedObject(inputManager);
            SerializedProperty axesArray = obj.FindProperty("m_Axes");
            List<string> axesArrayList = new List<string>();
            for (int i = 0; i < axesArray.arraySize; ++i)
            {
                SerializedProperty axis = axesArray.GetArrayElementAtIndex(i);
                string name = axis.FindPropertyRelative("m_Name").stringValue;
                axesArrayList.Add(name);
            }
            for (int i = 0; i < incAxes.Count; i++)
            {
                bool contains = axesArrayList.Any(t => t == incAxes[i]);
                if (contains)
                    continue;
                else
                    missingAxes.Add(incAxes[i]);
            }
            return missingAxes.ToArray();
        }

        /// <summary>
        /// Verify project tags and return missing require Unreal FPS tags. 
        /// </summary>
        /// <returns></returns>
        public static string[] GetMissingTags()
        {
            string[] editorTags = InternalEditorUtility.tags;
            string[] uTags = TNC.GetTags();
            List<string> missingTags = new List<string>();
            for (int i = 0; i < uTags.Length; i++)
            {
                bool contain = false;
                for (int j = 0; j < editorTags.Length; j++)
                {
                    if (uTags[i] == editorTags[j])
                    {
                        contain = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (!contain)
                {
                    missingTags.Add(uTags[i]);
                }
            }
            return missingTags.ToArray();
        }

        /// <summary>
        /// Verify project layers and return missing require Unreal FPS layers. 
        /// </summary>
        /// <returns></returns>
        public static string[] GetMissingLayers()
        {
            string[] editorLayers = InternalEditorUtility.layers;
            string[] requireLayers = LNC.GetLayers();
            List<string> missingLayers = new List<string>();
            for (int i = 0; i < requireLayers.Length; i++)
            {
                bool contain = editorLayers.Any(t => t == requireLayers[i]);
                if (contain)
                    continue;
                else
                    missingLayers.Add(requireLayers[i]);
            }
            return missingLayers.ToArray();
        }

        /// <summary>
        /// Auto add all require Unreal FPS missing tags in project settings.
        /// </summary>
        public static void AddMissingTags()
        {
            string[] missingTags = GetMissingTags();
            if (missingTags != null && missingTags.Length == 0)
            {
                UDisplayDialogs.Message("Tags", "Your project has all the necessary tags!");
                return;
            }
            else if (missingTags == null)
            {
                return;
            }

            for (int i = 0; i < missingTags.Length; i++)
            {
                InternalEditorUtility.AddTag(missingTags[i]);
            }
        }

        /// <summary>
        /// Add spaces to this line
        /// 
        ///     Note: Spaces are added only between  capital letters.
        /// </summary>
        /// <returns>Return string</returns>
        public static string AddSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}