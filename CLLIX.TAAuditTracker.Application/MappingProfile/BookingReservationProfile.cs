using AutoMapper;
using CLLIX.TAAuditTracker.Application.DTO;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Upload;
using CLLIX.TAAuditTracker.Domain;

namespace CLLIX.TAAuditTracker.Application.MappingProfile
{
    public class BookingReservationProfile : Profile
    {
        public BookingReservationProfile()
        {
            CreateMap<BookingReservationDto, BookingReservation>().ReverseMap();
            CreateMap<CreateBookingReservationCommand, BookingReservation>();
            CreateMap<CreateBookingReservationFromUploadCommand, BookingReservation>();
            //.ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
