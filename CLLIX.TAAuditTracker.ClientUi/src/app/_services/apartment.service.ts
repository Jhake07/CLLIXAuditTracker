import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../_env/environment.dev';
import { Observable } from 'rxjs';
import { ApartmentProperty } from '../_interfaces/apartment-property';
import { CustomResultResponse } from '../_interfaces/customresultresponse.model';

@Injectable({ providedIn: 'root' })
export class ApartmentPropertyService {
  private readonly apiUrl = `${environment.apiUrl}apartmentproperty`; //

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApartmentProperty[]> {
    return this.http.get<ApartmentProperty[]>(this.apiUrl);
  }

  getById(id: number): Observable<ApartmentProperty> {
    return this.http.get<ApartmentProperty>(`${this.apiUrl}/${id}`);
  }

  create(property: ApartmentProperty): Observable<CustomResultResponse> {
    return this.http.post<CustomResultResponse>(this.apiUrl, property);
  }

  update(id: number, property: ApartmentProperty): Observable<CustomResultResponse> {
    //const createdBy = this.accountService.getLoggedInUserId();
    // const createdBy = 'Jake Test';
    // const enrichedPayload = { ...property, createdBy };
    const payload = {
      newApartmentName: property.apartmentName,
      apartmentStatus: property.apartmentStatus,
    };

    console.log(payload);
    return this.http.put<CustomResultResponse>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
