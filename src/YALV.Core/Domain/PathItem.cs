using System;

namespace YALV.Core.Domain
{
    public class PathItem : BindableObject
    {
        public EntriesProviderType EntriesType
        {
            get { return _entriesType; }
            set { _entriesType = value; RaisePropertyChanged(nameof(EntriesType));}
        }
        private EntriesProviderType _entriesType;

        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(nameof(Name)); }
        }
        private string _name;

        public string Path
        {
            get { return _path; }
            set { _path = value; RaisePropertyChanged(nameof(Path)); }
        }
        private string _path;

        public PathItem()
            : this(string.Empty, string.Empty)
        {
        }

        public PathItem(string name, string path)
        {
            _name = name;
            _path = path;
            EntriesType = EntriesProviderType.Xml;
        }
    }
}
