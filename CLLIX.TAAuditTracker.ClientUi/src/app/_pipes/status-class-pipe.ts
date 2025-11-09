import { Pipe, PipeTransform } from '@angular/core';
import { GenericStatus } from '../_enums/generic-status.enums';

@Pipe({
  name: 'statusClass',
  standalone: true,
})
export class StatusClassPipe implements PipeTransform {
  transform(status: string): string {
    switch (status) {
      case GenericStatus.Open:
        return 'bg-info text-dark';
      case GenericStatus.InProgress:
        return 'bg-warning text-dark';
      case GenericStatus.Completed:
        return 'bg-success';
      case GenericStatus.Cancelled:
        return 'bg-danger';
      case GenericStatus.Inactive:
        return 'bg-secondary';
      case GenericStatus.Active:
        return 'bg-info text-dark';
      default:
        return 'bg-secondary';
    }
  }
}
