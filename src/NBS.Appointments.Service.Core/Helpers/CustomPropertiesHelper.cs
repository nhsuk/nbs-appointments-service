using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;

namespace NBS.Appointments.Service.Core.Helpers
{
    public interface ICustomPropertiesHelper
    {
        Dictionary<string, string> BuildCustomProperties(string properties);
    }

    public class CustomPropertiesHelper : ICustomPropertiesHelper
    {
        private readonly QflowOptions _options;

        public CustomPropertiesHelper(IOptions<QflowOptions> options)
        {
            _options = options.Value;
        }

        public Dictionary<string, string> BuildCustomProperties(string properties)
        {
            var customPropertiesDescriptor = QflowCustomPropertiesDescriptor.FromString(properties);

            var customProperties = new Dictionary<string, string>
            {
                { _options.CallCentreBookingFlagId, customPropertiesDescriptor.CallCentreBooking },
                { _options.AppBookingFlagId, customPropertiesDescriptor.AppBooking }
            };

            if (customPropertiesDescriptor.CallCentreEmailAddress != null)
                customProperties.Add(_options.CallCentreEmailFlagId, customPropertiesDescriptor.CallCentreEmailAddress);

            return customProperties;
        }
    }
}
