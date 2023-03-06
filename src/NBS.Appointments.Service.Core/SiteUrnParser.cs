namespace NBS.Appointments.Service.Core
{
    public static class SiteUrnParser
    {
        public static SiteUrn Parse(string urn)
        {
            var parts = urn.Split(":");
            if(parts.Length != 2)
                throw new FormatException("");
            return new SiteUrn(parts[0], parts[1]);
        }
    }

    public record SiteUrn(string Scheme, string Identifier);
}