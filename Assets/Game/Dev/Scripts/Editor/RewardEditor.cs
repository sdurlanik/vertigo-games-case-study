using UnityEditor;
using UnityEngine;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Editor
{
    [CustomEditor(typeof(RewardSO))]
    public class RewardEditor : UnityEditor.Editor
    {
        private RewardSO _rewardSo;
        private int _zoneNumber = 1; // Default zone number for debugging
        private float _zoneRewardMultiplier = 0.01f; // Default zone reward multiplier for debugging

        private void OnEnable()
        {
            _rewardSo = (RewardSO)target;
        }

        public override void OnInspectorGUI()
        {
            DrawIconSection();
            GUILayout.Space(20);

            DrawRewardProperties();
            GUILayout.Space(20);

            DrawDebuggingSection();
        }

        private void DrawIconSection()
        {
            EditorGUILayout.LabelField("Reward Icon Display", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            if (_rewardSo.icon != null)
            {
                DisplayIconWithAspectRatio();
            }
            else
            {
                EditorGUILayout.HelpBox("No Icon Assigned", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();
        }

        private void DisplayIconWithAspectRatio()
        {
            Texture2D sprite = AssetPreview.GetAssetPreview(_rewardSo.icon);
            if (sprite != null)
            {
                var aspectRatio = (float)sprite.width / sprite.height;
                var height = 100f;
                var width = height * aspectRatio;

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                Rect rect = GUILayoutUtility.GetRect(width, height);
                GUI.DrawTexture(rect, sprite, ScaleMode.ScaleToFit);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        private void DrawRewardProperties()
        {
            EditorGUILayout.LabelField("Reward Properties", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            base.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }

        private void DrawDebuggingSection()
        {
            EditorGUILayout.LabelField("Debugging", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            DrawZoneWeightCalculation();
            DrawZoneMultiplier();

            EditorGUILayout.EndVertical();
        }

        private void DrawZoneWeightCalculation()
        {
            const string debugText = "Calculate the weight and value of the reward for a specific zone number. (Debugging purposes only)";
            EditorGUILayout.HelpBox(debugText, MessageType.Info);

            _zoneNumber = EditorGUILayout.IntField("Zone Number", _zoneNumber);
            EditorGUILayout.LabelField("Calculated Weight", _rewardSo.CalculateWeight(_zoneNumber).ToString("N1"));
        }

        private void DrawZoneMultiplier()
        {
            GUILayout.Space(20);
            const string debugText = "Zone reward value multiplier can be changed in Zone ScriptableObjects. The default value is 0.01";
            EditorGUILayout.HelpBox(debugText, MessageType.Info);

            _zoneRewardMultiplier = EditorGUILayout.FloatField("Zone Value Multiplier", _zoneRewardMultiplier);
            EditorGUILayout.LabelField("Calculated Value", (_rewardSo.CalculateRewardAmount(_zoneRewardMultiplier * _zoneNumber).ToString("N0")));
        }
    }
}
