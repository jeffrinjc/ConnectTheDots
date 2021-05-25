using System.Collections.Generic;
using UnityEngine;

namespace ConnectTheDots.Common
{
    /// <summary>
    /// Class to hold the references of any MonoBehaviours in the game
    /// It uses FindObjectOfType once and caches the reference.
    /// </summary>
    public class ReferenceFactory
    {
        private static Dictionary<System.Type, MonoBehaviour> _references;

        /// <summary>
        /// Gets the MonoBehaviour of type T.
        /// </summary>
        public static T Get<T>() where T : MonoBehaviour
        {
            _references = _references ?? new Dictionary<System.Type, MonoBehaviour>();

            var v_manager = default(T);
            if (_references.ContainsKey(typeof(T)))
            {
                v_manager = (T)_references[typeof(T)];
            }
            else
            {
                v_manager = GameObject.FindObjectOfType<T>();
                if (v_manager != null)
                {
                    _references.Add(typeof(T), v_manager);
                }
            }
            return v_manager;
        }

        internal static void Refresh()
        {
            if (_references != null)
            {
                _references.Clear();
            }
        }
    }
}