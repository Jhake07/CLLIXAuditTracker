import { BaseEntity } from './base-entity';
import { TravelAgency } from './travel-agency';

export interface TravelAgencyAgent extends BaseEntity {
  agentName: string;
  agentCode: string;
  travelAgencyId: number;
  travelAgency: TravelAgency;
}
