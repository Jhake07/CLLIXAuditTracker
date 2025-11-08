import { BaseEntity } from './base-entity';

export interface TravelAgency extends BaseEntity {
  agencyName: string;
  agencyCode: string;
}
