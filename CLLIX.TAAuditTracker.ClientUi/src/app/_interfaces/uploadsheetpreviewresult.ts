import { BookingReservation } from './booking-reservation';

export interface UploadSheetPreviewResult {
  sheetName: string;
  totalRows: number;
  validRows: number;
  invalidRows: number;
  errors: RowValidationError[];
  previewRows: BookingReservation[]; //
  backfillSummary?: string; // ✅ updated: summary message instead of list
}

export interface RowValidationError {
  rowNumber: number;
  validationErrors: Record<string, string[]>;
}

export interface UploadSheetSaveResult {
  message: string;
  summary: UploadSheetPreviewResult;
}
