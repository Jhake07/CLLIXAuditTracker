import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BookingReservation } from '../../_interfaces/booking-reservation';
import { NoResultsComponents } from '../../shared/no-results/no-results';
import { StatusClassPipe } from '../../_pipes/status-class-pipe';
import { CommonModule } from '@angular/common';
import { SortableHeaderDirective } from '../../_directives/sortable-header.directive';

@Component({
  selector: 'app-booking-reservation-table-list',
  imports: [CommonModule, SortableHeaderDirective, NoResultsComponents, StatusClassPipe],
  templateUrl: './booking-reservation-table-list.html',
  styleUrl: './booking-reservation-table-list.css',
})
export class BookingReservationTableListComponent {
  @Input() reservationList: BookingReservation[] = [];

  @Input() sortColumn: string = '';
  @Input() sortDirection: 'asc' | 'desc' | '' = '';
  @Output() sort = new EventEmitter<{ column: string; direction: '' | 'asc' | 'desc' }>();

  @Output() view = new EventEmitter<BookingReservation>();
  @Output() edit = new EventEmitter<BookingReservation>();
  @Output() cancel = new EventEmitter<number>();

  getSortIcon(column: string): string {
    if (this.sortColumn !== column || this.sortDirection === '') {
      return 'bi-sort-alpha-down text-muted';
    }
    return this.sortDirection === 'asc' ? 'bi-sort-up text-primary' : 'bi-sort-down text-primary';
  }
}
