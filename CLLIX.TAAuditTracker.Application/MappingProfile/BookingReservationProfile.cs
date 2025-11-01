using AutoMapper;
using CLLIX.TAAuditTracker.Application.DTOs;
using CLLIX.TAAuditTracker.Application.Features.BookingReservation.Commands.Create;
using CLLIX.TAAuditTracker.Domain;

namespace CLLIX.TAAuditTracker.Application.MappingProfile
{
    public class BookingReservationProfile : Profile
    {
        public BookingReservationProfile()
        {
            CreateMap<BookingReservationDto, BookingReservation>().ReverseMap();
            CreateMap<CreateBookingReservationCommand, BookingReservation>();
            //.ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
