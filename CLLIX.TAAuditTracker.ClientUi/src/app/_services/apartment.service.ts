import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApartmentProperty } from '../_interfaces/apartment-property';

@Injectable({ providedIn: 'root' })
export class ApartmentPropertyService {
  private readonly apiUrl = '/api/apartmentproperty'; //

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApartmentProperty[]> {
    return this.http.get<ApartmentProperty[]>(this.apiUrl);
  }

  getById(id: number): Observable<ApartmentProperty> {
    return this.http.get<ApartmentProperty>(`${this.apiUrl}/${id}`);
  }

  create(property: ApartmentProperty): Observable<ApartmentProperty> {
    return this.http.post<ApartmentProperty>(this.apiUrl, property);
  }

  update(id: number, property: ApartmentProperty): Observable<ApartmentProperty> {
    return this.http.put<ApartmentProperty>(`${this.apiUrl}/${id}`, property);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
