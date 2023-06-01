using NBS.Appointments.Service.Core.Dtos.Qflow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBS.Appointments.Service.Helpers
{
    public class AppointmentsHelper
    {
        public static IList<AppointmentResponse> FilterPastCustomerAppointments(IList<AppointmentResponse> appointments, bool includePastAppointments)
        {
            return includePastAppointments
                ? appointments
                : appointments.Where(x => DateTime.Compare(x.AppointmentDate.ToUniversalTime(), DateTime.Today.ToUniversalTime()) >= 0).ToList();
        }
    }
}
