﻿using AutoMapper;
using TMS.API.DTOs;
using TMS.Core;

namespace TMS.API.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, PostUserDTO>()
               .ReverseMap();

            CreateMap<TaskAssignment, TaskAssignmentResponseDTO>()
              .ReverseMap();

            CreateMap<TaskAssignment, TaskAssignmentRequestDTO>()
              .ReverseMap();

            CreateMap<Tasks, TaskResponseDTO>()
              .ReverseMap();

            CreateMap<Tasks, TaskRequestDTO>()
              .ReverseMap();

            CreateMap<TimeSheetApprovals, TimeSheetApprovalResponseDTO>()
                .ReverseMap();

            CreateMap<TimeSheetApprovals, TimeSheetApprovalRequestDTO>()
                .ReverseMap();

            CreateMap<TimeSheetMaster, TimesheetMasterResponseDTO>()
                .ReverseMap();

            CreateMap<TimeSheetDetails, TimeSheetDetailRequestDTO>()
                .ReverseMap();

            CreateMap<TimeSheetDetails, TimeSheetDetailResponseDTO>()
                .ReverseMap();

            CreateMap<Invoice, InvoiceResponseDTO>()
                .ReverseMap();

            CreateMap<Invoice, InvoiceRequestDTO>()
               .ReverseMap();

            CreateMap<InvoiceDetails, InvoiceDetailResponseDTO>()
              .ReverseMap();

            CreateMap<InvoiceDetails, InvoiceDetailRequestDTO>()
            .ReverseMap();
        }
    }
}
