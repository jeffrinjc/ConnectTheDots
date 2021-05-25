using UnityEngine;

namespace ConnectTheDots.UI
{
    /// <summary>
    /// Base class of all the UI in the game 
    /// </summary>
    public class BaseUIPanel : MonoBehaviour
    {
        [SerializeField] protected GameObject _panelRoot;

        public virtual void Show()
        {
            _panelRoot.SetActive(true);
        }

        public virtual void Close()
        {
            _panelRoot.SetActive(false);
        }
    }
}