using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Hud;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxContoller _dialogBox;

        public DialogData FinalData
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound: return _bound;
                    case Mode.External: return _external.Data;
                }
                return null;
            }
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        public void Show()
        {
            if (_dialogBox == null)
            {
                _dialogBox = FindObjectOfType<DialogBoxContoller>();
            }
            _dialogBox.ShowDialog(FinalData);
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}