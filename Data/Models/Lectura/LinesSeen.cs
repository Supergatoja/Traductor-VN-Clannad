using SeenExtractor.Data.Models.Extenciones.ChangeEncoding.Strings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SeenExtractor.Data.Models.Lectura
{
    public class LinesSeen : INotifyPropertyChanged
    {
        private string _Dialog = string.Empty;
        private bool _Excede = false;
        private int _Length = 0;
        public required int Line { get; set; }
        public required string Character { get; set; }
        public bool IsFocus { get; set; }
        public bool Japones { get; set; }
        public required string Dialog
        {
            get => _Dialog;
            set
            {
                _Dialog = value;
                OnPropertyChanged();
                Limite();
            }
        }
        public required int Length { get => _Length; set { _Length = value; Limite(); } }
        public bool Excede { get => _Excede; set { _Excede = value; OnPropertyChanged(); } }

        public void Limite()
        {
            if (!Japones)
            {
                if (_Dialog.Length > Length & Length > 0)
                {
                    Excede = true;
                }
                else Excede = false;
            }
            else
            {
                var bytes = _Dialog.LengthBytesCP932();
                if (bytes > Length & Length > 0)
                {
                    Excede = true;
                }
                else Excede = false;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
