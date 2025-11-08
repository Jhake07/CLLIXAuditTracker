import { Routes } from '@angular/router';
import { ApartmentComponent } from './apartment/apartment';
import { BookingReservationComponent } from './booking-reservation/booking-reservation';
import { AppUserComponent } from './app-user/app-user';
import { TravelAgencyAgentComponent } from './travel-agency-agent/travel-agency-agent';
import { TravelAgencyComponent } from './travel-agency/travel-agency';

export const routes: Routes = [
  { path: 'apartment', component: ApartmentComponent },
  { path: 'booking-reservation', component: BookingReservationComponent },
  { path: 'app-user', component: AppUserComponent },
  { path: 'travel-agency', component: TravelAgencyComponent },
  { path: 'travel-agency-agent', component: TravelAgencyAgentComponent },
];
