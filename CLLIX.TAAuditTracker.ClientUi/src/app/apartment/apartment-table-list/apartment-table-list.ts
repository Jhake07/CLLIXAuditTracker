import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule, NgClass } from '@angular/common';
import { ApartmentProperty } from '../../_interfaces/apartment-property';
import { NoResultsComponents } from '../../shared/no-results/no-results';
import { StatusClassPipe } from '../../_pipes/status-class-pipe';
import { SortableHeaderDirective } from '../../_directives/sortable-header.directive';

@Component({
  selector: 'app-apartment-table-list',
  standalone: true,
  imports: [CommonModule, NoResultsComponents, StatusClassPipe, NgClass, SortableHeaderDirective],
  templateUrl: './apartment-table-list.html',
  styleUrl: './apartment-table-list.css',
})
export class ApartmentTableListComponent {
  @Input() apartmentList: ApartmentProperty[] = [];

  @Output() edit = new EventEmitter<ApartmentProperty>();
  @Output() view = new EventEmitter<ApartmentProperty>();

  @Input() sortColumn: string = '';
  @Input() sortDirection: 'asc' | 'desc' | '' = '';
  @Output() sort = new EventEmitter<{ column: string; direction: '' | 'asc' | 'desc' }>();

  getSortIcon(column: string): string {
    if (this.sortColumn !== column || this.sortDirection === '') {
      return 'bi-sort-alpha-down text-muted';
    }
    return this.sortDirection === 'asc' ? 'bi-sort-up text-primary' : 'bi-sort-down text-primary';
  }
}
