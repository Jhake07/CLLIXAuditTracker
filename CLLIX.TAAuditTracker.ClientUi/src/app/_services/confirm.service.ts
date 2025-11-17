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
        <div class="modal fade" id="confirmModal" tabindex="-1" aria-labelledby="confirmModalLabel">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="confirmModalLabel">${title}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
              <div class="modal-body">
                <p>${message}</p>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-success" id="btnConfirm">${btnOkText}</button>
                <button type="button" class="btn btn-danger" data-bs-dismiss="modal" id="btnCancel">${btnCancelText}</button>
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
        // Blur focused element to avoid accessibility warning
        if (document.activeElement instanceof HTMLElement) {
          document.activeElement.blur();
        }

        modal.hide();
        modalEl.addEventListener('hidden.bs.modal', () => {
          modalContainer.remove();
        });
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
