import { Component, AfterViewInit } from '@angular/core';

declare var bootstrap: any;

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  templateUrl: './confirm-dialog.html',
  styleUrls: ['./confirm-dialog.css'],
})
export class ConfirmDialog implements AfterViewInit {
  title = 'Confirm';
  message = 'Are you sure?';
  btnOkText = 'Yes';
  btnCancelText = 'No';

  private modal: any;

  ngAfterViewInit() {
    const modalEl = document.getElementById('confirmModal');
    this.modal = new bootstrap.Modal(modalEl);
  }

  open() {
    this.modal?.show();
  }

  confirm() {
    console.log('Confirmed!');
    this.modal?.hide();
  }
}
