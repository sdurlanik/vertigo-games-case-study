using System;
using UnityEngine;

namespace VertigoGamesCaseStudy.Core.Utility
{
    /// <summary>
    /// A simple singleton class.
    /// </summary>
    /// <typeparam name="T">This should always be the same class that inherits from this class.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    throw new InvalidOperationException("[Singleton::(get)Instance] Cannot get instance while not playing.");
                }
#endif
                if (m_Instance == null)
                {
                    // Debug.LogError($"Singleton instance of {typeof(T)} is null. Ensure the script is attached to an active GameObject.");
                }

                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
            }
            else if (m_Instance != this)
            {
                Debug.LogError($"[Singleton<{typeof(T)}>::Awake] Another instance of {typeof(T)} with name {name} was found and destroyed.");
                Destroy(gameObject);
            }
        }
    }
}
