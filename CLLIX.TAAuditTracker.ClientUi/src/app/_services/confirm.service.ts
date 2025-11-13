import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ConfirmService {
  confirm(
    title = 'Confirm',
    message = 'Are you sure?',
    btnOkText = 'Yes',
    btnCancelText = 'No'
  ): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      const modalContainer = document.createElement('div');
      modalContainer.innerHTML = `
        <div class="modal fade" id="confirmModal" tabindex="-1" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">${title}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body"><p>${message}</p></div>
              <div class="modal-footer">
                <button id="btnConfirm" class="btn btn-success">${btnOkText}</button>
                <button id="btnCancel" class="btn btn-danger" data-bs-dismiss="modal">${btnCancelText}</button>
              </div>
            </div>
          </div>
        </div>
      `;
      document.body.appendChild(modalContainer);

      const modalEl = modalContainer.querySelector('#confirmModal') as HTMLElement;
      const modal = new (window as any).bootstrap.Modal(modalEl);
      modal.show();

      const cleanup = () => {
        modal.hide();
        modalEl.addEventListener('hidden.bs.modal', () => modalContainer.remove());
      };

      modalContainer.querySelector('#btnConfirm')?.addEventListener('click', () => {
        resolve(true);
        cleanup();
      });

      modalContainer.querySelector('#btnCancel')?.addEventListener('click', () => {
        resolve(false);
        cleanup();
      });
    });
  }
}
