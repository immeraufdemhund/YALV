using System;

namespace YALV.Core.Domain
{
    [Serializable]
    public class FileItem : BindableObject
    {
        public EntriesProviderType EntriesType
        {
            get { return _entriesType; }
            set { _entriesType = value; RaisePropertyChanged(nameof(EntriesType)); }
        }
        private EntriesProviderType _entriesType;

        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked == value)
                    return;
                _checked = value;
                RaisePropertyChanged(nameof(Checked));
            }
        }
        private bool _checked;
        public static string PROP_Checked = "Checked";

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged(nameof(FileName));
            }
        }
        private string _fileName;

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisePropertyChanged(nameof(Path));
            }
        }
        private string _path;

        public FileItem(string _fileName, string _path)
        {
            Checked = false;
            FileName = _fileName;
            Path = _path;
            EntriesType = EntriesProviderType.Xml;
        }
    }
}
