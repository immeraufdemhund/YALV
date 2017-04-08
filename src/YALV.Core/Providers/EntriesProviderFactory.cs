using YALV.Core.Domain;

namespace YALV.Core.Providers
{
    public static class EntriesProviderFactory
    {
        public static AbstractEntriesProvider GetProvider(EntriesProviderType type = EntriesProviderType.Xml)
        {
            switch (type)
            {
                case EntriesProviderType.Xml:
                    return new XmlEntriesProvider();

                case EntriesProviderType.Sqlite:
                    return new SqliteEntriesProvider();

                case EntriesProviderType.MsSqlServer:
                    return new MsSqlServerEntriesProvider();

                case EntriesProviderType.Text:
                    return new FileEntriesProvider();

                default:
                    return new NotImplementedEntriesProvider(type);
            }
        }
    }
}