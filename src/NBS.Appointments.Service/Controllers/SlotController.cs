using Microsoft.AspNetCore.Mvc;
using NBS.Appointments.Service.Core.Interfaces.Services;
using System;

namespace NBS.Appointments.Service.Controllers
{
    [Route("slot")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly IQflowService _qflowService;

        public SlotController(IQflowService qflowService)
        {
            _qflowService = qflowService
                ?? throw new ArgumentNullException(nameof(qflowService));
        }
    }
}
